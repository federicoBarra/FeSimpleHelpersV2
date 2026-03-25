using FeSimpleHelpers.Core;
using UnityEngine;

namespace MyGame.UI
{
	[CreateAssetMenu(fileName = "UIConfig", menuName = "MyGame/Main Configs/UIConfig")]
	public class UIConfig : ConfigSingleton<UIConfig>
	{
		public Color normalWhite;
		public Color disabledColor;

		public Color someColor;
		public Sprite someImage;
	}
}