using System.Collections.Generic;
using FeSimpleHelpers.UI;
using TMPro;
using UnityEngine;

namespace FeSimpleHelpers.UIDebug
{
	public class UIDebugConsole : UIWindowSingleton<UIDebugConsole>
	{
		[Header("Console")]
		[SerializeField] 
		private TMP_Text mainText;
		[SerializeField] 
		private RectTransform textsContainer;
		[SerializeField] 
		private TMP_Text textPrefab;

		private Dictionary<string, TMP_Text> textsByTag;

		protected override void Awake()
		{
			base.Awake();
			textsByTag = new Dictionary<string, TMP_Text>();
		}

		public static void SetConsoleText(string t)
		{
			Get().mainText.text = t;
		}

		public void SetTag(string tag, string val)
		{
			TMP_Text tmpText;

			if (!textsByTag.TryGetValue(tag, out tmpText))
			{
				TMP_Text newText = Instantiate(textPrefab, textsContainer);
				textsByTag.Add(tag, newText);
			}

			if (tmpText)
				tmpText.text = tag + val;
		}
	}
}