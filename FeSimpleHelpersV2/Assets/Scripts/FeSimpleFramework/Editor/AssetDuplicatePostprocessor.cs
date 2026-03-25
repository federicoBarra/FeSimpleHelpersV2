#if UNITY_EDITOR
using FeSimpleHelpers.Core;
using UnityEditor;
using UnityEngine;

public class AssetDuplicatePostprocessor : AssetPostprocessor
{
	static void OnPostprocessAllAssets(
		string[] importedAssets,
		string[] deletedAssets,
		string[] movedAssets,
		string[] movedFromAssetPaths)
	{
		foreach (var path in importedAssets)
		{
			// Ignore non-.asset files if you want
			if (!path.EndsWith(".asset"))
				continue;

			// Load the duplicated asset
			var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
			if (asset == null)
				continue;

			HandleDuplicatedAsset(asset, path);
		}
	}

	private static void HandleDuplicatedAsset(ScriptableObject asset, string path)
	{
		if (asset is IDScriptableObject)
		{
			IDScriptableObject idAsset = (asset as IDScriptableObject);
			idAsset.ValidateDuplicatedInEditor();
		}
	}
}
#endif
