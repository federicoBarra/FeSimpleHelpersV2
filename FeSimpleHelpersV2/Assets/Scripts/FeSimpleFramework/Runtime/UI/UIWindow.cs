#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using FeSimpleHelpers.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FeSimpleHelpers.UI
{
	/// <summary>
	/// Basic Window functionality.
	/// </summary>
	[RequireComponent(typeof(Canvas))]
	[RequireComponent(typeof(CanvasGroup))]
	public class UIWindow : MonoBehaviour
	{
		[SerializeField]
		bool modifyGameObjectActiveState = true;
		[SerializeField]
		bool turnOffOnStart = true;
		[SerializeField]
		bool addCloseButtonOutsideOfWindow = false;
		[SerializeField]
		bool focuseable = false;


		protected UIWindowTransition[] transitions;
		protected UIWindowTransition[] Transitions { get { if (transitions == null) transitions = GetComponents<UIWindowTransition>(); return transitions; } }

		protected RectTransform rt;
		public RectTransform RectTransform { get { if (rt == null) rt = GetComponent<RectTransform>(); return rt; } }

		protected Canvas canvas;
		public Canvas Canvas { get { if (canvas == null) canvas = GetComponent<Canvas>(); return canvas; } }

		protected CanvasGroup cg;
		public CanvasGroup Cg { get { if (cg == null) cg = GetComponent<CanvasGroup>(); return cg; } }

		public bool IsUIWindowActive { get; private set; }

		public event Action OnPreShow;
		public event Action OnPreHide;
		public event Action OnPostShow;
		public event Action OnPostHide;
		public static event Action<UIWindow> OnFocusIntent;


		protected virtual void Awake()
		{
			Reset();
		}

		protected virtual void Reset()
		{
			rt = GetComponent<RectTransform>();
			canvas = GetComponent<Canvas>();
			cg = GetComponent<CanvasGroup>();
		}

		protected virtual void Start()
		{
			if (turnOffOnStart)
				Show(false, true, true);
			else
				Show(true, true, true);

			if (addCloseButtonOutsideOfWindow)
				AddCloseButtonOutsideOfWindow();
		}

		protected virtual void Initialize() { }

		protected virtual void OnEnable() { }
		protected virtual void OnDisable() { }
		protected virtual void Refresh() { }
		protected virtual void DestroyUIContent() { }

		public virtual void Show(bool b, bool instant = false)
		{
			Show(b, instant, false);
		}

		public virtual void Show(bool b, bool instant, bool force)
		{
			if (IsUIWindowActive == b && !force)
			{
				if (focuseable)
					OnFocusIntent?.Invoke(this);
				return;
			}

			IsUIWindowActive = b;

			if (b)
				PreShow();
			else
				PreHide();

			CancelInvoke();

			//stop all possible transitions
			if (Transitions != null)
			{
				for (int i = 0; i < Transitions.Length; i++)
				{
					UIWindowTransition transition = Transitions[i];
					if (transition && transition.Transitioning)
						transition.StopTransition();
				}
			}

			//launch wanted transitions
			float maxTransitionDuration = 0;
			if (Transitions != null && !instant)
			{
				for (int i = 0; i < Transitions.Length; i++)
				{
					UIWindowTransition transition = Transitions[i];

					if (transition && !transition.IsMuted(b))
					{
						transition.Launch(b);
						if (transition.GetDuration(b) > maxTransitionDuration)
							maxTransitionDuration = transition.GetDuration(b);
					}
				}
			}

			Invoke(b ? "PostShow" : "PostHide", maxTransitionDuration);
		}

		public bool IsHidden()
		{
			return Cg.interactable;
		}

		public virtual void ShowInTime(float timeToShow)
		{
			if (!gameObject.activeInHierarchy)
			{
				Debug.LogError("GO Disabled");
				return;
			}
			Invoke("Show", timeToShow);
		}

		public virtual void HideInTime(float timeToHide)
		{
			Invoke("Hide", timeToHide);
		}

		protected virtual void FocusThisWindow()
		{
			Canvas.sortingOrder = 30000;
		}


		#region Show/Hide Handling

		[ContextMenu("Show")]
		public virtual void Show()
		{
			Show(true);
		}

		protected virtual void PreShow()
		{
			//Debug.Log($"{this.name} PreShow");

			if (modifyGameObjectActiveState)
				gameObject.SetActive(true);

			TrySelectPreferedSelectable();

			if (focuseable)
				OnFocusIntent?.Invoke(this);

			Canvas.enabled = true;
			OnPreShow?.Invoke();
			Cg.interactable = true;
		}

		public void TrySelectPreferedSelectable()
		{
			if (preferedSelectable && preferedSelectable != EventSystem.current.currentSelectedGameObject)
				EventSystem.current.SetSelectedGameObject(preferedSelectable);
		}

		protected virtual void PostShow()
		{
			//Debug.Log($"{this.name} PostShow");
			Cg.blocksRaycasts = true;
			Cg.interactable = true;
			OnPostShow?.Invoke();
		}

		[ContextMenu("Hide")]
		public virtual void Hide()
		{
			Show(false);
		}

		protected virtual void PreHide()
		{
			//Debug.Log($"{this.name} PreHide");

			Cg.blocksRaycasts = false;
			Cg.interactable = false;
			OnPreHide?.Invoke();
		}

		protected virtual void PostHide()
		{
			//Debug.Log($"{this.name} PostHide");

			Canvas.enabled = false;
			if (modifyGameObjectActiveState)
				gameObject.SetActive(false);
			OnPostHide?.Invoke();
		}

		public void ForceBackSequence()
		{
			//Debug.Log($"{this.name} OnBack");
			OnBack?.Invoke();
			OnBack = null;
		}

		public virtual void Toggle()
		{
			Toggle(false);
		}

		public virtual void Toggle(bool instant)
		{
			Show(!IsUIWindowActive, instant);
		}

		public void SetDefaultWidget(GameObject gameObject)
		{
			preferedSelectable = gameObject;
		}

		#endregion Show/Hide Handling

		public T GetTransition<T>() where T : UIWindowTransition
		{
			if (Transitions == null)
				return null;
			for (var i = 0; i < Transitions.Length; i++)
			{
				UIWindowTransition transition = Transitions[i];
				if (transition is T variable)
					return variable;
			}
			return null;
		}

		protected GameObject AddCloseButtonOutsideOfWindow()
		{
			GameObject back = new GameObject("Background Exit Button", typeof(RectTransform));
			RectTransform backRT = back.GetComponent<RectTransform>();
			backRT.anchorMin = new Vector2(1, 0);
			backRT.anchorMax = new Vector2(0, 1);
			backRT.pivot = new Vector2(0.5f, 0.5f);
			backRT.sizeDelta = rt.rect.size;
			backRT.SetParent(rt);
			backRT.localScale = Vector3.one;
			backRT.localPosition = Vector3.zero;
			backRT.SetAsFirstSibling();
			if (backRT)
			{
				Button button = backRT.gameObject.AddComponent<Button>();
				button.transition = Selectable.Transition.None;
				button.onClick.AddListener(ClickedOutsideWindow);
				Image i = backRT.GetComponent<Image>();
				if (!i)
					i = backRT.gameObject.AddComponent<Image>();
				i.color = new Color(0, 0, 0, 0);
			}
			return back;
		}

		protected virtual void ClickedOutsideWindow()
		{
			Hide();
		}

		public GameObject preferedSelectable;

		public Action OnBack;

		public UIWindow SetOnBackCallback(Action onBack)
		{
			OnBack = onBack;
			return this;
		}

		public virtual void Back()
		{
			//Debug.Log($"{this.name} Back");

			Hide();
			ForceBackSequence();
		}

		protected virtual void OnDestroy()
		{
		}

		#region Helpers
		public static void ClearContent(Transform content, Transform except = null)
		{
			for (int i = content.childCount - 1; i >= 0; i--)
			{
				Transform child = content.GetChild(i);
				if (child == except)
					continue;
				child.SetParent(null);
				Destroy(child.gameObject);
			}
		}
		#endregion Helpers
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(UIWindow))]
public class UIWindowEditor : Editor
{

	public override void OnInspectorGUI()
	{
		UIWindow w = target as UIWindow;
		if (Application.isPlaying && GUILayout.Button("Toggle (currently: " + (w.IsUIWindowActive ? "Active" : "Inactive") + ")"))
			w.Toggle();

		EditorGUILayout.HelpBox("TODO add transition dropdown. \n Currently search for component UITransition \n TODO Show/Hide events", MessageType.Info);
		DrawDefaultInspector();
	}
}
#endif
