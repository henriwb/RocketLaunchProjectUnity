using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parachute : MonoBehaviour {
    [HideInInspector]
    public Rigidbody body;
    public ConstantForce constForce;
	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        constForce.force = new Vector3(MainScript.script.windForce, 0f, 0f);
    }

    public void CallRoutine()
    {
        StartCoroutine(ApplyDragRoutine(body));
    }

    IEnumerator ApplyDragRoutine(Rigidbody body)
    {
        while (body.drag < 2f)
        {
            yield return new WaitForSeconds(1f);
            body.drag += 0.1f;
        }
    }
}
