using System;
using System.Collections.Generic;
using UnityEngine;

// ACHTUNG! This system needs more tweaking and organization.
// It is also, heavily unoptimized.

namespace FeSimpleHelpers.StatsSystem
{
	public interface IStatHandlerProvider
	{
		StatHandler Handler { get; }
	}
	public interface IStatHandler
	{
		BaseStatsDefinition BaseStatsDefinition { get; }
		List<StatModifier> Modifiers { get; }

		bool AddModifier(StatModifier sm);
		bool RemoveModifier(StatModifier sm);
	}

	public class StatHandler : IStatHandler
	{
		public BaseStatsDefinition BaseStatsDefinition { get; set; }

		private List<IStatHandler> providers = new List<IStatHandler>();

		public List<IStatHandler> Providers => providers;

		private Dictionary<StatConfig, List<StatModifier>> modifiersByStat = new Dictionary<StatConfig, List<StatModifier>>();
		private Dictionary<StatConfig, float> statBaseValue = new Dictionary<StatConfig, float>();

		private List<StatConfig> statsList = new List<StatConfig>();

		public List<StatConfig> StatsList => statsList;

		public event Action OnStatsChanged;

		public void Init()
		{
			RegisterProvider(this, true);
		}

		public void RegisterProvider(IStatHandler p, bool invalidate = false)
		{
			if (!providers.Contains(p))
				providers.Add(p);
			if (invalidate)
				Invalidate();
		}

		public void UnregisterProvider(IStatHandler p, bool invalidate = false)
		{
			providers.Remove(p);
			if (invalidate)
				Invalidate();
		}

		public void Invalidate()
		{
			//add all stats
			statBaseValue.Clear();
			statsList.Clear();
			foreach (IStatHandler provider in Providers)
			{
				foreach (BaseStatsDefinition.StatVal statVal in provider.BaseStatsDefinition.stats)
				{
					//Debug.Log("statVal.stat: " + statVal.stat);
					//Debug.Log("statVal.baseValue: " + statVal.baseValue);
					if (statBaseValue.TryGetValue(statVal.stat, out var value))
					{
						statBaseValue[statVal.stat] += statVal.baseValue;
					}
					else
					{
						statBaseValue.Add(statVal.stat, statVal.baseValue);
						statsList.Add(statVal.stat);
					}
					//Debug.Log("statBaseValue[statVal.stat]: " + statBaseValue[statVal.stat]);
				}
			}

			//add all modifiers
			var modByStat = modifiersByStat; //made local for fast access
			modByStat.Clear();

			foreach (IStatHandler provider in Providers)
			{
				foreach (StatModifier modifier in provider.Modifiers)
				{
					if (!modByStat.TryGetValue(modifier.statType, out var modifiersForStat))
					{
						modifiersForStat = new List<StatModifier>();
						modByStat[modifier.statType] = modifiersForStat;
						modifiersForStat.Add(modifier.GetCopy());
					}
					else
					{
						for (int i = 0; i < modifiersForStat.Count; i++)
						{
							StatModifier cachedModifier = modifiersForStat[i];
							if (modifier.FastEquals(cachedModifier))
							{
								cachedModifier.value += modifier.value;
								break;
							}
						}
					}
				}
			}
			OnStatsChanged?.Invoke();
		}

		public float GetStatFinalVal(StatConfig st)
		{
			if (Providers.Count <= 0)
				Init();

			if (!statBaseValue.TryGetValue(st, out float val))
			{
#if UNITY_EDITOR
				Debug.LogError($"Stat {st.displayName} not found for {BaseStatsDefinition} in stat handler. Returning 0. Make sure to call Invalidate() after adding providers or modifiers.");
#endif
			}

			modifiersByStat.TryGetValue(st, out var modifiers);

			if (modifiers == null || modifiers.Count <= 0)
				return val;

			//TODO optimize this

			foreach (StatModifier modifier in modifiers)
			{
				if (modifier.operation == StatOperation.Add)
					val = modifier.process(val);
			}

			foreach (StatModifier modifier in modifiers)
			{
				if (modifier.operation == StatOperation.Multiply)
					val = modifier.process(val);
			}

			foreach (StatModifier modifier in modifiers)
			{
				if (modifier.operation == StatOperation.FinalAdd)
					val = modifier.process(val);
			}

			foreach (StatModifier modifier in modifiers)
			{
				if (modifier.operation == StatOperation.Override)
					val = modifier.process(val);
			}

			val = Mathf.Clamp(val, st.minValue, st.maxValue);

			return val;
		}

		protected List<StatModifier> modifiers= new List<StatModifier>();
		public List<StatModifier> Modifiers => modifiers;

		public bool AddModifier(StatModifier sm)
		{
			if (modifiers == null)
				return false;

			if (modifiers.Contains(sm))
				return false;

			modifiers.Add(sm);
			return true;
		}

		public bool RemoveModifier(StatModifier sm)
		{
			if (modifiers == null)
				return false;

			if (!modifiers.Contains(sm))
				return false;

			modifiers.Remove(sm);
			return true;
		}

		public string DebugListStats()
		{
			string statsString = "";

			for (var i = 0; i < StatsList.Count; i++)
			{
				var stat = StatsList[i];
				float baseVal = 0;
				statBaseValue.TryGetValue(stat, out baseVal);
				float finalVal = GetStatFinalVal(stat);
				statsString += $"<color=yellow>{stat.displayName}</color> - Base: {baseVal} - Final: {finalVal}\n";
			}

			return statsString;
		}
	}
}