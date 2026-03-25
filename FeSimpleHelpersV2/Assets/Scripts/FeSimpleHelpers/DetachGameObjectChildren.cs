using UnityEngine;

namespace FeSimpleHelpers.General
{
	/// <summary>
	/// See use of this on "Level 01" scene, "LEVEL NEEDED OBJECTS" GO.
	/// </summary>
	public class DetachGameObjectChildren : MonoBehaviour
	{
		public string startName = "--------- START OF Scene Needed Game Objects -------";
		public string endName = "--------- END OF Scene Needed Game Objects -------";

		public bool startFromCurrentChildIndex = true;

		void Start()
		{

			int startIndex = 0;

			if (startFromCurrentChildIndex)
			{
				startIndex = transform.GetSiblingIndex();
			}

			int sceneIndex = startIndex + 1;

			for (int i = 0; i < transform.childCount; ++i)
			{
				Transform child = transform.GetChild(i);
				child.SetParent(null);
				child.SetSiblingIndex(sceneIndex);
				sceneIndex++;
				i--;
			}

			transform.SetSiblingIndex(startIndex);
			gameObject.name = startName;
			GameObject separator = new GameObject(endName);
			separator.transform.SetSiblingIndex(sceneIndex);
		}
	}
}