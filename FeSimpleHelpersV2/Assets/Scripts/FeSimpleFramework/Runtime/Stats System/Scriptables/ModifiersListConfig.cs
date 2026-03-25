using System.Collections.Generic;
using FeSimpleHelpers.Core;
using UnityEngine;

namespace FeSimpleHelpers.StatsSystem
{
	[CreateAssetMenu(menuName = "FeSimpleFramework/Stats/Perk Config")]
	public class ModifiersListConfig : IDScriptableObject
	{
		public BaseStatsDefinition target;
		public List<StatModifier> modifiers;
	}
}