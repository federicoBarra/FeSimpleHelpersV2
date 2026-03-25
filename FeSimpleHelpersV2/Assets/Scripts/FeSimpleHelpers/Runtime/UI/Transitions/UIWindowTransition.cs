using System.Collections;
using UnityEngine;

namespace FeSimpleHelpers.UI
{
    [RequireComponent(typeof(UIWindow))]
	public class UIWindowTransition : MonoBehaviour
	{
		[Header("In Transition")]
		public bool inMuted;
		public float inDuration = 0.5f;
		public AnimationCurve inCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

		[Header("Out Transition")]
		public bool outMuted;
		public float outDuration = 0.5f;
		public AnimationCurve outCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

		public bool Transitioning { get; set; }

		public bool IsMuted(bool isIn)
		{
			return isIn ? inMuted : outMuted;
		}

		public float GetDuration(bool isIn)
		{
			return isIn ? inDuration : outDuration;
		}

		/// <summary>
		/// Launch in/out Transition Animation
		/// </summary>
		/// <param name="isIn">true transition in / false transition out</param>
		public virtual void Launch(bool isIn)
		{
			PreTransition(isIn);
			if (gameObject.activeInHierarchy)
				StartCoroutine(Process(isIn));
		}

		public virtual void LaunchIn()
		{
			Launch(true);
		}

		public virtual void LaunchOut()
		{
			Launch(false);
		}

		protected virtual void PreTransition(bool isIn)
		{
			Transitioning = true;
		}

		IEnumerator Process(bool isIn)
		{
			float t = 0;
			float duration = isIn ? inDuration : outDuration;
			while (t <= duration)
			{
				Process(isIn, t/duration);
				t += Time.deltaTime;
				yield return null;
			}
			Process(isIn, 1.0f);
			yield return null;
			PostTransition(isIn);
		}
		protected virtual void Process(bool isIn, float percent)
		{
		}

		protected virtual void PostTransition(bool isIn)
		{
			Transitioning = false;
		}

		public virtual void StopTransition()
		{
			StopAllCoroutines();
		}
	}
}