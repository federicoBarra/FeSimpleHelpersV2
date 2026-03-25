using UnityEngine;

namespace FeSimpleHelpers
{
	public class WaypointsHandler : MonoBehaviour
	{
		private Waypoint[] waypoints;
		public Waypoint[] Path => waypoints;
		[SerializeField] private bool oneWay;
		public bool IsOneWay => oneWay;

		public bool IsEndPoint(Waypoint w)
		{
			if (oneWay && w.IsStartPoint)
				return false;

			return w.IsEndPoint;
		}

		void Awake()
		{
			waypoints = GetComponentsInChildren<Waypoint>();
		}

		public void RefreshWaypoints(bool addBackTracks = false)
		{
			waypoints = GetComponentsInChildren<Waypoint>();

			if (!addBackTracks)
				return;

			foreach (Waypoint waypoint in waypoints)
			{
				if (waypoint.neightbours != null)
				{
					foreach (Waypoint neightbour in waypoint.neightbours)
					{
						if (!neightbour.HasNeightbour(waypoint))
						{
							neightbour.neightbours.Add(waypoint);
							Debug.Log("Adding " + waypoint.name + " to " + neightbour.name);
						}
					}
				}
			}
		}
	}
}