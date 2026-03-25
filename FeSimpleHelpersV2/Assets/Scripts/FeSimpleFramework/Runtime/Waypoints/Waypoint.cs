using System.Collections.Generic;
using UnityEngine;

namespace FeSimpleHelpers
{
	public class Waypoint : MonoBehaviour
	{
		public List<Waypoint> neightbours = new List<Waypoint>();
		public bool IsEndPoint => neightbours.Count <= 1;
		[SerializeField]
		private bool startPoint;
		public bool IsStartPoint => startPoint;
		public Waypoint GetRandomWaypoint(Waypoint exclude = null)
		{
			Waypoint ret = null;

			int rnd = Random.Range(0, neightbours.Count);
			ret = neightbours[rnd];
			if (ret == exclude)
			{
				rnd++;
				rnd %= neightbours.Count;
				ret = neightbours[rnd];
			}
			return ret;
		}

		public bool HasNeightbour(Waypoint w)
		{
			if (neightbours == null)
				return false;
			return neightbours.Contains(w);
		}

		public void AddNeightbour(Waypoint w)
		{
			if (neightbours == null)
				neightbours = new List<Waypoint>();
			neightbours.Add(w);
		}

		void OnDrawGizmosSelected()
		{
			if (neightbours == null || neightbours.Count<= 0)
				return;

			Gizmos.color = Color.red;

			Vector3 yOffset = Vector3.up * 0.01f;

			foreach (Waypoint neightbour in neightbours)
			{
				Gizmos.DrawLine(transform.position + yOffset, neightbour.transform.position + yOffset);
			}
		}
	}
}

