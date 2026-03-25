using FeSimpleHelpers.Core;
using FeSimpleHelpers.UI;
using MyGame.General;

namespace MyGame.Ingame.UI
{
	public class UIIngameMenu : UIWindow
	{
		private void ChangedPauseState(bool obj)
		{
			if (obj)
				Show();
		}


		public void HowToPlay()
		{
			//options.SetOnBackCallback(Show).Show();
		}

		public void Options()
		{
			//options.SetOnBackCallback(Show).Show();
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