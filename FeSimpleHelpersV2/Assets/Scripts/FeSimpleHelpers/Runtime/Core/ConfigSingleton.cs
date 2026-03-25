using UnityEngine;

namespace FeSimpleHelpers.Core
{
	/// <summary>
	/// Extend from this class to create a singleton ScriptableObject that can be easily accessed from anywhere in the code using the Get() method.
	/// The asset created with this should be placed in Resources (or Resource/_Main Connfigs/) folder, and it will be loaded automatically when accessed for the first time (Lazy load).
	/// Best uses for this are Settings Configurations or ScriptableObjects containers.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ConfigSingleton<T> : ScriptableObject where T : ScriptableObject
	{
		private static T _instance;
		const string alterativePath = "_Main Configs/";
		public static T Get()
		{
			if (_instance == null)
			{
				_instance = Resources.Load<T>(typeof(T).Name);
				if (_instance == null)
				{
					_instance = Resources.Load<T>(alterativePath + typeof(T).Name);
				}
				(_instance as ConfigSingleton<T>)?.OnFirstLoad();
			}
			return _instance;
		}

		/// <summary>
		/// Use this method to load a child of other ConfigSingleton.
		/// Example MyGameCheats : CheatsConfig
		/// </summary>
		/// <typeparam name="W">Real type of the wanted ConfigSingleton</typeparam>
		/// <returns></returns>
		public static T Get<W>() 
		{
			if (_instance == null)
			{
				_instance = Resources.Load<T>(typeof(W).Name);
				if (_instance == null)
				{
					_instance = Resources.Load<T>(alterativePath + typeof(W).Name);
				}
				(_instance as ConfigSingleton<T>)?.OnFirstLoad();
			}
			return _instance;
		}

		public virtual void OnFirstLoad() { }
	}
}