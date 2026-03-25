using FeSimpleHelpers.UI;
using MyGame.General;
using MyGame.Persistence;
using UnityEngine;

namespace MyGame.UI
{
	public class UIMainMenu : UIWindow
	{
		[SerializeField]
		private UIProfileHandling uiProfiles;
		[SerializeField]
		private UIWindow uiKeyconfig;
		[SerializeField]
		private UIWindow uiOptions;
		[SerializeField]
		private UIWindow uiCredits;

		public void Play()
		{
			if (UserProfiles.Get().ExistAnyProfile)
				GameManager.Get().StartNewGame();
			else
				uiProfiles.ShowProfileSelection();
		}

		public void KeyOptions()
		{
			uiKeyconfig.SetOnBackCallback(Show).Show();
		}

		public void Options()
		{
			uiOptions.SetOnBackCallback(Show).Show();
		}

		public void Credits()
		{
			uiCredits.SetOnBackCallback(Show).Show();
		}

		public void Quit()
		{
			GameManager.Get().ExitGame();
		}
	}
}