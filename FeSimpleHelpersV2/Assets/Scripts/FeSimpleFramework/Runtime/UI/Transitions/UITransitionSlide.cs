using UnityEngine;

namespace FeSimpleHelpers.UI
{
    public class UITransitionSlide : UIWindowTransition
	{
		[Header("Slide Values")]
		public RectTransform target;
		public Vector2 openPos;
		public Vector2 closedPos;
		//public bool transitionFromCurrentPos;

		Vector3 initPos;
		Vector3 wantedPosition;

		protected void Awake()
		{
			if (!target)
				Debug.LogError("No movable panel for transition");
		}

		protected override void PreTransition(bool isIn)
		{
			base.PreTransition(isIn);
			if (!target)
				return;

			initPos = closedPos;
			//if (transitionFromCurrentPos)
			//	initPos = target.anchoredPosition;
			wantedPosition = openPos;
		}

		protected override void Process(bool isIn, float percent)
		{
			base.Process(isIn, percent);
			if (!target)
				return;
			AnimationCurve curve = isIn ? inCurve : outCurve;
			float val = curve.Evaluate(percent);
			target.anchoredPosition = initPos + (wantedPosition - initPos) * val;
		}
	}
}