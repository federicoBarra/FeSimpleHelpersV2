using System;

namespace FeSimpleHelpers.xNodeFSM.Nodes
{
	public abstract class FSMGenericActionNode<T> : FSMBasicNode where T : Enum
	{
		public T action;

		[Input] public float _in;
		[Output] public int out0;
	}
}