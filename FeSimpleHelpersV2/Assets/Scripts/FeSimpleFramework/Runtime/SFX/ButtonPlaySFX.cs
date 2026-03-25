using UnityEngine;
using UnityEngine.UI;

namespace FeSimpleHelpers.SFX
{
	public class ButtonPlaySFX : MonoBehaviour
	{
		void Awake()
		{
			Button btn = GetComponent<Button>();
			btn.onClick.AddListener(SFXManager.S_PlayButtonSound);
		}
	}
}