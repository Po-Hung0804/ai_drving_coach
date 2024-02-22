using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class steeringwhellcontrol : MonoBehaviour
{
   
    public float rotationSpeed = 200f;
    public float maxRotationAngle = 45f;
    public float currentrotation=0f;
    
    public float rotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float horizontalInput = Input.GetAxis("Horizontal");
        
        rotation = horizontalInput * rotationSpeed * Time.deltaTime;
        currentrotation += rotation;
        
        if (currentrotation>=maxRotationAngle)
        {
            rotation = 0f;
            transform.Rotate(0f, rotation, 0f, Space.Self);
            currentrotation = maxRotationAngle;

        }
        else if (currentrotation <= maxRotationAngle * -1)
        {
            rotation = 0f;
            transform.Rotate(0f, rotation,0f, Space.Self);
            currentrotation = -maxRotationAngle;
        }
        else
        {
            transform.Rotate(0f, rotation, 0f, Space.Self);
        }
       
        if (Mathf.Approximately(horizontalInput, 0f) && !Mathf.Approximately(currentrotation, 0f))
        {
            float returnrotation = Mathf.Lerp(currentrotation, 0f, Time.deltaTime * 10f);
            transform.Rotate(0f, returnrotation - currentrotation, 0f, Space.Self);
            currentrotation = returnrotation;
        }


    }
}
