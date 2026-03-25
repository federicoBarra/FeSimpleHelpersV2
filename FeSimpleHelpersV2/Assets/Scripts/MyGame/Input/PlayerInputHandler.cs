using MyGame.General;
using UnityEngine;

namespace MyGame.Input
{
	public class PlayerInputHandler : MonoBehaviour
	{
		//TODO Player private Dude dude;
		private IngameManager ingameManager;

		// Start is called before the first frame update
		void Start()
		{
			//TODO Player dude = FindAnyObjectByType<Dude>();
			ingameManager = FindAnyObjectByType<IngameManager>();

			InputConfig.OnTryInvokePause += PauseGame;

			InputConfig.OnMove += Move;

			InputConfig.OnTryMelee += TryMelee;
			InputConfig.OnTryInteract += TryInteract;
		}

		void OnDestroy()
		{
			InputConfig.OnTryInvokePause -= PauseGame;

			InputConfig.OnMove -= Move;

			InputConfig.OnTryMelee -= TryMelee;
			InputConfig.OnTryInteract -= TryInteract;
		}

		//GAME
		private void PauseGame()
		{
			ingameManager.TryPause();
		}

		//CART
		private void Move(Vector2 move)
		{
			//TODO Player pepito?.SetMoveInput(move);
		}

		private void TryMelee()
		{
			//TODO Player dude?.combat.TryMelee();
		}

		//PEPITO
		private void TryInteract()
		{
			//TODO Player pepito?.TryInteract();
		}
	}
}