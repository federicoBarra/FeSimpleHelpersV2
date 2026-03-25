using FeSimpleHelpers.UI;
using MyGame.Input;
using UnityEngine;

namespace MyGame.UI
{
	[RequireComponent(typeof(UIWindowFocusHandler))]
	public class UIWindowInteractHandler : MonoBehaviour
	{
		private UIWindowFocusHandler focusHandler;
		void Awake()
		{
			focusHandler = GetComponent<UIWindowFocusHandler>();
			InputConfig.OnMenuCancel += BackCurrentFocusedUIWindow;
		}

		void OnDestroy()
		{
			InputConfig.OnMenuCancel -= BackCurrentFocusedUIWindow;
		}

		private void BackCurrentFocusedUIWindow()
		{
			focusHandler?.BackCurrentFocusedUIWindow();
		}
	}
}