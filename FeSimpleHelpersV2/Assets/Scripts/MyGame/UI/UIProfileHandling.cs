using FeSimpleHelpers.UI;
using MyGame.General;
using MyGame.Persistence;
using TMPro;
using UnityEngine;

namespace MyGame.UI
{
	public class UIProfileHandling : UIWindow
	{
		[SerializeField]
		private GameObject currentProfileGO;
		[SerializeField]
		private GameObject selectProfileGO;
		[SerializeField]
		private GameObject createNewProfileDialogGO;
		[SerializeField]
		private TMP_InputField inputField;
		[SerializeField]
		private TMP_Text curentProfileName;
		[SerializeField]
		private TMP_Text versionText;

		[SerializeField]
		private UIUserProfile[] userProfiles;

		protected override void Start()
		{
			base.Start();
			Refresh();
		}

		protected override void Refresh()
		{
			base.Refresh();
			if (UserProfiles.Get().ExistAnyProfile)
			{
				currentProfileGO.SetActive(true);
				curentProfileName.text = UserProfiles.Get().CurrentProfile.name;
			}
			else
				currentProfileGO.SetActive(false);

			versionText.text = GameManager.Get().buildInfo;
		}

		public void ShowProfileSelection()
		{
			FillProfilesUI();
			selectProfileGO.SetActive(true);
		}

		void FillProfilesUI()
		{
			for (var i = 0; i < userProfiles.Length; i++)
			{
				var profile = userProfiles[i];
				profile.Set(UserProfiles.Get().Profiles[i]);
			}
		}

		public void ShowCreateNewProfileDialog()
		{
			createNewProfileDialogGO.SetActive(true);
		}

		public void EraseProfile(int index)
		{
			UserProfiles.Get().RemoveProfile(index);
			selectProfileGO.SetActive(false);
			Refresh();
		}

		public void CreateProfile()
		{
			UserProfiles.Get().AddProfile(inputField.text);
			createNewProfileDialogGO.SetActive(false);
			selectProfileGO.SetActive(false);
			Refresh();
		}

		public void SelectProfile(int i)
		{
			UserProfiles.Get().SetSelectedProfile(i);
			selectProfileGO.SetActive(false);
			Refresh();
		}

	}
}