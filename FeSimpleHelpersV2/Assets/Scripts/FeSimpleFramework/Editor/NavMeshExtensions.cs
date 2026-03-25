#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public static class TransformExtensions
{
	public const float kRelocateDistance = 50.0f;
	public const int kRelocateAreaMask = 1;
	public const int kRelocateAgentID = -1;

	[MenuItem("CONTEXT/Transform/Place on NavMesh")]
	public static void PlaceOnNavMesh(MenuCommand command)
	{
		Transform transform = (Transform)command.context;
		PlaceOnNavMesh(transform);
	}

	public static void PlaceOnNavMesh(Transform transform)
	{
		NavMeshHit hit;
		NavMeshQueryFilter filter = new NavMeshQueryFilter();
		filter.agentTypeID = kRelocateAgentID;
		filter.areaMask = kRelocateAreaMask;
		Debug.DrawLine(transform.position, transform.position + Vector3.down * kRelocateDistance, Color.green);
		if (NavMesh.SamplePosition(transform.position, out hit, kRelocateDistance, filter))
			transform.position = hit.position;
		else if (NavMesh.FindClosestEdge(transform.position, out hit, kRelocateAreaMask))
			transform.position = hit.position;
	}
}
#endif