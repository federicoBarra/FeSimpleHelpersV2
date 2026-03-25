#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace FeSimpleHelpers.xNodeFSM
{
	[CustomNodeGraphEditor(typeof(FSMGraph))]
	public class FSMGraphEditor : NodeGraphEditor
	{
		private FSMGraph graph;

		public override void OnGUI()
		{
			base.OnGUI();
			if (graph == null)
				graph = target as FSMGraph;
			EditorGUI.LabelField(new Rect(0, 0, 300, 50), graph.name);
		}
	}
}
#endif