using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace FeSimpleHelpers.FeUtils
{
	/// <summary>
	/// A compendium of useful classes and methods that don't fit in a specific category, but are still useful to have around.
	/// </summary>
	public static class Utils
	{
		[Serializable]
		public class LerpedFloat
		{
			[SerializeField] protected float speed;
			[Header("Debug")] [SerializeField] protected float target;
			[SerializeField] protected float current;

			public void SetTarget(float val, float newSpeed = -1)
			{
				target = val;
				if (newSpeed > 0)
					speed = newSpeed;
			}

			public void SetSpeed(float val)
			{
				speed = val;
			}

			public void Set(float val)
			{
				current = val;
				target = val;
			}

			public void Process(float dt) => current = Mathf.Lerp(current, target, dt * speed);

			public float Get() => current;
		}

		[Serializable]
		public class CurverFloat
		{
			public AnimationCurve curve;
			public float duration = 1;
			public float min = 0;
			public float max = 1;
			public float Eval(float t) => Mathf.Lerp(min, max, curve.Evaluate(t));
			public float EvalUnclamped(float t) => Mathf.LerpUnclamped(min, max, curve.Evaluate(t));
		}

		[Serializable]
		public class CurverV3
		{
			public AnimationCurve curve;
			public Vector3 min = Vector3.zero;
			public Vector3 max = Vector3.one;
			public Vector3 EvalUnclamped(float t) => Vector3.LerpUnclamped(min, max, curve.Evaluate(t));
		}

		[Serializable]
		public class ErrorFloat
		{
			public float val;
			public float error;

			public float Get(float errorFactor = -1)
			{
				float ret = val;
				float errorVal = Random.Range(-error, error);

				errorVal = errorFactor >= 0 ? errorVal * errorFactor : errorVal;
				ret += errorVal;

				return ret;
			}
		}

		public static Vector3 RandomV3(float min, float max)
		{
			Vector3 ret = Vector3.zero;
			ret.x = Random.Range(min, max);
			ret.y = Random.Range(min, max);
			ret.z = Random.Range(min, max);
			return ret;
		}

		/// <summary>
		/// Kept for legacy reasons.
		/// Use RandomConfig system to have control over rnd seeds.
		/// </summary>
		public static int RandomWrapper(int min, int maxNonInclusive)
		{
			return Random.Range(min, maxNonInclusive);
		}

		/// <summary>
		/// Kept for legacy reasons.
		/// Use RandomConfig system to have control over rnd seeds.
		/// </summary>
		public static float RandomWrapper(float min, float max)
		{
			return Random.Range(min, max);
		}

		public static bool Dice01(float chance)
		{
			float rnd = RandomWrapper(0f, 1f);
			return chance > rnd;
		}

		public static T GiveRandom<T>(T[] list) where T : Object
		{
			if (list == null || list.Length <= 0)
				return null;

			int rnd = RandomWrapper(0, list.Length);
			return list[rnd];
		}

		public static T GiveRandom<T>(List<T> list) where T : Object
		{
			if (list == null || list.Count <= 0)
				return null;

			int rnd = RandomWrapper(0, list.Count);
			return list[rnd];
		}


		public static bool HasFloatChanged(float newVal, ref float lastVal)
		{
			float dif = newVal - lastVal;
			lastVal = newVal;
			return (Mathf.Abs(dif) > 0.001f);
		}

		public static bool HasIntChanged(int newVal, ref int lastVal)
		{
			int dif = newVal - lastVal;
			lastVal = newVal;
			return dif != 0;
		}

		public static void ClearContentEditor(Transform content, Transform except = null)
		{
			for (int i = content.childCount - 1; i >= 0; i--)
			{
				Transform child = content.GetChild(i);
				if (child == except)
					continue;
				child.SetParent(null);
				GameObject.DestroyImmediate(child.gameObject);
			}
		}

		public const float kRelocateDistance = 50.0f;
		public const int kRelocateAreaMask = 1;
		public const int kRelocateAgentID = -1;

		public static Vector3 GetPointInNavmesh(Vector3 pos)
		{
			NavMeshHit hit;
			NavMeshQueryFilter filter = new NavMeshQueryFilter();
			filter.agentTypeID = kRelocateAgentID;
			filter.areaMask = kRelocateAreaMask;
			if (NavMesh.SamplePosition(pos, out hit, kRelocateDistance, filter))
				pos = hit.position;
			else if (NavMesh.FindClosestEdge(pos, out hit, 0))
				pos = hit.position;

			return pos;
		}

		public static bool ValidateUniqueID(ref string uniqueID)
		{
			if (!string.IsNullOrEmpty(uniqueID))
				return false;

			Guid id = Guid.NewGuid();
			uniqueID = id.ToString();

			return true;
		}

		//TODO this should be in an editor utils class
		public static void GatherSOs<T>(List<T> list, string folderName) where T : Object
		{
			if (list != null)
				list.Clear();
			else
				list = new List<T>();

			T[] mConfigs = Resources.LoadAll<T>(folderName);
			for (var i = 0; i < mConfigs.Length; i++)
			{
				var m = mConfigs[i];
				list.Add(m);
			}
		}

		public static float Mitigate(float val, float val02, float multiplier = 1)
		{
			return val - val * val02 * multiplier;
		}

		public static float Acentuate(float val, float val02, float multiplier = 1)
		{
			return val + val * val02 * multiplier;
		}

		public static int AcentuateInt(int val, float val02, float multiplier = 1)
		{
			int add = (int)(val * val02 * multiplier);
			if (add == 0)
				add++;
			return val + add;
		}

		public static float Sum(float val, float val02, float multiplier = 1)
		{
			return val + val02 * multiplier;
		}

		public static Vector2 GetAngledPos(float radius, float angle)
		{
			float rad = angle * Mathf.Deg2Rad;
			// x = r * cos(a) | y = r * sin(a)
			float x = radius * Mathf.Cos(rad);
			float y = radius * Mathf.Sin(rad);
			return new Vector2(x, y);
		}
	}
}