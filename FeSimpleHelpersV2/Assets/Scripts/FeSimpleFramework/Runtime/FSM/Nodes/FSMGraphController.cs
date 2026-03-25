using UnityEngine;

namespace FeSimpleHelpers.xNodeFSM
{
	public class FSMGraphController : MonoBehaviour
	{
		private FSMGraph graphAsset;
		public FSMGraph Graph
		{
			get { return graphAsset; }
			set { graphAsset = value; }
		}
	}
}
