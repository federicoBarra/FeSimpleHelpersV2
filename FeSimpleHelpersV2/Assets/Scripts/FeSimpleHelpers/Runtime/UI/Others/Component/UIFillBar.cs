using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FeSimpleHelpers.UI
{
	public class UIFillBar : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text textNumber;
		[SerializeField]
		public Image fillBar;

		public void Set(float val, float max)
		{
			fillBar.fillAmount = val / max;
			if (textNumber)
				textNumber.text = val.ToString("N0");
		}
	}
}