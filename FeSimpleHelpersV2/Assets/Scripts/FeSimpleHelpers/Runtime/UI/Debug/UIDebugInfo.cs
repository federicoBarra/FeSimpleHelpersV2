using System.Collections.Generic;
using FeSimpleHelpers.UI;
using TMPro;
using UnityEngine;

namespace FeSimpleHelpers.UIDebug
{
	/// <summary>
	/// TODO should have an unset but just restart the level... idk
	/// </summary>
	public class UIDebugInfo : UIWindowSingleton<UIDebugInfo>
	{
		[SerializeField]
		private TMP_Text mainText;
		public class NodeSet
		{
			public string name;
			public string value;
			public Dictionary<string, NodeSet> nodeSet;
			public bool HasValue => nodeSet is not { Count: > 0 };
		}

		private bool hasChanged = false;
		private NodeSet rootNode = new NodeSet();
		private Dictionary<string, NodeSet> fastLookupTable = new Dictionary<string, NodeSet>();

		public const char Separator = '|';


		protected override void Awake()
		{
			base.Awake();
			ClearInternal();
			//Set("Current Level", "2");
			//Set("Tito/Position", "984621");
			//Set("Tito/Speed", "6456541");
			//Set("Enemies/Count", "6456541");
			//Set("Enemies/Alive", "564");
		}

		void Update()
		{
			if (!hasChanged)
				return;
			hasChanged = false;

			mainText.text = GetText(rootNode);
		}

		void SetInternal(string path, string value)
		{
			if (fastLookupTable.TryGetValue(path, out var node))
			{
				if (node.value != value)
				{
					hasChanged = true;
					node.value = value; // Overwrite
				}
				return;
			}

			hasChanged = true;

			NodeSet current = rootNode;
			NodeSet lastSet = CreateNode(current, path, value);
			fastLookupTable[path] = lastSet;
		}

		void ClearInternal()
		{
			rootNode = new NodeSet();
			rootNode.name = "";
			fastLookupTable = new Dictionary<string, NodeSet>();
			hasChanged = true;
		}

		//this supposses the node does not exist
		NodeSet CreateNode(NodeSet parent, string path, string value)
		{
			int firstMatch = path.IndexOf(Separator);

			string name = firstMatch > 0 ?path.Substring(0,firstMatch ) : path;

			NodeSet newNode = null;
			if (parent.nodeSet == null)
				parent.nodeSet = new Dictionary<string, NodeSet>();
			else
			{
				parent.nodeSet.TryGetValue(name, out newNode);
			}

			if (newNode == null)
			{
				newNode = new NodeSet();
				newNode.name = name;
				newNode.value = value;
				parent.nodeSet[name] = newNode;
			}

			if (firstMatch < 0)
			{
				return newNode;
			}

			return CreateNode(newNode, path.Substring(firstMatch + 1, path.Length-1 - firstMatch), value);
		}
		string GetText(NodeSet nodeSet, int indentLevel = -1)
		{
			string indent = indentLevel>= 0 ? new string(' ', indentLevel) : "";
			string s = "";

			if (nodeSet.HasValue)
			{
				s += indent + nodeSet.name + ": " + nodeSet.value + "\n";
			}
			else
			{
				s += indent + nodeSet.name + "\n";
				foreach (KeyValuePair<string, NodeSet> pair in nodeSet.nodeSet)
				{
					s += GetText(pair.Value, indentLevel+1);
				}
			}
			return s;
		}

		public static void Set(string name, string value)
		{
			Instance?.SetInternal(name, value);
		}

		public static void Clear()
		{
			Instance?.ClearInternal();
		}
	}
}