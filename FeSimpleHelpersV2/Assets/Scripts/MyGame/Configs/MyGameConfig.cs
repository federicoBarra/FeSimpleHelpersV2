using FeSimpleHelpers.Core;
using UnityEngine;

namespace MyGame.General
{
	[CreateAssetMenu(fileName = "MyGameConfig", menuName = "MyGame/Main Configs/MyGameConfig")]
	public class MyGameConfig : ConfigSingleton<MyGameConfig>
	{
		public float someValue = 1;
	}
}