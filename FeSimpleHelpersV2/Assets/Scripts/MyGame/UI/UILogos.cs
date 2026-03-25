using System.Collections;
using System.Collections.Generic;
using FeSimpleHelpers.Core;
using FeSimpleHelpers.ScenesHandling;
using MyGame.General;
using UnityEngine;

namespace MyGame.UI
{
	public class UILogos : MonoBehaviour
	{
		[SerializeField] private SceneConfig introConfig;

		[SerializeField] private List<CanvasGroup> thingsToShow;

		[SerializeField] private float logosDuration = 3;
		[SerializeField] private AnimationCurve alphaCurve;

		void Start()
		{
			foreach (CanvasGroup cg in thingsToShow)
			{
				cg.alpha = 0;
			}

			StartCoroutine(LaunchLogos());
		}

		IEnumerator LaunchLogos()
		{
			int logoIndex = 0;
			float t = 0;
			CanvasGroup cg = thingsToShow[logoIndex];
			cg.alpha = 0;

			while (t <= logosDuration)
			{
				cg.alpha = alphaCurve.Evaluate(t / logosDuration);

				t += Time.deltaTime;

				if (t >= logosDuration)
				{
					logoIndex++;
					if (logoIndex < thingsToShow.Count)
					{
						cg.alpha = 0;
						cg = thingsToShow[logoIndex];
						t = 0;
					}
				}

				yield return null;
			}

			yield return null;

			EndIntro();
		}

		void EndIntro()
		{
			GameManager.Get().GoToScene(introConfig.nextSceneConfig, 2f, LoaderManager.LoadingType.Simple);
		}
	}
}