using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class camerachange : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera frontwindow;
    public Camera rightwindow;
    public Camera leftwindow;
    void Start()
    {
        frontwindow.enabled=true;
        rightwindow.enabled=false;
        leftwindow.enabled=false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Keypad1))
        {
            frontwindow.enabled=true;
            rightwindow.enabled=false;
            leftwindow.enabled=false;
        }
        if(Input.GetKeyDown(KeyCode.Keypad2))
        {
            frontwindow.enabled=false;
            rightwindow.enabled=true;
            leftwindow.enabled=false;
        }
        if(Input.GetKeyDown(KeyCode.Keypad3))
        {
            frontwindow.enabled=false;
            rightwindow.enabled=false;
            leftwindow.enabled=true;
        }
    }
}
