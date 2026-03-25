using FeSimpleHelpers.General;
using FeSimpleHelpers.UI;
using TMPro;
using UnityEngine;

namespace FeSimpleHelpers.UIDebug
{
	public class UICheatsFeedback : UIWindow
	{
		[Header("Cheat Sheet")]
		[SerializeField] 
		private GameObject allCheatsPanel;
		[SerializeField]
		private TMP_Text dumpText;

		[Header("Cheat Pressed")]
		[SerializeField]
		private GameObject activeCheatFeedback;
		[SerializeField]
		private TMP_Text activeCheatFeedbackText;
		[SerializeField] 
		private float cheatTimeOnScreen = 2;

		[Header("Level Info Screen")]
		[SerializeField]
		protected TMP_Text infoText;

		protected override void OnEnable()
		{
			base.OnEnable();
			CheatsConfig.OnDumpInfo += OnDumpInfo;
			CheatsConfig.OnCheatActivated += ShowActiveCheatFeedback;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			CheatsConfig.OnDumpInfo -= OnDumpInfo;
			CheatsConfig.OnCheatActivated -= ShowActiveCheatFeedback;
		}

		protected virtual void OnDumpInfo(string data)
		{
			//Toggle();
			allCheatsPanel.SetActive(!allCheatsPanel.activeInHierarchy);
			dumpText.text = data;
			dumpText.gameObject.SetActive(true);

			//LevelConfig lvlConfig = GeneralGameManager.Get().CurrenLevelConfig;
			//
			//if (!lvlConfig)
			//	return;
			//
			//string levelData = "";
			//levelData += "<color=green>Level Title: </color>" + lvlConfig.levelTitle + "\n";
			//levelData += "<color=green>Description: </color>" + lvlConfig.description + "\n";
			//levelData += "<color=green>Scene: </color>" + lvlConfig.scene + "\n";
			//levelData += "<color=green>Available: </color>" + lvlConfig.available + "\n";
			//levelData += "<color=green>Next Level: </color>" + (lvlConfig.nextLevel ? lvlConfig.nextLevel.levelTitle : "None") + "\n";
			//levelData += "<color=green>Next Cutscene: </color>" + (lvlConfig.nextCutscene ? lvlConfig.nextCutscene : "None") + "\n";
			//levelData += "<color=green>Load Main Menu: </color>" + lvlConfig.loadMainMenu + "\n";
			//levelData += "<color=green>Is Debug: </color>" + lvlConfig.isDebug + "\n";
			//levelData += "<color=green>Dev Name: </color>" + lvlConfig.owner + "\n";
			//levelData += "<color=green>Dev Info: </color>" + lvlConfig.devInfo + "\n";
			//infoText.text = levelData;
		}
		private void ShowActiveCheatFeedback(string cheatName)
		{
			activeCheatFeedback.SetActive(true);
			activeCheatFeedbackText.text = cheatName;
			CancelInvoke(nameof(HideCheatFeedbackText));
			Invoke(nameof(HideCheatFeedbackText), cheatTimeOnScreen);
		}

		private void HideCheatFeedbackText()
		{
			activeCheatFeedback.SetActive(false);
		}
	}
}