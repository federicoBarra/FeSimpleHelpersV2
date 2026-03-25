using UnityEngine;

namespace FeSimpleHelpers.StatsSystem.Example
{
	public class WeaponExample : MonoBehaviour
	{
		public StatConfig damageStat;
		public StatConfig attackSpeed;
		public StatConfig projectilesAmount;

		//public StatHandler StatHandler;

		// Start is called once before the first execution of Update after the MonoBehaviour is created
		void Awake()
		{
		}

		public void TryAttack()
		{
			//Debug.Log("damageStat: " + StatHandler.GetStatFinalVal(damageStat));
		}
	}
}