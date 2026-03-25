using System.Collections.Generic;
using UnityEngine;

namespace FeSimpleHelpers.UI
{
	public class UIWindowFocusHandler : MonoBehaviour
	{
		private Stack<UIWindow> focusedWindowQueue = new Stack<UIWindow>();

		void Awake()
		{
			UIWindow.OnFocusIntent += WindowTryFocus;
		}
		void OnDestroy()
		{
			UIWindow.OnFocusIntent -= WindowTryFocus;
		}

		public void BackCurrentFocusedUIWindow()
		{
			if (focusedWindowQueue == null || focusedWindowQueue.Count <= 0)
				return;

			UIWindow lastWindow = focusedWindowQueue.Pop();

			lastWindow.Back();
		}
		public void WindowTryFocus(UIWindow window)
		{
			if (focusedWindowQueue.Count > 0 && focusedWindowQueue.Peek() == window)
				return;

			focusedWindowQueue.Push(window);
		}
	}
}
