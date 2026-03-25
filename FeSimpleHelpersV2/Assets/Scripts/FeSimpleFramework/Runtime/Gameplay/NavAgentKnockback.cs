using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentKnockback : MonoBehaviour
{
	public float defaultKnockbackDistance = 0.8f;
	public float duration = 1;
	public AnimationCurve curve;

	private NavMeshAgent agent;
	//private Animator animator;
	private bool knockingBack;
	public bool BeingKnocked => knockingBack;
	private float knockbackDistance = 0.8f;
	private bool prevAgentUpdatePosition;
	private bool prevAgentUpdateRotation;

	void Awake()
    {
	    agent = GetComponent<NavMeshAgent>();
		//animator = GetComponentInChildren<Animator>();
    }

    public void Knockback(Vector3 sourcePos, float pushStrenght = 1, float distance = -1)
    {
	    //Debug.Log("Knockback");
		if (knockingBack)
			return;

		knockbackDistance = distance >= 0 ? distance: defaultKnockbackDistance;

		knockbackDistance *= pushStrenght;

	    knockingBack = true;

	    prevAgentUpdatePosition = agent.updatePosition;
	    prevAgentUpdateRotation = agent.updateRotation;

		agent.updatePosition = false;
	    agent.updateRotation = false;
		//animator.SetTrigger(knockBackName);

		sourcePos.y = 0;
		Vector3 dir = (transform.position - sourcePos).normalized;

		StartCoroutine(DoKnockback(dir));
    }

    IEnumerator DoKnockback(Vector3 direction)
    {
		Transform trans = agent.transform;
		agent.transform.rotation = Quaternion.LookRotation(-direction);
		Vector3 lastPos = trans.position;
		Vector3 newPos = trans.position + direction * knockbackDistance;
		float t = 0;

		while (t<=duration)
		{
			float eval = curve.Evaluate(t / duration);
			trans.position = Vector3.Lerp(lastPos, newPos, eval);
			t += Time.deltaTime;
			yield return null;
		}

	    yield return null;
		
	    agent.nextPosition = trans.position;
		agent.updatePosition = prevAgentUpdatePosition;
		agent.updateRotation = prevAgentUpdateRotation;
		knockingBack = false;
    }
}
