using FeSimpleHelpers;
using MyGame.General;
using UnityEngine;

namespace MyGame.UI
{
	public class UIMainMenu : MonoBehaviour
	{
		public UIProfileHandling uiProfiles;

		public void Play()
		{
			if (UserProfiles.Get().ExistAnyProfile)
				GameManager.Get().GoToLobby();
			else
				uiProfiles.ShowProfileSelection();
		}

		public void Options()
		{
			Debug.Log("TODO OPTIONS");
		}

		public void Quit()
		{
			GameManager.Get().ExitGame();
		}
	}
}