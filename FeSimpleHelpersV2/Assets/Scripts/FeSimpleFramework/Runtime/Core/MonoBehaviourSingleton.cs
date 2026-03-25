using UnityEngine;

namespace FeSimpleHelpers.Core
{
	/// <summary>
	/// Base class for MonoBehaviourSingleton
	/// </summary>
	public class BehaviourSingleton : MonoBehaviour
	{
		/// Indicates if the singleton should persist between scenes.
		public bool persistent = false;

		public void TrySetDontDestroyOnLoad()
		{
			if (persistent)
			{
				if (transform.parent != null)
					transform.SetParent(null);
				DontDestroyOnLoad(this);
			}
		}

		public virtual void AwakeSingleton() { }
		public virtual void OnDestroySingleton() { }
	}

	public class MonoBehaviourSingleton<T> : BehaviourSingleton where T : BehaviourSingleton
	{
		public static T Instance { get; private set; } = null;

		public static T Get() { return Instance; }

#if UNITY_EDITOR
		public static T Find()
		{
			if (!Application.isPlaying)
				Instance = FindAnyObjectByType<T>();
			return Instance;
		}
#endif

		public static bool Exists { get { return Instance != null; } }
		public static DerivedType Get<DerivedType>() where DerivedType : T { return Get() as DerivedType; }

		protected void Awake()
		{
			AwakeSingleton();
		}

		protected void OnDestroy()
		{
			OnDestroySingleton();
		}

		public override void AwakeSingleton()
		{
			if (Instance == null)
			{
				Instance = this as T;
				TrySetDontDestroyOnLoad();
				OnInitialized();
			}
			else if (Instance != this)
				Destroy(gameObject);
		}

		public override void OnDestroySingleton()
		{
			if (this == Instance)
			{
				OnFinalized();
				Instance = null;
			}
		}

		public virtual void OnInitialized() { }
		public virtual void OnFinalized() { }
	}
}