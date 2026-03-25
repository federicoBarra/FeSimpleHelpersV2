using System.Collections.Generic;
using UnityEngine;
using XNode;
using System;
using FeSimpleHelpers.xNodeFSM.Nodes;

namespace FeSimpleHelpers.xNodeFSM
{
	public class NodeFSMStateBase<TActionEnum, UConditionEnum> : INodeFSMState<TActionEnum, UConditionEnum>
		where UConditionEnum : Enum where TActionEnum : Enum
	{
		public virtual string StateName => "unnamed";

		public virtual float StateMaxDuration { get; set; }
		public float ElapsedTime => timeInState;

		protected GameObject owner;
		protected FSMBasicNode node;
		protected NodePort outPort;

		protected List<FSMGenericConditionsNode<UConditionEnum>> conditionNodes;
		public List<FSMGenericConditionsNode<UConditionEnum>> ConditionNodes => conditionNodes;
		protected float timeInState;

		public virtual void Create(INodeFSM<TActionEnum, UConditionEnum> ai, FSMBasicNode _node, GameObject _owner)
		{
			owner = _owner;
			node = _node;
			outPort = _node.GetOutputPort("out0");
			conditionNodes = new List<FSMGenericConditionsNode<UConditionEnum>>();

			for (int i = 0; i < outPort.ConnectionCount; i++)
			{
				NodePort conn = outPort.GetConnection(i);
				if (conn.node as FSMGenericConditionsNode<UConditionEnum>)
				{
					conditionNodes.Add(conn.node as FSMGenericConditionsNode<UConditionEnum>);
				}
			}
		}

		public virtual void InitState()
		{
		}

		public virtual void EnterState()
		{
			//Debug.Log(StateName + " State Enter");
			timeInState = 0;
		}

		public virtual void UpdateState()
		{
			timeInState += Time.deltaTime;
			//Debug.Log(StateName + "State Update");
		}

		public virtual void ExitState()
		{
			//Debug.Log(StateName + " State Exit");
		}
	}
}