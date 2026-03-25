using FeSimpleHelpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame.UI
{
	public class UIUserProfile : MonoBehaviour
	{
		public Image profileImage;
		public TMP_Text nameText;
		public TMP_Text durationText;
		public Button addProfileButton;
		public Button removeProfileButton;

		public void Set(UserProfiles.UserProfile userProfile)
		{
			if (userProfile.isBeingUsed)
			{
				nameText.text = userProfile.name;
				durationText.text = userProfile.totalPlayTime.ToString("N2");
			}
			else
			{
				nameText.text = "Empty Slot";
				durationText.text = "";
			}

			removeProfileButton.gameObject.SetActive(userProfile.isBeingUsed);
			addProfileButton.gameObject.SetActive(!userProfile.isBeingUsed);
		}
	}
}