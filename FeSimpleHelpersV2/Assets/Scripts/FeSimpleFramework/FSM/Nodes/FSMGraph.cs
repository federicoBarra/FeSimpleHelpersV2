using UnityEngine;
using XNode;
using xNodeFSM.Nodes;

namespace xNodeFSM
{
	[CreateAssetMenu(fileName = "NodeFSM Graph", menuName = "FeSimpleFramework/NodeFSM/New Graph", order = 1)]
	public class FSMGraph : NodeGraph
	{
		public Node GetStart()
		{
			FSMStartNode startNode = (FSMStartNode)nodes.Find(x => x.name == "Start");
			return startNode.GetFirstNode();
		}
	}
}