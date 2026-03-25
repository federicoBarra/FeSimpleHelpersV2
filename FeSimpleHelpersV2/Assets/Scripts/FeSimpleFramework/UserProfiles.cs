using System;
using System.Collections.Generic;
using FeSimpleHelpers.Core;
using UnityEngine;

namespace FeSimpleHelpers
{
	public interface IProfileSyncher
	{
		void SyncFromProfile(UserProfiles.UserProfile profile);
		void SyncToProfile(UserProfiles.UserProfile profile);
	}
	public class UserProfiles : MonoBehaviourSingleton<UserProfiles>
	{
		private const string profilesName = "profile";
		private const string selectedProfileName = "selectedProfile";
		private const string emptySlotName = "Empty Slot";

		[Serializable]
		public class UserProfile
		{
			//General
			public string name = "";
			public bool isBeingUsed = false;
			public float totalPlayTime = 0;

			public int lastLevelWon = 0;
			public int NextLevelIndex => lastLevelWon + 1;

			public void Reset()
			{
				//General
				name = emptySlotName;
				isBeingUsed = false;
				totalPlayTime = 0;
				lastLevelWon = 0;
			}

			public void StartNewGame()
			{
				Debug.Log("UserProfile - Start New Game");
				Reset();
			}

		}

		List<UserProfile> profiles;
		int currentProfileIndex;

		public UserProfile CurrentProfile => currentProfileIndex >= 0 ? profiles[currentProfileIndex] : null;
		public List<UserProfile> Profiles => profiles;
		public bool ExistAnyProfile => CurrentProfile != null;

		[Header("DEBUG")] public bool verbose;
		public int forceProfileIndex = -1;

		protected override void Awake()
		{
			base.Awake();

			ValidateSaves();

			currentProfileIndex = PlayerPrefs.GetInt(selectedProfileName);
			profiles = new List<UserProfile>();
			for (int i = 0; i < 3; i++)
			{
				UserProfile profile = JsonUtility.FromJson<UserProfile>(PlayerPrefs.GetString(profilesName + i));
				profiles.Add(profile);
			}

			//DEBUG
			if (forceProfileIndex >= 0)
				currentProfileIndex = forceProfileIndex;

			if (verbose)
				DEBUG_PrintProfiles();
		}

		void ValidateSaves()
		{
			if (PlayerPrefs.HasKey(selectedProfileName))
				return;

			Debug.LogWarning("Profiles not found. Creating Profiles... ");
			//create
			for (int i = 0; i < 3; i++)
			{
				UserProfile newup = new UserProfile();
				newup.Reset();
				SaveProfile(i, newup);
			}

			PlayerPrefs.SetInt(selectedProfileName, 0);

			Debug.LogWarning("Profiles CREATED.");
		}

		public void RemoveProfile(int index)
		{
			var profile = profiles[index];
			profile.Reset();
			SaveProfile(index, profile);

			if (currentProfileIndex != index)
				return;

			// find new profile (if any) to set as current.
			currentProfileIndex = -1;
			for (var i = 0; i < profiles.Count; i++)
			{
				profile = profiles[i];
				if (profile.isBeingUsed)
				{
					currentProfileIndex = i;
					break;
				}
			}

			PlayerPrefs.SetInt(selectedProfileName, currentProfileIndex);
		}

		public void AddProfile(string newProfileName)
		{
			for (var i = 0; i < profiles.Count; i++)
			{
				var profile = profiles[i];
				if (!profile.isBeingUsed)
				{
					profile.Reset();
					profile.isBeingUsed = true;
					profile.name = newProfileName;
					SaveProfile(i, profile);
					currentProfileIndex = i;
					PlayerPrefs.SetInt(selectedProfileName, currentProfileIndex);
					return;
				}
			}
		}

		public void SaveAll()
		{
			for (var i = 0; i < profiles.Count; i++)
			{
				var profile = profiles[i];
				SaveProfile(i, profile);
			}
		}

		void SaveProfile(int index, UserProfile profile)
		{
			PlayerPrefs.SetString(profilesName + index, JsonUtility.ToJson(profile));
		}

		public void DEBUG_PrintProfiles()
		{
			for (int i = 0; i < 3; i++)
			{
				string profileData = JsonUtility.ToJson(profiles[i]);
				Debug.Log("Profile " + i + ": " + profileData);
			}
		}
	}
}