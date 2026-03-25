using MyGame.General;
using UnityEngine;

namespace MyGame.Ingame
{
	public class PlayerController : MonoBehaviour
	{
		[SerializeField]
		private float MoveSpeed = 3;
		[SerializeField]
		private float turnSmoothTime = 0.1f;
		[SerializeField]
		private float gravityValue = -9.81f;

		CharacterController controller;
		Vector2 moveInput;
		Vector3 playerVelocity;

		void Awake()
		{
			controller = GetComponent<CharacterController>();
		}

		void Update()
		{
			UpdateController();
		}

		void UpdateController()
		{
			if (controller.isGrounded && playerVelocity.y < 0)
				playerVelocity.y = 0f;

			Vector3 move = new Vector3(moveInput.x, 0, moveInput.y).normalized;

			controller.Move(move * Time.deltaTime * MoveSpeed);

			if (move != Vector3.zero)
			{
				Quaternion targetRotation = Quaternion.LookRotation(move);
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSmoothTime * Time.deltaTime);
			}

			playerVelocity.y += gravityValue * Time.deltaTime;
			controller.Move(playerVelocity * Time.deltaTime);
		}

		public void TryInteract()
		{
			Debug.Log("TryInteract");
		}
		public void TryMelee()
		{
			//don't do all this obviusly!
			Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Exclude,FindObjectsSortMode.None);
			foreach (Enemy enemy in enemies)
			{
				enemy.GetComponent<IHittable>().ReceiveDamage(DamageInfo.low10Damage);
			}
		}
		public void SetMoveInput(Vector2 _moveInput)
		{
			moveInput = _moveInput;
		}
	}
}