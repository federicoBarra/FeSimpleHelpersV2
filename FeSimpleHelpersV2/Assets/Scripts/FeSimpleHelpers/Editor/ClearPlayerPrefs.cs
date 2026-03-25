#if UNITY_EDITOR
using UnityEngine;

namespace FeSimpleHelpers.Tools
{
	public class ClearPlayerPrefs : MonoBehaviour
	{
		[UnityEditor.MenuItem("FeSimpleHelpers/Clear Player Prefs")]
		public static void PlayerPrefsDeleteAll()
		{
			PlayerPrefs.DeleteAll();
		}
	}
}
#endif