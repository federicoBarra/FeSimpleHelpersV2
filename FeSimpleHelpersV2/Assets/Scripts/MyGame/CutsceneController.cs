using System;
using MyGame.Input;
using UnityEngine;

namespace MyGame.General
{
	public class CutsceneController : MonoBehaviour
	{
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
			GameManager.Get().ContinueGame();
		}
	}
}