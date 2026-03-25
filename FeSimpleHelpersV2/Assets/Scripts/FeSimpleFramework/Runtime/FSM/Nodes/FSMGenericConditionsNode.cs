using System;
using System.Collections.Generic;
using XNode;

namespace FeSimpleHelpers.xNodeFSM.Nodes
{
	public abstract class FSMGenericConditionsNode<T> : FSMBasicNode where T : Enum
	{
		[Serializable]
		public class ExitCondition
		{
			public T condition;
			public float value;
		}

		public List<ExitCondition> exitConditions;

		[Input] public float _in;
		[Output] public int out0;

		public Node GetNextConditionNode()
		{
			return GetOutputPort("out0")?.Connection?.node;
		}
	}
}