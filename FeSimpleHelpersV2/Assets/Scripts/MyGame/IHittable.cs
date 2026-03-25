using System;
using UnityEngine;

namespace MyGame.General
{
	public interface IHittable
	{
		GameObject gameObject { get; }
		//float Radius { get; }
		bool IsAlive { get; }
		bool CanReceiveDamage { get; }
		Vector3 Position { get; }
		bool IsTargeteable { get; }
		//Transform CachedTransform { get; }
		Transform PreferedHitPoint { get; }
		void ReceiveDamage(DamageInfo di);
		//event Action<IHittable> OnInvalidate;
	}

	/// <summary>
	/// This class is used to pass all the information about a damage event.
	/// Creator is private.
	/// TODO Create pool for this
	/// </summary>
	[Serializable]
	public class DamageInfo
	{
		public static DamageInfo muchDamage = new DamageInfo() { damage = 99999 };
		public static DamageInfo lowDamage = new DamageInfo() { damage = 1 };
		public static DamageInfo low10Damage = new DamageInfo() { damage = 10 };
		public static DamageInfo low70Damage = new DamageInfo() { damage = 70 };

		public bool IsCrashDamage => damageType == DamageType.Crash;

		public enum DamageType
		{
			Crash,
			Melee,
			Ranged,
			Other
		}

		public DamageType damageType;
		public float damage = 9999;
		public float damageMultiplier = 0;
		public float critChance = 0;
		public float critMultiplier = 0;
		public float knockbackChance = 0;
		public float pushStrenght = 0;

		public float applyPoisonChance;

		public float finalDamage = 9999;

		public Vector3 sourcePosition = Vector3.zero;
		public Vector3 hitPosition;
		public Transform damageOwner;

		private DamageInfo()
		{
		}

		public static DamageInfo NewDamage()
		{
			return new DamageInfo();
		}

		public static DamageInfo NewCrashDamage()
		{
			DamageInfo damage = new DamageInfo();
			damage.damageType = DamageType.Crash;
			return damage;
		}
	}
}