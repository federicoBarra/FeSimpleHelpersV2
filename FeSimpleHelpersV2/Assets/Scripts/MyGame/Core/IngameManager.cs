using System;
using FeSimpleHelpers.Core;
using MyGame.Input;
using MyGame.Persistence;
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

		private float levelDuration = 0;

		public bool LevelWon => levelEndReason == LevelEndReason.Win;
		public float LevelDuration => levelDuration;
		public bool IngameState => currentState == GameState.Ingame;

		public static event Action<bool> OnGamePaused;
		public static event Action<LevelEndReason> OnLevelEnded;

		public override void AwakeSingleton()
		{
			base.AwakeSingleton();
			//Player.OnDie += PlayerDied;
		}

		public override void OnDestroySingleton()
		{
			base.OnDestroySingleton();
			//Player.OnDie -= PlayerDied;
		}

		void Start()
		{
			levelDuration = 0;
			PreloadLevel();
			LevelIntro();
			//Here for simplification, one would want to process the level intro and then Start the level.
			StartLevel(); 
		}

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
			SetState(GameState.Ingame);
			//this could be done here or in LevelIntro state.
			UserProfiles.UserProfile currentUserProfile = UserProfiles.Get().CurrentProfile;
			//Player.SyncFromProfile(currentUserProfile);
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

			Debug.Log("Somebody do something!");
		}
	}
}