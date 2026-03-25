
namespace FeSimpleHelpers.FeRandom
{
	public class RandomProvider
	{
		private System.Random random;
		private int baseSeed;

		public RandomProvider(int seed)
		{
			baseSeed = seed;
			random = new System.Random(seed);
		}

		public void Clear()
		{
			random = new System.Random(baseSeed);
		}

		public int Range(int min, int max)
		{
			return random.Next(min, max);
		}

		public float Range(float min, float max)
		{
			return (float)(random.NextDouble() * (max - min) + min);
		}

		public bool Chance(float probability)
		{
			return random.NextDouble() < probability;
		}
	}
}