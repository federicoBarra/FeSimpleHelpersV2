using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FeSimpleHelpers.FeUtils
{
	public static class PhysicsUtils
	{
		private static Collider[] collidersOverlapingStatic = new Collider[32];

		public static void AngledOverlapSphereNonAlloc32(Transform caller, Vector3 pos, Vector3 dir, float angle,
			float range, LayerMask hittablesLayer, List<GameObject> hittables)
		{
			int amount = Physics.OverlapSphereNonAlloc(pos, range, collidersOverlapingStatic, hittablesLayer);
			hittables.Clear();

			Vector3 rangedForward = pos + dir * range;

			for (int i = 0; i < amount; i++)
			{
				Collider c = collidersOverlapingStatic[i];
				Vector3 closest = c.ClosestPoint(rangedForward);
				closest.y = pos.y;
				if (c.transform == caller || c.transform.parent == caller)
					continue;
				if (Vector3.Angle(dir, closest - pos) < angle / 2)
					hittables.Add(c.transform.gameObject);
			}
		}
	}
}