using System;
using System.IO;
using UnityEngine;

namespace FeSimpleHelpers.Build
{
	//This is supposed to work in tandem with Editor/DevBuildWindow

	[Serializable]
	public class BuildInfo
	{
		public string branchName;
		public string date;
		public string builderName;
		public string commitHash;
		public string unityVersion;
		public string buildTarget;

		public string GetNice()
		{
			string branchColor = "#FFFFFF";

			switch (branchName)
			{
				case "development":
					branchColor = "#FF4040"; // red
					break;

				case "internal":
					branchColor = "#FFD54A"; // yellow
					break;

				case "main":
					branchColor = "#FFFFFF"; // white
					break;
			}

			string shortCommit = string.IsNullOrEmpty(commitHash)
				? ""
				: commitHash.Substring(0, Mathf.Min(7, commitHash.Length));

			string ret =
				$"<color={branchColor}>{branchName}</color> " +
				$"<color=#9A9A9A>{shortCommit}</color> " +
				$"<color=#6F6F6F>{date}</color> " +
				$"<color=#D4AF37AA>({builderName})</color>";

			return ret;
		}
	}

	public static class BuildInfoHandler
	{
		private const string FileName = FileNameNOExtension + ".json";
		private const string FileNameNOExtension = "build_info";

		private static string FilePath =>
			Path.Combine(Application.dataPath + "\\Resources\\", FileName);

		public static void Save(string branchName, string commitHash, string builderName)
		{
			BuildInfo info = new BuildInfo
			{
				branchName = branchName,
				date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
				builderName = builderName,
				commitHash = commitHash,
			};

			string json = JsonUtility.ToJson(info, true);
			File.WriteAllText(FilePath, json);

#if UNITY_EDITOR
			UnityEditor.AssetDatabase.Refresh();
#endif
		}

		public static BuildInfo Load()
		{
			string json = Resources.Load<TextAsset>(FileNameNOExtension)?.ToString();

			return JsonUtility.FromJson<BuildInfo>(json);
		}
	}
}