using UnityEngine;

namespace FeSimpleHelpers.ScenesHandling
{
	[CreateAssetMenu(fileName = "CutsceneConfig", menuName = "FeSimpleFramework/Configs/Cutscene Config")]
	public class CutsceneConfig : SceneConfig
	{
		public string cutsceneTitle;
		[TextArea]
		public string description;
	}
}