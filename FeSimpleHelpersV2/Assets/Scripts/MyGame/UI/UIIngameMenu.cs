using FeSimpleHelpers.Core;
using FeSimpleHelpers.UI;
using MyGame.General;
using UnityEngine;

namespace MyGame.Ingame.UI
{
	public class UIIngameMenu : UIWindow
	{
		[SerializeField]
		private UIWindow uiKeyconfig;
		[SerializeField]
		private UIWindow uiOptions;
		protected override void Awake()
		{
			base.Awake();
			//options = FindObjectOfType<UIOptions>();
			IngameManager.OnGamePaused += ChangedPauseState;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			IngameManager.OnGamePaused -= ChangedPauseState;
		}

		private void ChangedPauseState(bool obj)
		{
			if (obj)
				Show();
		}

		public override void Back()
		{
			base.Back();
			IngameManager.Get().TryContinueFromPause();
		}

		public void KeyOptions()
		{
			uiKeyconfig.SetOnBackCallback(Show).Show();
		}

		public void Options()
		{
			uiOptions.SetOnBackCallback(Show).Show();
		}

		public void ExitToMainMenu()
		{
			GameManager.Get().GoToMainMenu(2, LoaderManager.LoadingType.Simple);
		}

		public void ExitToDOS()
		{
			GameManager.Get().ExitGame();
		}
	}
}