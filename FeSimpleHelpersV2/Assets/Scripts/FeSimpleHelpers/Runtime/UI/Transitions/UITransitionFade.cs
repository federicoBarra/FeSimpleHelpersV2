using System.Collections;
using UnityEngine;

namespace FeSimpleHelpers.UI
{
	/// <summary>
	/// Add this component for fade in/out animation in Canvas groups.
	/// </summary>
	public class UITransitionFade : UIWindowTransition
	{
		 
		[Space]
		public CanvasGroup cg = null; // Exposed as public so it can be used CanvasGroup from child objects

		protected void Awake()
		{
			if(cg == null )
				cg = GetComponent<CanvasGroup>();
		}

		//protected override void PreTransition(bool isIn)
		//{
		//	base.PreTransition(isIn);
		//	AnimationCurve curve = isIn ? inCurve : outCurve;
		//	cg.alpha = curve.Evaluate(0);
		//}

		protected override void Process(bool isIn, float percent)
		{
			base.Process(isIn, percent);
			AnimationCurve curve = isIn ? inCurve : outCurve;
			cg.alpha = curve.Evaluate(percent);
		}

		//protected override void PostTransition(bool isIn)
		//{
		//	base.PostTransition(isIn);
		//	AnimationCurve curve = isIn ? inCurve : outCurve;
		//	cg.alpha = curve.Evaluate(1);
		//}
	}
}