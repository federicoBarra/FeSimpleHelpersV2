using MyGame.Input;
using UnityEngine;

namespace FeSimpleHelpers.UI
{
	/// <summary>
	/// TODO might find a better name suited for this class. It is responsible for handling the current focused window and the cancel input to close windows.
	/// </summary>
	public class UIInteractHandler : MonoBehaviour
	{
		private UIWindow currentFocusWindow;

		void Awake()
		{
			InputConfig.OnMenuCancel += CancelPressed;
			UIWindow.OnFocusIntent += WindowTryFocus;
		}

		private void CancelPressed()
		{
			if (!currentFocusWindow)
				return;

			UIWindow lastWindow = currentFocusWindow;

			currentFocusWindow.Back();

			if (lastWindow == currentFocusWindow)
				currentFocusWindow = null;
		}

		void OnDestroy()
		{
			InputConfig.OnMenuCancel -= CancelPressed;
			UIWindow.OnFocusIntent -= WindowTryFocus;
		}

		public void WindowTryFocus(UIWindow window)
		{

			currentFocusWindow = window;
		}
	}
}