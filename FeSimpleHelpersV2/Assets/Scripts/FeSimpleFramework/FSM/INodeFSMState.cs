using System.Collections.Generic;
using UnityEngine;
using xNodeFSM.Nodes;

namespace xNodeFSM
{
	public interface INodeFSMState<TActionEnum,UConditionEnum> where TActionEnum : System.Enum where UConditionEnum : System.Enum
	{
		string StateName { get; }
		float StateMaxDuration { get; set; }
		float ElapsedTime { get; }
		void Create(INodeFSM<TActionEnum,UConditionEnum> ai, FSMBasicNode node, GameObject owner);
		void InitState();
		void EnterState();
		void UpdateState();
		void ExitState();

		List<FSMGenericConditionsNode<UConditionEnum>> ConditionNodes { get; }
	}
}