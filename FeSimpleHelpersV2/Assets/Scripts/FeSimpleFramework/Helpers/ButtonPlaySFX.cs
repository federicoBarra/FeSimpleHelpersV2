using UnityEngine;
using UnityEngine.UI;

namespace FeSimpleHelpers.General
{
	public class ButtonPlaySFX : MonoBehaviour
	{
		// Start is called before the first frame update
		void Awake()
		{
			Button btn = GetComponent<Button>();
			btn.onClick.AddListener(SFXManager.S_PlayButtonSound);
		}
	}
}