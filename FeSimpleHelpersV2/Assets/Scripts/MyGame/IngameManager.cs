using System;
using FeSimpleHelpers;
using FeSimpleHelpers.Core;
using MyGame.Input;
using UnityEngine;

namespace MyGame.General
{
	public class IngameManager : MonoBehaviourSingleton<IngameManager>
	{
		enum GameState
		{
			None,
			LevelIntro,
			Ingame,
			IngameCutscene,
			Paused,
			LevelEnd,
		}
		private GameState currentState = GameState.None;

		public enum LevelEndReason
		{
			Win,
			PlayerDied,
		}
		private LevelEndReason levelEndReason;
		public bool LevelWon => levelEndReason == LevelEndReason.Win;

		[SerializeField]
		private float levelDuration = 0;
		public float LevelDuration => levelDuration;
		public bool IngameState => currentState == GameState.Ingame;

		public static event Action<bool> OnGamePaused;
		public static event Action<LevelEndReason> OnLevelEnded;

		public override void AwakeSingleton()
		{
			base.AwakeSingleton();
			//TODO Player.OnDie += PlayerDied;
		}

		public override void OnDestroySingleton()
		{
			base.OnDestroySingleton();
			//TODO Player.OnDie -= PlayerDied;
		}

		// Start is called once before the first execution of Update after the MonoBehaviour is created
		void Start()
		{
			levelDuration = 0;
			PreloadLevel();
			LevelIntro();
			StartLevel(); //TODO delay this to be called after intro
		}

		// Update is called once per frame
		void Update()
		{
			levelDuration += Time.unscaledDeltaTime;

			if (currentState == GameState.Ingame)
			{
				//Current Profile should always exist at this point.
				UserProfiles.Get().CurrentProfile.totalPlayTime += Time.deltaTime;
			}
		}

		void SetState(GameState newState)
		{
			switch (newState)
			{
				case GameState.None:
					Time.timeScale = 0;
					InputConfig.Get().DisableAllInput();
					break;
				case GameState.LevelIntro:
					InputConfig.Get().DisableAllInput();
					break;
				case GameState.Ingame:
					Time.timeScale = 1;
					InputConfig.Get().EnableIngame();
					break;
				case GameState.IngameCutscene:
					Time.timeScale = 0;
					InputConfig.Get().DisableAllInput();
					break;
				case GameState.Paused:
					Time.timeScale = 0;
					InputConfig.Get().EnableMenu();
					break;
				case GameState.LevelEnd:
					InputConfig.Get().EnableMenu();
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
			}

			currentState = newState;
		}

		public void PreloadLevel()
		{
			SetState(GameState.None);
		}

		public void LevelIntro()
		{
			SetState(GameState.LevelIntro);
		}

		public void StartLevel()
		{
			SetState(GameState.Ingame); //TODO there should be an intro state first
			UserProfiles.UserProfile currentUserProfile = UserProfiles.Get().CurrentProfile;
			//TODO Player.SyncFromProfile(currentUserProfile);
		}

		public void TryPause()
		{
			if (currentState == GameState.Paused)
				return;

			SetState(GameState.Paused);
			OnGamePaused?.Invoke(true);
		}

		public void TryContinueFromPause()
		{
			if (currentState != GameState.Paused)
				return;

			SetState(GameState.Ingame);
			OnGamePaused?.Invoke(false);
		}

		public void PlayerDied(IHittable h)
		{
			LevelEnd(LevelEndReason.PlayerDied);
		}

		public void WinLevel()
		{
			LevelEnd(LevelEndReason.Win);
		}

		void LevelEnd(LevelEndReason reason)
		{
			levelEndReason = reason;
			SetState(GameState.LevelEnd);
			OnLevelEnded?.Invoke(reason);

			UserProfiles.UserProfile currentUserProfile = UserProfiles.Get().CurrentProfile;
			// do something with currentUserProfile
			GameManager.Get().SaveToCurrentProfile();
		}
	}
}