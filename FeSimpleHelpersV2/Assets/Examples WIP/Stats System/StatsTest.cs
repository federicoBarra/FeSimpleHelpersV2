using System.Collections.Generic;
using FeSimpleHelpers.StatsSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Example.StatsSystem
{
	public class StatsTest : MonoBehaviour
	{
		public List<GameObject> perksTargets;

		private Dictionary<BaseStatsDefinition, IStatHandler> statProviders = new Dictionary<BaseStatsDefinition, IStatHandler>();

		public List<ModifiersListConfig> perksExamples;

		void Start()
		{
			GatherProviders();
		}

		void GatherProviders()
		{
			statProviders.Clear();

			foreach (GameObject target in perksTargets)
			{
				IStatHandler[] providers = target.GetComponentsInChildren<IStatHandler>(includeInactive: true);
				foreach (IStatHandler statProvider in providers)
				{
					if (!statProviders.TryAdd(statProvider.BaseStatsDefinition, statProvider))
					{
						Debug.LogError("BaseStatsDefinition is duplicated => " + statProvider.BaseStatsDefinition );
					}
				}
			}

			Debug.Log("Providers gathered: ");
			foreach (KeyValuePair<BaseStatsDefinition, IStatHandler> pair in statProviders)
			{
				//Debug.Log(pair.Value.gameObject.name + " => " + pair.Key);
			}
		}


		// Update is called once per frame
		void Update()
		{
			if (Keyboard.current.qKey.wasPressedThisFrame)
			{
				foreach (ModifiersListConfig example in perksExamples)
					AddPerk(example);
			}

			if (Keyboard.current.rKey.wasPressedThisFrame)
			{
				foreach (ModifiersListConfig example in perksExamples)
					RemovePerk(example);
			}
		}

		void AddPerk(ModifiersListConfig modifiersList)
		{
			if (!statProviders.TryGetValue(modifiersList.target, out IStatHandler provider))
			{
				Debug.Log("provider for modifiersList not found");
				return;
			}
			foreach (StatModifier modifier in modifiersList.modifiers)
			{
				provider.AddModifier(modifier);
			}
		}

		void RemovePerk(ModifiersListConfig modifiersList)
		{
			if (!statProviders.TryGetValue(modifiersList.target, out IStatHandler provider))
			{
				Debug.Log("provider for modifiersList not found");
				return;
			}
			foreach (StatModifier modifier in modifiersList.modifiers)
			{
				provider.RemoveModifier(modifier);
			}
		}
	}
}