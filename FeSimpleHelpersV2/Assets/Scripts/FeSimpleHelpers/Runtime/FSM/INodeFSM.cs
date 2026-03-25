using UnityEngine;

namespace FeSimpleHelpers.xNodeFSM
{
	public interface INodeFSM<TActionEnum, UConditionEnum> where TActionEnum : System.Enum where UConditionEnum : System.Enum
	{
		void CreateFSM(GameObject _owner);
		void Process();
		void SetState(INodeFSMState<TActionEnum,UConditionEnum> es);
	}
}