using System;
using FeSimpleHelpers.Core;
using FeSimpleHelpers.ScenesHandling;
using MyGame.Input;
using UnityEngine;

namespace MyGame.General
{
	public class CutsceneController : MonoBehaviour
	{
		[SerializeField] private SceneConfig cutsceneConfig;

		public static Action<CutsceneController> OnCutsceneStarted;
		public static Action<CutsceneController> OnCutsceneEnded;

		void Start()
		{
			PlayCutscene();
			InputConfig.Get().EnableMenu();
		}

		public void PlayCutscene()
		{
			OnCutsceneStarted?.Invoke(this);
		}

		public void EndCutscene()
		{
			OnCutsceneEnded?.Invoke(this);
			GameManager.Get().GoToScene(cutsceneConfig.nextSceneConfig, 0.1f, LoaderManager.LoadingType.NoInterface);
		}
	}
}