using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratorCable : MonoBehaviour
{
    float speed = 10.0f;
    int counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        counter = counter +1;
       Vector3 dir = Vector3.zero;
        dir.x = Input.acceleration.y;
        //dir.z = Input.acceleration.x;
       // if (dir.sqrMagnitude > 1)
       //     dir.Normalize();

        dir *= Time.deltaTime;

        // Move object
        //Debug.Log(dir*speed);
        float x = Input.acceleration.x;
        //transform.Translate(x, 0,(-1*Input.acceleration.z));
        transform.Rotate(0,0,-x);
        if(counter == 60){
         //Debug.Log("X" + Input.acceleration.x);
        Debug.Log(Input.acceleration.y);
        counter = 0;
        }
   

        

    }
}