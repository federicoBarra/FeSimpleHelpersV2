using FeSimpleHelpers.General;
using FeSimpleHelpers.UIDebug;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MyGame.General
{
	[CreateAssetMenu(fileName = "MyGameCheatsConfig", menuName = "MyGame/Main Configs/MyGameCheatsConfig")]
	public class MyGameCheatsConfig : CheatsConfig
	{
		public static MyGameCheatsConfig GetInherit => (MyGameCheatsConfig)Get<MyGameCheatsConfig>();

		public override void Recreate()
		{
			base.Recreate();
			AddCheat(DumpCheatInfo, Key.F2, Key.None, "Open/Close this Window", "Will dump info in console if verbose enabled");
			AddCheat(ToggleDebugInfo, Key.F3, "Debug Info");
			AddCheat(WinLevel, Key.F5);
			AddCheat(ToggleConsole, Key.F4);

			AddCheat(ToggleTimeScaleDebug, Key.F8, Key.LeftShift, "Time Scale Sliders");

			AddCheat(ToggleFreeze, Key.P);
		}

		private void ToggleDebugInfo()
		{
			UIDebugInfo.Get()?.Toggle();
		}

		private void WinLevel()
		{
			IngameManager.Get()?.WinLevel();
		}
		private void ToggleConsole()
		{
			UIDebugConsole.Get()?.Toggle();
		}

		private void ToggleTimeScaleDebug()
		{
			UIDebugTimeScale ui = UIDebugTimeScale.Get();
			if (!ui)
				return;

			ui.Toggle();
		}
		private void ToggleFreeze()
		{
			Time.timeScale = Time.timeScale > 0 ? 0 : 1;
		}
	}
}