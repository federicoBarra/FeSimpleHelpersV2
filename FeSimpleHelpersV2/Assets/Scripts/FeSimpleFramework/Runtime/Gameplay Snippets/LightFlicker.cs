using FeSimpleHelpers.FeUtils;
using UnityEngine;

// Just Copy and modify
namespace FeSimpleHelpers.Snippets
{
	public class LightFlicker : MonoBehaviour
	{
		public Utils.CurverFloat intensity;
		public Utils.CurverFloat range;

		private float ti;
		private float tr;

		private Light l;

		void Start()
		{
			l = GetComponent<Light>();
		}

		// Update is called once per frame
		void Update()
		{
			ti += Time.deltaTime;
			tr += Time.deltaTime;

			l.intensity = intensity.EvalUnclamped(ti / intensity.duration);
			l.range = range.EvalUnclamped(tr / range.duration);

			if (ti >= intensity.duration)
				ti = 0;
			if (tr >= range.duration)
				tr = 0;
		}
	}
}