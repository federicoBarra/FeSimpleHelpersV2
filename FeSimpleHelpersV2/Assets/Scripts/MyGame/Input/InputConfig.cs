using System;
using FeSimpleHelpers.Core;
using UnityEngine;
using UnityEngine.InputSystem;

//TODO remove unused stuff
namespace MyGame.Input
{
	[CreateAssetMenu(fileName = "InputConfig", menuName = "Mepep/InputConfig")]
	public class InputConfig : ConfigSingleton<InputConfig>, GeneratedInputActions.IIngameActions, GeneratedInputActions.IMenusActions
	{
		private GeneratedInputActions inputControls;

		public static event Action<Vector2> OnMove;
		public static event Action<bool> OnNitroPressed;

		public static event Action OnTryInvokePause;
		public static event Action OnTryMelee;
		public static event Action<bool> OnTryShoot;
		public static event Action OnTryInteract;
		public static event Action<int> OnTrySetWeapon;
		public static event Action OnTryReload;

		public static event Action OnTryToggleMap;

		public static event Action OnTryPendexGoTo;
		public static event Action OnTryPendexReturn;

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

		public void OnNitro(InputAction.CallbackContext context)
		{
			if (context.performed)
				OnNitroPressed?.Invoke(true);
			if (context.canceled)
				OnNitroPressed?.Invoke(false);
		}

		public void OnShoot(InputAction.CallbackContext context)
		{
			if (context.performed)
				OnTryShoot?.Invoke(true);
			if (context.canceled)
				OnTryShoot?.Invoke(false);
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

		public void OnPendexGoTo(InputAction.CallbackContext context)
		{
			if (context.performed)
				OnTryPendexGoTo?.Invoke();
		}

		public void OnPendexReturn(InputAction.CallbackContext context)
		{
			if (context.performed)
				OnTryPendexReturn?.Invoke();
		}

		public void OnToggleMap(InputAction.CallbackContext context)
		{
			if (context.performed)
				OnTryToggleMap?.Invoke();
		}

		public void OnSetWeapon01(InputAction.CallbackContext context)
		{
			if (context.performed)
				OnTrySetWeapon?.Invoke(0);
		}

		public void OnSetWeapon02(InputAction.CallbackContext context)
		{
			if (context.performed)
				OnTrySetWeapon?.Invoke(1);
		}

		public void OnSetWeapon03(InputAction.CallbackContext context)
		{
			if (context.performed)
				OnTrySetWeapon?.Invoke(2);
		}

		public void OnSetWeapon04(InputAction.CallbackContext context)
		{
			if (context.performed)
				OnTrySetWeapon?.Invoke(3);
		}

		public void OnReload(InputAction.CallbackContext context)
		{
			if (context.performed)
				OnTryReload?.Invoke();
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