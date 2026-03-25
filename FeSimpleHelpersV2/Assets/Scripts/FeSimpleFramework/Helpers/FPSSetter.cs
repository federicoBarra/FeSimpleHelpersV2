using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSSetter : MonoBehaviour
{
	public int targetFPS = 60;

	private int lastFPS;
    void Start()
    {
	    QualitySettings.vSyncCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (lastFPS != targetFPS)
            Set();
    }

    void Set()
    {
	    Application.targetFrameRate = targetFPS;
	    lastFPS = targetFPS;
    }
}
