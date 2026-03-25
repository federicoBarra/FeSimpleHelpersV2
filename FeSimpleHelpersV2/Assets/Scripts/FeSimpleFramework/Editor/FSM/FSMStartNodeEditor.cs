#if UNITY_EDITOR
using XNodeEditor;

namespace FeSimpleHelpers.xNodeFSM.Nodes
{
	[NodeEditor.CustomNodeEditor(typeof(FSMStartNode))]
	public class FSMStartNodeEditor : NodeEditor
	{
		public const int SmallNodeWidth = 125;
		public override int GetWidth()
		{
			return SmallNodeWidth;
		}
	}
}
#endif