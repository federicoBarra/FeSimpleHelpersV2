using MyGame.General;
using UnityEngine;

namespace MyGame.Ingame
{
	public class Enemy : MonoBehaviour, IHittable
	{
		public bool IsAlive => true;
		public bool CanReceiveDamage => true;
		public Vector3 Position => transform.position;
		public bool IsTargeteable => true;
		public Transform PreferedHitPoint => transform;
		public void ReceiveDamage(DamageInfo di)
		{
			Debug.Log("Enemy received damage, do something");
		}
	}
}