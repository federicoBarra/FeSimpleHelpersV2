using System.Collections.Generic;
using FeSimpleHelpers.Core;
using UnityEngine;

namespace FeSimpleHelpers.ScenesHandling
{
	/// <summary>
	/// Unique Scriptable Object to handle all the scenes in the game, from levels to cutscenes, main menu, etc.
	/// </summary>
	[CreateAssetMenu(fileName = "ScenesConfig", menuName = "FeSimpleFramework/Main Configs/ScenesConfig")]
	public class ScenesConfig : ConfigSingleton<ScenesConfig>
	{
		public LevelConfig firstLevel;
		public List<LevelConfig> levels;
		public List<CutsceneConfig> cutscenes;
		public SceneConfig bootScene;
		public SceneConfig introScene;
		public SceneConfig mainMenuScene;
		public SceneConfig lobbyScene;

		public List<ISceneInfoProvider> GetAllScenes()
		{
			List<ISceneInfoProvider> scenes = new List<ISceneInfoProvider>();
			scenes.AddRange(levels);
			scenes.AddRange(cutscenes);
			scenes.Add(lobbyScene);
			scenes.Add(introScene);
			scenes.Add(mainMenuScene);
			scenes.Add(bootScene);
			return scenes;
		}
	}
}