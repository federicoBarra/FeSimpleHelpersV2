using MyGame.General;
using MyGame.Ingame;
using UnityEngine;

namespace MyGame.Input
{
	public class PlayerInputHandler : MonoBehaviour
	{
		private PlayerController playerController;
		private IngameManager ingameManager;

		void Start()
		{
			//these references could be handled more elegantly, but for the sake of simplicity and time, this is fine.
			playerController = FindAnyObjectByType<PlayerController>();
			ingameManager = FindAnyObjectByType<IngameManager>();

			InputConfig.OnTryInvokePause += TryPauseGame;

			InputConfig.OnMove += Move;
			InputConfig.OnTryMelee += TryMelee;
			InputConfig.OnTryInteract += TryInteract;
		}

		void OnDestroy()
		{
			InputConfig.OnTryInvokePause -= TryPauseGame;

			InputConfig.OnMove -= Move;
			InputConfig.OnTryMelee -= TryMelee;
			InputConfig.OnTryInteract -= TryInteract;
		}

		//GAME
		private void TryPauseGame()
		{
			ingameManager.TryPause();
		}

		//PLAYER
		private void Move(Vector2 move)
		{
			playerController?.SetMoveInput(move);
		}

		private void TryMelee()
		{
			playerController?.TryMelee();
		}

		private void TryInteract()
		{
			playerController?.TryInteract();
		}
	}
}