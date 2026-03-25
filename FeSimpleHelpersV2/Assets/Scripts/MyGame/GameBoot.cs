using FeSimpleHelpers.Core;
using FeSimpleHelpers.ScenesHandling;
using UnityEngine;

namespace MyGame.General
{
	public class GameBoot : MonoBehaviour
	{
		[SerializeField] private SceneConfig bootConfig;

		void Start()
		{
			GameManager.Get().GoToScene(bootConfig.nextSceneConfig, 0.1f, LoaderManager.LoadingType.NoInterface);
		}
	}
}