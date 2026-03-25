using MyGame.Persistence;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame.UI
{
	public class UIUserProfile : MonoBehaviour
	{
		[SerializeField]
		private Image profileImage;
		[SerializeField]
		private TMP_Text nameText;
		[SerializeField]
		private TMP_Text durationText;
		[SerializeField]
		private Button addProfileButton;
		[SerializeField]
		private Button removeProfileButton;
		[SerializeField]
		private Button selectProfileButton;

		public void Set(UserProfiles.UserProfile userProfile)
		{
			if (userProfile.isBeingUsed)
			{
				nameText.text = userProfile.name;
				durationText.text = userProfile.totalPlayTime.ToString("N2");
				selectProfileButton.gameObject.SetActive(true);
			}
			else
			{
				nameText.text = "Empty Slot";
				durationText.text = "";
				selectProfileButton.gameObject.SetActive(false);
			}

			removeProfileButton.gameObject.SetActive(userProfile.isBeingUsed);
			addProfileButton.gameObject.SetActive(!userProfile.isBeingUsed);
		}
	}
}