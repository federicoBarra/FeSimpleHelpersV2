using System;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using xNodeFSM.Nodes;

namespace xNodeFSM
{
	public abstract class NodeFSM<TActionEnum,UConditionEnum> : MonoBehaviour, INodeFSM<TActionEnum,UConditionEnum> where UConditionEnum : Enum where TActionEnum : Enum
	{
		protected GameObject owner;

		[SerializeField]
		protected FSMGraphController controller;

		protected Dictionary<TActionEnum, Type> nodeStateMap = new();

		protected Dictionary<FSMBasicNode, INodeFSMState<TActionEnum,UConditionEnum>> nodeInstanceMap = new();

		protected INodeFSMState<TActionEnum, UConditionEnum> previousState;
		protected INodeFSMState<TActionEnum, UConditionEnum> currentState;

		private INodeFSMState<TActionEnum, UConditionEnum> firstState;

		protected abstract void FillNodeStateMap();

		public void SetGraphAsset(FSMGraph graphAsset)
		{
			controller.Graph = graphAsset;
		}

		public virtual void CreateFSM(GameObject _owner)
		{
			owner = _owner;

			FillNodeStateMap();

			if (nodeStateMap == null || nodeStateMap.Count <= 0)
			{
				Debug.LogError("Node State Map is null or empty", controller.Graph);
				return;
			}

			foreach (Node graphNode in controller.Graph.nodes)
			{
				if (graphNode is FSMGenericActionNode<TActionEnum>)
				{
					FSMGenericActionNode<TActionEnum> actionNode = graphNode as FSMGenericActionNode<TActionEnum>;

					if (nodeStateMap.TryGetValue(actionNode.action, out var stateType))
					{
						INodeFSMState<TActionEnum, UConditionEnum> stateInstance = (INodeFSMState<TActionEnum, UConditionEnum>)Activator.CreateInstance(stateType);
						stateInstance.Create(this, actionNode, owner);
						stateInstance.InitState();
						nodeInstanceMap.Add(actionNode, stateInstance);
					}
					else
					{
						Debug.LogError("Error here, check");
					}
				}
			}

			FSMStartNode startNode = null;
			// search controller.Graph start node and assign currentState to it's repective nodeInstanceMap
			foreach (Node graphNode in controller.Graph.nodes)
			{
				if (graphNode is FSMStartNode node)
				{
					startNode = node;
					break;
				}
			}

			if (startNode == null)
			{
				Debug.LogError("Non Start node in AI", controller.Graph);
				return;
			}

			FSMGenericActionNode<TActionEnum> firstActionNode = startNode.GetFirstNode() as FSMGenericActionNode<TActionEnum>;

			if (firstActionNode == null)
			{
				Debug.LogError("First node must be an action node", controller.Graph);
				return;
			}


			if (!nodeInstanceMap.TryGetValue(firstActionNode, out var value))
			{
				Debug.LogError("Invalid first action node", controller.Graph);
				return;
			}

			firstState = value;
		}

		public virtual void ResetFSM()
		{
			SetState(firstState);
		}

		public void Process()
		{
			if (currentState != null)
			{
				currentState.UpdateState();
				CheckCurrentStateConditions();
			}
		}

		public virtual void SetState(FSMBasicNode node)
		{
			if (nodeInstanceMap.ContainsKey(node))
			{
				SetState(nodeInstanceMap[node]);
			}
		}

		public void SetState(INodeFSMState<TActionEnum, UConditionEnum> es)
		{
			previousState = currentState;
			previousState?.ExitState();

			currentState = es;

			if (currentState == null)
			{
				Debug.LogError("SetState with null state");
				return;
			}

			currentState.EnterState();
		}

		protected abstract void CheckCurrentStateConditions();
	}
}