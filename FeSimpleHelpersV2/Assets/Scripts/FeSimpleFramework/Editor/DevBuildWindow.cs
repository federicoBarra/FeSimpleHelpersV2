using UnityEditor;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using Debug = UnityEngine.Debug;
using UnityEditor.Build.Reporting;
using System.Linq;

namespace FeSimpleHelpers.Build
{
	/// <summary>
	/// Build window class.
	/// Create Builds based on current branch, and upload to steam if needed.
	/// This works particularly with my build pipeline and the way I handle branches and steam.
	/// </summary>
	public class DevBuildWindow : EditorWindow
	{
		string GameName => "MyGame";
		string BuilderName => "Fede";
		string BuildFolder => buildFolder;

		string SteamAppID => steamAppID;
		string SteamUsername => steamUsername;
		string SteamCmdPath => steamCmdPath;
		string SteamVdfPathFolder => steamVdfPathFolder;

		string buildFolder = "D:\\Builds\\MyGame";
		string steamAppID = "00000000";
		string steamUsername = "00000000";
		string steamCmdPath = @"D:\steamworks_sdk_163\sdk\tools\ContentBuilder\builder\steamcmd.exe";
		string steamVdfPathFolder = @"D:\Builds\MyGame\steam\vdfs"; //\app_00000000_development.vdf";

		string branchName = "...";
		string commitHash = "...";
		string status = Status_WaitingForAction;

		private bool cleanBuild = false;
		private bool uploadToSteam = false;
		private bool onlyScriptsBuild = false;

		private const string Status_WaitingForAction = "<color=white>Waiting For Action</color>";
		private const string Status_WaitingForBuild = "<color=yellow>Waiting For Build To Complete...</color>";

		private const string Status_BuildCompleteUploadingToSteam =
			"<color=yellow>Build Complete, uploading to steam...</color>";

		private const string Status_BuildComplete = "<color=green>Build Complete! END</color>";
		private const string Status_SteamUploadComplete = "<color=green>Hooray!!!, Build now on Steam END</color>";

		private const string Error_WrongBranch = "<color=red>Error: Wrong branch, switch to intended</color>";
		private const string Error_BuildFailed = "<color=red>Error: Build failed!</color>";
		private const string Error_BuildFolderNotFound = "<color=red>Error: Build folder not found!</color>";
		private const string Error_SteamUploadFailed = "<color=red>Error: Steam Upload Failed!</color>";

		enum BuildTargetSteamBranch
		{
			development,
			@internal,
			main
		}

		[MenuItem("FeSimpleHelpers/Build Window")]
		public static void Open()
		{
			GetWindow<DevBuildWindow>("Dev Build");
		}

		void OnEnable()
		{
			UpdateBranch();
		}

		void OnGUI()
		{
			GUIStyle richStyle = new GUIStyle(EditorStyles.label);
			richStyle.richText = true;

			GUILayout.Space(10);

			EditorGUILayout.LabelField("Current branch", branchName);
			EditorGUILayout.LabelField("commit hash", commitHash);

			GUILayout.Space(10);
			EditorGUILayout.LabelField("Build Config");

			GUILayout.Space(5);

			using (new EditorGUI.DisabledScope(true))
			{
				EditorGUILayout.TextField("Build location", BuildFolder);
				EditorGUILayout.TextField("Steam App ID", SteamAppID);
				EditorGUILayout.TextField("Steam User", SteamUsername);
				EditorGUILayout.TextField("Steam Cmd Path", SteamCmdPath);
				EditorGUILayout.TextField("Steam Vdf Path Folder", SteamVdfPathFolder);
			}

			cleanBuild = EditorGUILayout.Toggle("Clean Build", cleanBuild);
			uploadToSteam = EditorGUILayout.Toggle("Upload To Steam", uploadToSteam);
			onlyScriptsBuild = EditorGUILayout.Toggle("DEBUG Only scripts build", onlyScriptsBuild);

			GUILayout.Space(10);

			if (GUILayout.Button("Build Dev"))
			{
				BuildGame(BuildTargetSteamBranch.development);
			}

			if (GUILayout.Button("Build Internal"))
			{
				BuildGame(BuildTargetSteamBranch.@internal);
			}

			if (GUILayout.Button("Build Main"))
			{
				BuildGame(BuildTargetSteamBranch.main);
			}

			if (GUILayout.Button("Test Create build Info Json"))
			{
				CreateBuildInfoJson();
			}

			if (GUILayout.Button("Test Upload to Steam"))
			{
				UploadBuildToSteam(null);
			}


			GUILayout.Space(10);

			EditorGUILayout.LabelField("Status", status, richStyle);
		}

		void BuildGame(BuildTargetSteamBranch targetBranch)
		{
			Debug.Log("Start Build process");

			UpdateBranch();

			if (branchName != targetBranch.ToString())
			{
				ExitWithError(Error_WrongBranch);
				return;
			}

			string foldersContainerPath = BuildFolder;
			if (!Directory.Exists(foldersContainerPath))
			{
				ExitWithError(Error_BuildFolderNotFound);
				return;
			}

			SetStatus(Status_WaitingForBuild);

			string nameWithtargetBranch =
				GameName + (targetBranch == BuildTargetSteamBranch.main ? "" : "_" + targetBranch);

			string folderPath = BuildFolder + "/" + nameWithtargetBranch;

			if (!Directory.Exists(folderPath))
			{
				Directory.CreateDirectory(folderPath);
			}

			string buildLocation = folderPath + "/" + nameWithtargetBranch + ".exe";

			Debug.Log("folderPath: " + folderPath);
			Debug.Log("buildLocation: " + buildLocation);

			var buildPlayerOptions = new BuildPlayerOptions
			{
				scenes = EditorBuildSettings.scenes
					.Where(s => s.enabled)
					.Select(s => s.path)
					.ToArray(),

				locationPathName = buildLocation,
				target = BuildTarget.StandaloneWindows64,
				options = cleanBuild ? BuildOptions.CleanBuildCache : BuildOptions.None,
			};

			if (onlyScriptsBuild)
			{
				buildPlayerOptions.options |= BuildOptions.BuildScriptsOnly;
			}

			Debug.Log("Building...");

			BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);

			OnBuildFinished(report);
		}

		void OnBuildFinished(BuildReport report)
		{
			if (report.summary.result == BuildResult.Succeeded)
			{
				Debug.Log($"Build size: {report.summary.totalSize / (1024 * 1024)} MB");
				if (uploadToSteam)
					UploadBuildToSteam(report);
				else
					SetStatus(Status_BuildComplete);
			}
			else if (report.summary.result == BuildResult.Failed)
			{
				ExitWithError(Error_BuildFailed);
			}
		}

		void UploadBuildToSteam(BuildReport report)
		{
			SetStatus(Status_BuildCompleteUploadingToSteam);

			try
			{
				var process = new Process();

				string vdfPath = SteamVdfPathFolder + "\\app_" + SteamAppID + "_" + branchName + ".vdf";

				Debug.Log("VDF Path: " + vdfPath);

				process.StartInfo.FileName = SteamCmdPath;
				process.StartInfo.Arguments =
					$"+login {SteamUsername} " +
					$"+run_app_build \"{vdfPath}\" " +
					$"+quit";

				process.StartInfo.UseShellExecute = false;
				process.StartInfo.RedirectStandardOutput = true;
				process.StartInfo.RedirectStandardError = true;
				process.StartInfo.CreateNoWindow = true;

				process.Start();

				while (!process.StandardOutput.EndOfStream)
				{
					Debug.Log(process.StandardOutput.ReadLine());
				}

				while (!process.StandardError.EndOfStream)
				{
					Debug.LogError(process.StandardError.ReadLine());
				}

				process.WaitForExit();

				if (process.ExitCode != 0)
				{
					ExitWithError(Error_SteamUploadFailed);
					return;
				}

				SetStatus(Status_SteamUploadComplete);
			}
			catch (System.Exception e)
			{
				Debug.LogException(e);
				ExitWithError(Error_SteamUploadFailed);
			}
		}

		void ExitWithError(string error)
		{
			Debug.LogError(error);
			status = error;
			Repaint();
		}

		void SetStatus(string _status)
		{
			Debug.Log(_status);
			status = _status;
			Repaint();
		}

		//HELPERS

		void CreateBuildInfoJson()
		{
			UpdateBranch();
			BuildInfoHandler.Save(branchName, commitHash, BuilderName);
		}

		void UpdateBranch()
		{
			//git show -s--format = "%H"
			commitHash = RunGit("show -s --format=\"%H\"");
			branchName = RunGit("branch --show-current");
			Repaint();
		}

		static string RunGit(string args)
		{
			try
			{
				var process = new Process();

				process.StartInfo.FileName = "git";
				process.StartInfo.Arguments = args;
				process.StartInfo.RedirectStandardOutput = true;
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.CreateNoWindow = true;

				process.Start();

				string result = process.StandardOutput.ReadToEnd();
				process.WaitForExit();

				return result.Trim();
			}
			catch
			{
				return "git?";
			}
		}
	}
}