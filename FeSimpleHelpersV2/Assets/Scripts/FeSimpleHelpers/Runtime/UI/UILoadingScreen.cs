using System;
using System.Collections.Generic;
using FeSimpleHelpers.Core;
using UnityEngine;
using UnityEngine.UI;

namespace FeSimpleHelpers.UI
{
	/// <summary>
	/// TODO This needs polishing
	/// </summary>
	public class UILoadingScreen : MonoBehaviourSingleton<UILoadingScreen>
	{
		[SerializeField]
		private UILoadingNoInterface noInterface;
		[SerializeField]
		private UILoadingSimple simple;
		[SerializeField] private UILoadingBigLogo bigLogo;
		[SerializeField] private UILoadingMulti multi;
		public bool loading;

		Dictionary<LoaderManager.LoadingType, UILoadingState> loadingStates = new Dictionary<LoaderManager.LoadingType, UILoadingState>();

		UILoadingState loadingState = null;

		public override void AwakeSingleton()
		{
			base.AwakeSingleton();
			LoaderManager.OnLoadingStart += LoadingStart;
			LoaderManager.OnLoadingEnd += LoadingEnd;

			loadingStates.Add(LoaderManager.LoadingType.NoInterface, noInterface);
			loadingStates.Add(LoaderManager.LoadingType.Simple, simple);
			loadingStates.Add(LoaderManager.LoadingType.BigLogo, bigLogo);
			loadingStates.Add(LoaderManager.LoadingType.Multi, multi);
		}

		public override void OnDestroySingleton()
		{
			base.OnDestroySingleton();
			LoaderManager.OnLoadingStart -= LoadingStart;
			LoaderManager.OnLoadingEnd -= LoadingEnd;
		}

		void LoadingStart(LoaderManager lm)
		{
			if (loadingState != null)
				Debug.LogError("This shouldn't happen");

			loadingState = loadingStates[lm.loadingType];
			loadingState.Start();
			loading = true;
		}

		void Update()
		{
			if (!loading)
				return;
			loadingState.Update(LoaderManager.Get().loadingProgress);
		}
		void LoadingEnd(LoaderManager lm)
		{
			loadingState.End();
			loading = false;
			loadingState = null;
		}
	}

	[Serializable]
	public abstract class UILoadingState
	{
		public GameObject panel;
		public virtual void Start() { panel.SetActive(true); }
		public virtual void Update(float loadingProgress) { }
		public virtual void End() { panel.SetActive(false); }
	}
	[Serializable]
	public class UILoadingNoInterface : UILoadingState
	{
	}
	[Serializable]
	public class UILoadingSimple : UILoadingState
	{
		public Canvas canvas;
		public Image image;
		public Color imageColor;
		public float duration = 0;
		public float sinSpeed = 2;
		public override void Start()
		{
			base.Start();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			duration = 0;
		}

		public override void Update(float loadingProgress)
		{
			base.Update(loadingProgress);
			duration += Time.deltaTime;
			imageColor.a = (MathF.Sin(duration * sinSpeed) + 1) / 2f;
			image.color = imageColor;
		}
	}
	[Serializable]
	public class UILoadingBigLogo : UILoadingState
	{
		public Canvas canvas;
		public Image image;

		public override void Start()
		{
			base.Start();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			//TODO fix this
			//canvas.renderMode = RenderMode.ScreenSpaceCamera;
			//Camera c = Camera.main;
			//canvas.worldCamera = c;
			//canvas.planeDistance = 1;
		}

		public override void Update(float loadingProgress)
		{
			base.Update(loadingProgress);
			image.fillAmount = loadingProgress;
		}
	}
	[Serializable]
	public class UILoadingMulti : UILoadingState
	{

	}
}