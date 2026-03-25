using XNode;

namespace xNodeFSM.Nodes
{
	public class FSMBasicNode : Node
	{
		public virtual Node GetNext(int i, string portNamePrefix = "out")
		{
			string nextName = portNamePrefix + i;
			Node outPort = GetOutputPort(nextName)?.Connection?.node;
			if (outPort == null)
				outPort = GetOutputPort(portNamePrefix)?.Connection?.node;
			return outPort;
		}

		public override object GetValue(NodePort port)
		{
			return null;
		}
	}
}