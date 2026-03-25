using Tymski;
using UnityEngine;

namespace FeSimpleHelpers.ScenesHandling
{
	[CreateAssetMenu(fileName = "SceneConfig", menuName = "FeSimpleFramework/Configs/Scene Config")]
	public class SceneConfig : ScriptableObject, ISceneInfoProvider
	{
		public SceneReference scene;
		public SceneConfig nextSceneConfig;
		[TextArea(20, 50)]
		public string devInfo;
		public virtual ISceneInfoProvider NextSceneInfoProvider => nextSceneConfig;
		public virtual SceneReference CurrentScene => scene;
	}
}
