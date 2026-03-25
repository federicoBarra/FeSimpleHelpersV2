using XNode;

namespace FeSimpleHelpers.xNodeFSM.Nodes
{
	[NodeTint(0, 140, 0)]
	public class FSMStartNode : FSMBasicNode
	{
		//public bool useDebug;

		[Output] public int start;
		//[Output] public int debug;
		public Node GetFirstNode()
		{
			//if (useDebug)
			//{
			//	Node debugPort = GetOutputPort("debug")?.Connection?.node;
			//	return debugPort;
			//}

			Node outPort = GetOutputPort("start")?.Connection?.node;
			return outPort;
		}
	}
}

