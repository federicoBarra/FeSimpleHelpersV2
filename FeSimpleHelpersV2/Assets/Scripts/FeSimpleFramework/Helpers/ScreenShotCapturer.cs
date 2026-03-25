using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenShotCapturer : MonoBehaviour
{
	private const string screenshotFolder = "Screenshots/";
	private const string iconImagesFolder = "Screenshots/Icons/";
	private const string brushesImagesFolder = "Assets/Editor/Tiles/";

	public Canvas gridCanvas;
	public GameObject iconGosContainer;
	public float waitTimeBetweenTakes = 0.1f;
	public bool working;

	public bool forceRes;
	public Vector2Int forcedRes;

	static string GetScreenshotFileName()
	{
		DateTime localDate = DateTime.Now;
		String cultureName = "es-AR";
		var culture = new CultureInfo(cultureName);
		string name = screenshotFolder + "Screenshot_" + localDate.ToString(culture).Replace("/", "_").Replace(":", "_").Replace(".", "_") + ".png";
		return name;
	}

	static string GetIconImageFileName(string _goName)
	{
		string name = iconImagesFolder + "Icon_" + _goName + ".png";
		return name;
	}

	static string GetBrushImageFileName(string _goName)
	{
		string name = brushesImagesFolder + _goName + ".png";
		return name;
	}

	//[Button("Take Icons Images")]
	public void TakeIconsImages()
	{
		if (!Application.isPlaying)
		{
			Debug.Log("Must run in play mode");
			return;
		}

		if (working)
		{
			Debug.Log("Ocuppied");
			return;
		}
		StartCoroutine(TakeScreens(GetIconImageFileName));
	}

	//[Button("Take Brushes Images")]
	public void TakeBrushesImages()
	{
		if (!Application.isPlaying)
		{
			Debug.Log("Must run in play mode");
			return;
		}

		if (working)
		{
			Debug.Log("Ocuppied");
			return;
		}
		StartCoroutine(TakeScreens(GetBrushImageFileName));
	}


	IEnumerator TakeScreens(Func<string, string> filenameFunc)
	{
		gridCanvas.enabled = false;

		foreach (Transform t in iconGosContainer.transform)
		{
			t.gameObject.SetActive(false);
		}

		Debug.Log("Started");
		yield return new WaitForSeconds(waitTimeBetweenTakes);
		working = true;
		int index = 0;

		GameObject lastGO = null;

		int resolutionWidth = forceRes ? forcedRes.x : Camera.main.pixelWidth;
		int resolutionHeight = forceRes ? forcedRes.y : Camera.main.pixelHeight;

		while (index < iconGosContainer.transform.childCount)
		{
			GameObject go = iconGosContainer.transform.GetChild(index).gameObject;

			if (lastGO != null) 
				lastGO.SetActive(false);

			go.SetActive(true);

			yield return new WaitForSeconds(waitTimeBetweenTakes);

			SaveScreenshotToFile(filenameFunc(go.name), resolutionWidth, resolutionHeight);

			yield return new WaitForSeconds(waitTimeBetweenTakes);

			lastGO = go;
			index++;
		}

		Debug.Log("Ended");
		gridCanvas.enabled = true;
		working = false;
		yield return null;
	}

	// Update is called once per frame
	void Update()
	{
		if (Keyboard.current.digit3Key.wasPressedThisFrame)
		{
			TakeGameViewScreenshot();
		}
	}

	private static void TakeGameViewScreenshot()
	{
		DateTime localDate = DateTime.Now;
		String cultureName = "es-AR";
		var culture = new CultureInfo(cultureName);
		string name = "Screenshots/Screenshot_" + localDate.ToString(culture).Replace("/", "_").Replace(":", "_").Replace(".", "_") + ".png";
		ScreenCapture.CaptureScreenshot(name);
		Debug.Log(name);
	}

	static Texture2D Screenshot(int width, int height)
	{
		int resWidth = width;
		int resHeight = height;

		Camera camera = Camera.main;
		RenderTexture rt = new RenderTexture(resWidth, resHeight, 32);
		camera.targetTexture = rt;
		Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.ARGB32, false);
		camera.Render();
		RenderTexture.active = rt;
		screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
		screenShot.Apply();
		camera.targetTexture = null;
		RenderTexture.active = null;
		DestroyImmediate(rt);
		return screenShot;
	}

	public static Texture2D SaveScreenshotToFile(string fileName, int width, int height)
	{
		Texture2D screenShot = Screenshot(width, height);
		byte[] bytes = screenShot.EncodeToPNG();
		System.IO.File.WriteAllBytes(fileName, bytes);
		return screenShot;
	}

#if UNITY_EDITOR
	[UnityEditor.MenuItem("Tools/Take GameView Screenshot")]
	public static void TakeCurrentGameViewScreenshot()
	{
		TakeGameViewScreenshot();
	}
#endif
}
