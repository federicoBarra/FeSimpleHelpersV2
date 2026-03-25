using System;
using FeSimpleHelpers.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MyGame.Input
{
	[CreateAssetMenu(fileName = "InputConfig", menuName = "MyGame/Main Configs/InputConfig")]
	public class InputConfig : ConfigSingleton<InputConfig>, GeneratedInputActions.IIngameActions, GeneratedInputActions.IMenusActions
	{
		private GeneratedInputActions inputControls;

		public static event Action<Vector2> OnMove;

		public static event Action OnTryInvokePause;
		public static event Action OnTryMelee;
		public static event Action OnTryInteract;

		//UI
		public static event Action OnMenuCancel;
		public static event Action OnMenuSubmit;

		private void OnEnable()
		{
			if (inputControls != null)
				return;

			inputControls = new GeneratedInputActions();

			inputControls.Ingame.SetCallbacks(this);
			inputControls.Menus.SetCallbacks(this);
		}

		private void OnDisable()
		{
			inputControls.Ingame.Disable();
			inputControls.Menus.Disable();
		}

		public void EnableIngame()
		{
			inputControls.Ingame.Enable();
			inputControls.Menus.Disable();
		}

		public void EnableMenu()
		{
			inputControls.Ingame.Disable();
			inputControls.Menus.Enable();
		}

		public void DisableAllInput()
		{
			inputControls.Ingame.Disable();
			inputControls.Menus.Disable();
		}

		public void OnMovement(InputAction.CallbackContext context)
		{
			OnMove?.Invoke(context.ReadValue<Vector2>());
		}

		public void OnMelee(InputAction.CallbackContext context)
		{
			if (context.performed)
				OnTryMelee?.Invoke();
		}

		public void OnInteract(InputAction.CallbackContext context)
		{
			if (context.performed)
				OnTryInteract?.Invoke();
		}

		public void OnEsq(InputAction.CallbackContext context)
		{
			//Debug.Log("Onesq");

			if (context.performed)
			{
				//Debug.Log("Invoke Esq");
				OnTryInvokePause?.Invoke();
			}
		}

		// ////////// UI /////////////////////////

		public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
		{
			//throw new System.NotImplementedException();
		}

		public void OnTrackedDevicePosition(InputAction.CallbackContext context)
		{
			//throw new System.NotImplementedException();
		}

		public void OnRightClick(InputAction.CallbackContext context)
		{
			//throw new System.NotImplementedException();
		}

		public void OnMiddleClick(InputAction.CallbackContext context)
		{
			//throw new System.NotImplementedException();
		}

		public void OnScrollWheel(InputAction.CallbackContext context)
		{
			//throw new System.NotImplementedException();
		}

		public void OnClick(InputAction.CallbackContext context)
		{
			//throw new System.NotImplementedException();
		}

		public void OnPoint(InputAction.CallbackContext context)
		{
			//throw new System.NotImplementedException();
		}

		public void OnSubmit(InputAction.CallbackContext context)
		{
			if (context.performed)
				OnMenuSubmit?.Invoke();
		}

		public void OnCancel(InputAction.CallbackContext context)
		{
			if (context.performed)
				OnMenuCancel?.Invoke();
		}

		public void OnNavigate(InputAction.CallbackContext context)
		{
			//throw new System.NotImplementedException();
		}
	}
}