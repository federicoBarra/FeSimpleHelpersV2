using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class UIDamageText : MonoBehaviour
{
	public TMP_Text text;

	public Utils.ErrorFloat totalDuration = new(){val = 0.5f, error = 0.1f};
	public AnimationCurve translationCurve;
	public Utils.CurverFloat size;
	public float sizeOffsetError = 0.1f;
	float sizeOffset = 0.5f;

	public Gradient colorGrad;
	public Vector3 endOffset;
	public float offsetError = 0.1f;

	private Vector3 startPos;
	private Vector3 endPos;

	public void Launch(string _t, Vector3 position)
	{
		text.text = _t;
		startPos = position;
		endPos = startPos + endOffset + Utils.RandomV3(-offsetError, offsetError);

		sizeOffset = Random.Range(-sizeOffsetError, sizeOffsetError);

		StartCoroutine(Animate());
	}

	IEnumerator Animate()
	{
		float duration = totalDuration.Get();
		float t = duration;
		while (t>=0)
		{
			t -= Time.deltaTime;
			float factor = 1 - t / duration;
			Vector3 newPos = Vector3.LerpUnclamped(startPos, endPos, translationCurve.Evaluate(factor));
			text.color = colorGrad.Evaluate(factor);
			text.fontSize = size.EvalUnclamped(factor) + sizeOffset;

			transform.position = newPos;
			yield return null;
		}
		yield return null;
		gameObject.SetActive(false);
	}
}



//void NormalUpdate()
//{
//	translationT += translationSpeed * Time.deltaTime;
//	zoomT += zoomSpeed * Time.deltaTime;
//
//	Vector3 newLooAt = Vector3.LerpUnclamped(lastLookAtPos, wantedTransform.position, translationCurve.Evaluate(translationT / translationDuration));
//
//	Vector3 newOffset = Vector3.LerpUnclamped(lastOffset, wantedOffset, translationCurve.Evaluate(translationT / translationDuration));
//
//	float newZoom = Mathf.LerpUnclamped(lastZoom, wantedZoom, zoomCurve.Evaluate(zoomT / zoomDuration));
//
//	if (translationT > translationDuration && zoomT > zoomDuration)
//	{
//		camTransform.position = wantedTransform.position + wantedOffset.normalized * wantedZoom;
//		camTransform.LookAt(wantedTransform.position);
//	}
//	else
//	{
//		Vector3 newPos = newLooAt + newOffset.normalized * newZoom;
//		camTransform.position = newPos;
//		Quaternion lookRot = Quaternion.LookRotation(newLooAt - camTransform.position);
//		camTransform.rotation = lookRot;
//	}
//}