using UnityEngine;

namespace FeSimpleHelpers.ScenesHandling
{
	[CreateAssetMenu(fileName = "LevelConfig", menuName = "FeSimpleFramework/Configs/Level Config")]
	public class LevelConfig : SceneConfig
	{
		public string levelTitle;
		[TextArea] 
		public string description;

		public Sprite mapIcon;
		public bool available = true;
	}
}