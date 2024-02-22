using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float movespeed=5f;
    public float backspeed = 2f;
    public float rotatespeed = 12f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.D)){
            transform.Rotate(0, rotatespeed * Time.deltaTime, 0);
        }
       if(Input.GetKey(KeyCode.A)){
            transform.Rotate(0, -1 * rotatespeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.S)){
            transform.Translate(0,0,-1*backspeed*Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W)){
            transform.Translate(0,0,movespeed*Time.deltaTime);
        }
    }
}
