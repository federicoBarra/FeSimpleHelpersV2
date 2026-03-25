using System;
using System.Collections.Generic;
using System.Linq;
public class StrategyeitorInterface<T>// where T : interface
{
	private List<T> instances;

	List<string> instancesNames;

	private T current;
	public T Current => current;
	private string currentName;

	public StrategyeitorInterface()
	{
		Construct();
	}

	public void Construct()
	{
		var type = typeof(T);
		var types = AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(s => s.GetTypes())
			.Where(p => type.IsAssignableFrom(p));

		instances = new List<T>();
		instancesNames = new List<string>();

		List<Type> typesList = types.ToList();

		for (var i = 1; i < typesList.Count; i++)
		{
			var t = typesList[i];
			T newInstance = (T)Activator.CreateInstance(t);
			instances.Add(newInstance);
			instancesNames.Add(newInstance.GetType().ToString());
		}

		current = instances[0];
		currentName = current.ToString();
	}

	public List<string> GetList()
	{
		return instancesNames;
	}

	public void SetCurrent(string strategyName, bool force = false)
	{
		if (String.CompareOrdinal(currentName, strategyName) == 0 && !force)
			return;

		current = instances[instancesNames.IndexOf(strategyName)];
		currentName = strategyName;
	}
}

public static class InterfaceImplementedNames
{
	private static Dictionary<Type, List<string>> typeNames = new Dictionary<Type, List<string>>();

	public static List<string> GetList<T>()
	{
		List<string> names;
		if (typeNames.TryGetValue(typeof(T), out names))
		{
			return names;
		}

		var type = typeof(T);
		var types = AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(s => s.GetTypes())
			.Where(p => type.IsAssignableFrom(p));

		names = new List<string>();
		List<Type> typesList = types.ToList();

		for (var i = 1; i < typesList.Count; i++)
		{
			var t = typesList[i];
			names.Add(t.ToString());
		}

		typeNames.Add(typeof(T), names);

		return names;
	}
}