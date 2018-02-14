using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

 public class RotateBaseButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    public bool isRightButton;
    private bool Left = false;
    private bool Right = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        if(isRightButton)
        {
            Right = true;

        }else
        {
            Left = true;

        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isRightButton)
        {
            Right = false;

        }
        else
        {
            Left = false;

        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Right)
        {
            MainScript.script.LaunchBase.transform.Rotate(new Vector3(0f, 0f, -0.5f));
        }

        if (Left)
        {
            MainScript.script.LaunchBase.transform.Rotate(new Vector3(0f, 0f, 0.5f));
        }
	}
}
