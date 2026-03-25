using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FeSimpleHelpers.Core
{
	public class LoaderManager : MonoBehaviourSingleton<LoaderManager>
	{
		public enum LoadingType
		{
			NoInterface,
			Simple,
			BigLogo,
			Multi
		}

		public LoadingType loadingType;
		public float loadingProgress;
		[SerializeField]
		private float timeLoading;
		[SerializeField]
		private float fakeLoadTime = 2;

		public static event Action<LoaderManager> OnLoadingStart;
		public static event Action<LoaderManager> OnLoadingEnd;

		public void LoadScene(string sceneName, float fakeTime = -1, LoadingType loadType = LoadingType.BigLogo)
		{
			//Debug.Log("Load Scene: " + sceneName + " => " + loadType);
			loadingType = loadType;

			fakeTime = fakeTime < 0.01f ? fakeLoadTime : fakeTime;

			switch (loadingType)
			{
				case LoadingType.NoInterface:
					StartCoroutine(AsynchronousLoad(sceneName));
					break;
				case LoadingType.Simple:
					StartCoroutine(AsynchronousLoadWithFake(sceneName, fakeTime));
					break;
				case LoadingType.BigLogo:
					StartCoroutine(AsynchronousLoadWithFake(sceneName, fakeTime));
					break;
				case LoadingType.Multi:
					break;
			}
		}

		IEnumerator AsynchronousLoad(string scene)
		{
			OnLoadingStart?.Invoke(this);

			loadingProgress = 0;

			yield return null;

			AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
			ao.allowSceneActivation = false;

			while (!ao.isDone)
			{
				loadingProgress = ao.progress + 0.1f;

				// Loading completed
				if (ao.progress >= 0.9f)
				{
					ao.allowSceneActivation = true;
				}

				yield return null;
			}

			OnLoadingEnd?.Invoke(this);
		}

		IEnumerator AsynchronousLoadWithFake(string scene, float fakeTime)
		{
			OnLoadingStart?.Invoke(this);

			//Debug.Log("AsynchronousLoadWithFake");

			loadingProgress = 0;
			timeLoading = 0;
			yield return null;

			AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
			ao.allowSceneActivation = false;

			while (!ao.isDone)
			{
				timeLoading += Time.unscaledDeltaTime;
				//Debug.Log("Time.deltaTime: " + Time.unscaledDeltaTime);
				loadingProgress = ao.progress + 0.1f;
				loadingProgress = loadingProgress * timeLoading / fakeTime;
				//Debug.Log("loadingProgress: " + loadingProgress + " => " + timeLoading + " => " + fakeTime);

				// Loading completed
				if (loadingProgress >= 1)
				{
					ao.allowSceneActivation = true;
				}

				yield return null;
			}

			OnLoadingEnd?.Invoke(this);
		}
	}
}