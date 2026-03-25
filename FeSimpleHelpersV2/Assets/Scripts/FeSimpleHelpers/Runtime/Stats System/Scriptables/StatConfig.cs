#if UNITY_EDITOR
using UnityEditor;
#endif
using FeSimpleHelpers.Core;
using UnityEngine;

namespace FeSimpleHelpers.StatsSystem
{
	[CreateAssetMenu(menuName = "FeSimpleFramework/Stats/Stat Config")]
	public class StatConfig : IDScriptableObject
	{
		public float maxValue = 9999;
		public float minValue = -9999;
		public bool silentStat = false;

		protected override void Validate()
		{
			base.Validate();
#if UNITY_EDITOR
			if (displayName.Contains("Stat - "))
			{
				displayName = displayName.Replace("Stat - ", "");

				EditorUtility.SetDirty(this);
				SerializedObject so = new SerializedObject(this);
				so.ApplyModifiedProperties();
			}
#endif
		}
	}
}