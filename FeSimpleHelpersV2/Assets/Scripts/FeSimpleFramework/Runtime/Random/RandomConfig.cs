using System.Collections.Generic;
using FeSimpleHelpers.Core;
using UnityEngine;

namespace FeSimpleHelpers.FeRandom
{
	[CreateAssetMenu(menuName = "FeSimpleFramework/Main Configs/RandomConfig")]
	public class RandomConfig : ConfigSingleton<RandomConfig>
	{
		[Header("Master Seed")]
		public bool useMasterSeed = true;
		public int masterSeed = 12345;

		private Dictionary<string, RandomProvider> providers = new();

		private int runtimeSeed;

		public override void OnFirstLoad()
		{
			Init();
		}
		//[Button("Clear")]
		public void Init()
		{
			providers = new();
			runtimeSeed = useMasterSeed
				? masterSeed
				: UnityEngine.Random.Range(int.MinValue, int.MaxValue);

			Debug.Log($"RandomConfig seed = {runtimeSeed}");
		}

		public RandomProvider InitRandomForSystem(string systemName)
		{
			if (providers == null)
				Init();

			if (providers.TryGetValue(systemName, out var provider))
			{
				provider.Clear();
				return provider;
			}

			int seed = Hash(runtimeSeed, systemName);

			provider = new RandomProvider(seed);

			providers.Add(systemName, provider);

			return provider;
		}

		private int Hash(int seed, string key)
		{
			unchecked
			{
				int hash = seed;
				foreach (char c in key)
					hash = hash * 31 + c;
				return hash;
			}
		}
	}
}