using System;
using System.Collections.Generic;
using FeSimpleHelpers.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using Key = UnityEngine.InputSystem.Key;

namespace FeSimpleHelpers.General
{
	/// <summary>
	/// Unique Scriptable Object to handle cheats in the game.
	/// CreateAssetMenu is commented here as one would not create this asset directly, but extend it to create a custom CheatsConfig (MyGameCheatsConfig) for the game.
	/// Cheats can be added dynamically during runtime, and they will be cleared when exiting play mode.
	/// This is useful to have cheats related directly to specific systems, scenes or contexts.
	/// </summary>
	//[CreateAssetMenu(fileName = "CheatsConfig", menuName = "FeSimpleFramework/Main Configs/CheatsConfig")]
	public class CheatsConfig : ConfigSingleton<CheatsConfig>
	{
		public bool verbose;

		public static event Action<string> OnDumpInfo; // string is info to dump
		public static event Action<string> OnCheatActivated;

		[Serializable]
		public class CheatCommand
		{
			public CheatCommand(string name, Action callback, Key mainKey, Key modifierKey)
			{
				this.name = name;
				this.callback = callback;
				this.mainKey = mainKey;
				this.modifierKey = modifierKey;
			}

			public string name;
			public Action callback;
			public Key mainKey;
			public Key modifierKey;
		}

		List<CheatCommand> staticCheats = new List<CheatCommand>();
		List<CheatCommand> dynamicCheats = new List<CheatCommand>();
		List<CheatCommand> allCheats = new List<CheatCommand>();

		List<string> cheatInfo = new List<string>();

		public override void OnFirstLoad()
		{
			base.OnFirstLoad();
			if (verbose)
				Debug.Log("First Load cheats");
			Recreate();
		}

		void OnDestroy()
		{
			if (verbose)
				Debug.Log("On Destroy");
		}

		public virtual void Recreate()
		{
			cheatInfo.Clear();
			cheatInfo = new List<string>();
			dynamicCheats.Clear();
			staticCheats.Clear();
			allCheats.Clear();
			allCheats.AddRange(staticCheats);
		}

		public bool AddCheat(Action callback, Key mainKey)
		{
			return AddCheat(callback, mainKey, Key.None);
		}

		public bool AddCheat(Action callback, Key mainKey, string cheatDescription)
		{
			return AddCheat(callback.Method.Name, callback, mainKey, Key.None, cheatDescription);
		}

		public bool AddCheat(string cheatName, Action callback, Key mainKey)
		{
			return AddCheat(callback, mainKey, Key.None, cheatName);
		}

		public bool AddCheat(string cheatName, Action callback, Key mainKey, Key modifierKey)
		{
			return AddCheat(callback, mainKey, modifierKey, cheatName);
		}

		public bool AddCheat(string cheatName, Action callback, Key mainKey, string cheatDescription)
		{
			return AddCheat(callback, mainKey, Key.None, cheatName, cheatDescription);
		}

		public bool AddCheat(string cheatName, Action callback, Key mainKey, Key modifierKey, string cheatDescription)
		{
			return AddCheat(callback, mainKey, modifierKey, cheatName, cheatDescription);
		}

		public bool AddCheat(Action callback, Key mainKey, Key modifierKey, string cheatName = null,
			string cheatDescription = null)
		{
			cheatName = string.IsNullOrEmpty(cheatName) ? callback.Method.Name : cheatName;

			if (verbose)
				Debug.Log("add Cheat: " + cheatName);
			for (int i = 0; i < allCheats.Count; i++)
			{
				var cheat = allCheats[i];
				if (cheat.mainKey == mainKey && cheat.modifierKey == modifierKey)
				{
					Debug.LogError(cheatName + " cannont be added, already a cheat with those commands: " +
					               mainKey +
					               " + " + modifierKey);
					return false;
				}
			}

			CheatCommand newCheat = new CheatCommand(cheatName, callback, mainKey, modifierKey);
			allCheats.Add(newCheat);
			dynamicCheats.Add(newCheat);
			string newCheatInfo = "";

			if (modifierKey != Key.None)
				newCheatInfo += "<color=yellow>" + modifierKey + "</color> + ";
			newCheatInfo += "<color=green>" + mainKey + "</color> ";
			newCheatInfo += "\t" + cheatName;
			if (!string.IsNullOrEmpty(cheatDescription))
				newCheatInfo += "<color=grey> - " + cheatDescription + "</color>";
			cheatInfo.Add(newCheatInfo);
			return true;
		}

		public void StartPlayState()
		{
			if (verbose)
				Debug.Log("StartPlayState");
			Recreate();
		}

		public void Process()
		{
			CheatCommand targetCheat = null;

			int i = 0;
			while (i < allCheats.Count)
			{
				CheatCommand cheat = allCheats[i];
				if (Keyboard.current[cheat.mainKey].wasPressedThisFrame)
				{
					targetCheat = cheat;
					while (i < allCheats.Count)
					{
						CheatCommand cheat02 = allCheats[i];
						if (cheat02.modifierKey != Key.None && targetCheat.mainKey == cheat02.mainKey &&
						    Keyboard.current[cheat02.modifierKey].isPressed)
						{
							targetCheat = cheat02;
							i = allCheats.Count;
						}

						i++;
					}
				}

				i++;
			}

			targetCheat?.callback();
			if (targetCheat != null)
			{
				if (verbose)
					Debug.Log("Cheat activated: " + targetCheat.name);
				OnCheatActivated?.Invoke(targetCheat.name);
			}
		}

		public void EndPlayState()
		{
			//running = false;
			if (verbose)
				Debug.Log("EndPlayState");
			dynamicCheats.Clear();
			staticCheats.Clear();
			allCheats.Clear();
		}

		public void DumpCheatInfo()
		{
			string dump = "";

			if (verbose)
				Debug.Log("Cheats: ");
			foreach (string s in cheatInfo)
			{
				dump += s + "\n";
				if (verbose)
					Debug.Log(s);
			}

			OnDumpInfo?.Invoke(dump);
		}
	}
}