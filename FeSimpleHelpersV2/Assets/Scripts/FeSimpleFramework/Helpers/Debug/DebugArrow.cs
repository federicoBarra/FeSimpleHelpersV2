using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugArrow : MonoBehaviour
{
	public Transform pivot;
	public float fat = 0.2f;
	public float lengthMultiplier = 1;
	public Color color;

	void Awake()
	{
		SetFat(fat);
	}

	public void Init(Color c, string _name, float _fat = 0)
	{
		SetColor(c);
		gameObject.name = _name;
		if (_fat <= 0)
			_fat = fat;
		SetFat(_fat);
	}

	public void SetLengthMultiplier(float _mult)
	{
		lengthMultiplier = _mult;
	}

	public void SetColor(Color c)
	{
		color = c;
		Renderer[] rends = GetComponentsInChildren<Renderer>();
		foreach (Renderer rend in rends)
		{
			rend.material.color = color;
		}
	}
	public void SetFat(float _fat)
	{
		fat = _fat;
		pivot.transform.localScale = new Vector3(fat, fat, 1);
	}

	public void SetLength(float _length)
	{
		transform.localScale = new Vector3(1, 1, _length * lengthMultiplier);
	}

	public void SetPosRotLength(Vector3 pos, Quaternion rot, float _length)
	{
		transform.position = pos;
		transform.rotation = rot;
		SetLength(_length);
	}
}
