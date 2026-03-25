using System.Collections.Generic;
using FeSimpleHelpers.Build;
using FeSimpleHelpers.Core;
using FeSimpleHelpers.ScenesHandling;
using MyGame.Persistence;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace MyGame.General
{
	/// <summary>
	/// This object handles the overall game app flow and state
	/// </summary>
	public class GameManager : MonoBehaviourSingleton<GameManager>
	{
		private ISceneInfoProvider previousSceneInfoProvider;
		private ISceneInfoProvider currentSceneInfoProvider;

		public bool InLobby => currentSceneInfoProvider == ScenesConfig.Get()?.lobbyScene as ISceneInfoProvider;
		public bool InMainMenu => currentSceneInfoProvider == ScenesConfig.Get()?.mainMenuScene as ISceneInfoProvider;
		public bool InLevel => currentSceneInfoProvider as LevelConfig;
		public bool CommingFromBoot => previousSceneInfoProvider == ScenesConfig.Get()?.bootScene as ISceneInfoProvider;
		public bool CommingFromIntro => previousSceneInfoProvider == ScenesConfig.Get()?.introScene as ISceneInfoProvider;

		[SerializeField]
		protected int gameSeed;
		public int GameSeed => gameSeed;

		[HideInInspector]
		public string buildInfo;

		public override void AwakeSingleton()
		{
			base.AwakeSingleton();
			MyGameCheatsConfig.GetInherit.StartPlayState();

			if (GameSeed >= 0)
				Random.InitState(GameSeed);

			//TODO take this to settings manager or something like that. There should be a persistance for settings separeted from game data
			QualitySettings.vSyncCount = 0;
			Application.targetFrameRate = 60;
			BuildInfo bInfo = BuildInfoHandler.Load();
			buildInfo = bInfo != null ? bInfo.GetNice() : "";
		}

		void Start()
		{
			LoadFromCurrentProfile();
			ValidateCurrentSceneConfigConfig();
		}

		protected void OnDisable()
		{
			MyGameCheatsConfig.GetInherit.EndPlayState();
		}

		void Update()
		{
			MyGameCheatsConfig.GetInherit.Process();
		}

		public void StartNewGame()
		{
			UserProfiles.Get().CurrentProfile.StartNewGame();
			LoadLevel(ScenesConfig.Get().firstLevel);
		}

		public void ContinueGame()
		{
			LoadLevel(UserProfiles.Get().CurrentProfile.NextLevelIndex);
		}

		public void LoadLevel(int i)
		{
			LoadLevel(ScenesConfig.Get().levels[i]);
		}
		public void LoadLevel(ISceneInfoProvider sceneInfoProvider)
		{
			GoToScene(sceneInfoProvider);
		}
		public void GoToLobby()
		{
			GoToScene(ScenesConfig.Get().lobbyScene);
		}
		public void GoToMainMenu(float fakeTime = 2, LoaderManager.LoadingType loadType = LoaderManager.LoadingType.Simple)
		{
			GoToScene(ScenesConfig.Get().mainMenuScene, fakeTime, loadType);
		}
		public void GoToScene(ISceneInfoProvider sceneInfoProvider, float fakeTime = 2, LoaderManager.LoadingType loadType = LoaderManager.LoadingType.Simple)
		{
			//Debug.Log("GOTO SCENE: " + sceneInfoProvider.CurrentScene);
			SetCurrentScene(sceneInfoProvider);
			LoaderManager.Get().LoadScene(currentSceneInfoProvider.CurrentScene, fakeTime, loadType);
		}

		void SetCurrentScene(ISceneInfoProvider sceneInfoProvider)
		{
			ValidateCurrentSceneConfigConfig();

			previousSceneInfoProvider = currentSceneInfoProvider;
			currentSceneInfoProvider = sceneInfoProvider;
		}

		public void ExitGame()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
		}

		void ValidateCurrentSceneConfigConfig()
		{
			if (currentSceneInfoProvider != null)
				return;

			string curretLevelName = SceneManager.GetActiveScene().path;
			List<ISceneInfoProvider> scenes = ScenesConfig.Get().GetAllScenes();

			for (var i = 0; i < scenes.Count; i++)
			{
				var config = scenes[i];
				if (config != null && config.CurrentScene == curretLevelName)
				{
					currentSceneInfoProvider = config;
					break;
				}
			}
		}

		public virtual void LoadFromCurrentProfile()
		{
			UserProfiles.UserProfile currentUserProfile = UserProfiles.Get().CurrentProfile;
			//Call SyncFromProfile on needed objects that extend from IProfileSyncher
		}

		public virtual void SaveToCurrentProfile()
		{
			UserProfiles.UserProfile currentUserProfile = UserProfiles.Get().CurrentProfile;
			//Call SyncToProfile on needed objects that extend from IProfileSyncher
			UserProfiles.Get().SaveAll();
		}
	}
}