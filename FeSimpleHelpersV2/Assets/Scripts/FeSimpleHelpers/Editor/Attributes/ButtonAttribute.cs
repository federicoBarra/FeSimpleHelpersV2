using UnityEngine;

[System.AttributeUsage(System.AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
public class ButtonAttribute : PropertyAttribute
{
	public readonly string Label;

	public ButtonAttribute(string label = null)
	{
		Label = label;
	}
}
