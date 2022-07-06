using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratorCable : MonoBehaviour
{
    public bool moveAllowed = false;
    float speed = 100.0f;
    int counter = 0;
    private CharacterController controller;
    private float baseSpeed = 40.0f;
    private float rotateSpeedX = 20.0f; // r - l
    private float rotateSpeedY = 10.0f; // u - d

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!moveAllowed)
            return;

        // Give the player forward velocity
        Vector3 moveVector = transform.forward * baseSpeed;

        // Gather players Input

        // Get the delta direction
        float inputX = Input.acceleration.x;
        Vector3 yaw = Input.acceleration.x * transform.right * rotateSpeedX * Time.deltaTime;
        Vector3 pitch = Input.acceleration.z * transform.up * rotateSpeedY * Time.deltaTime;
        Vector3 dir = yaw + pitch;

        // Make sure we limit the player from doing a loop
        float maxX = Quaternion.LookRotation(moveVector + dir).eulerAngles.x;

        //if he does not going to far up/down, add the direction to the moveVector
        if(maxX < 90 && maxX > 70 || maxX > 270 && maxX < 290)
        {
            // too far
        }
        else
        {
            // add the direction to current move
            moveVector += dir;

            // have the player face where is is going
            transform.rotation = Quaternion.LookRotation(moveVector);
        }

        // move him
        controller.Move(moveVector * Time.deltaTime);

        // rotate the plane
        float maxPlaneRotation = 20f;
        float valPlaneRotation = GameObject.Find("Paperplane").transform.rotation.eulerAngles.z;
        if(valPlaneRotation > 360 - maxPlaneRotation || valPlaneRotation < maxPlaneRotation // > 340 || < 20
            || (valPlaneRotation < 360 - maxPlaneRotation && valPlaneRotation > 180 && inputX < 0) // < 340 & > 180 & -
            || (valPlaneRotation > maxPlaneRotation && valPlaneRotation < 180 && inputX > 0)) // > 20 && < 180 & +
            GameObject.Find("Paperplane").transform.Rotate(0, 0, inputX * -1);


        /*
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
           //transform.Translate(x, (Input.acceleration.z), 0);
           transform.Translate(x, 0 , 0);
           transform.Translate(0, 0, 0.1f);
           transform.Rotate(0,0,-x);
           if(counter == 60){
            //Debug.Log("X" + Input.acceleration.x);
           Debug.Log(Input.acceleration.y);
           counter = 0;
           }
           */




    }

    public void IncreaseSpeed(float increasePercentage)
    {
        baseSpeed = (baseSpeed * (1 + increasePercentage) > 0) ? baseSpeed * (1 + increasePercentage) : 0;
    }

    public void DecreaseSpeed(float decreasePercentage)
    {
        // OR: IncreaseSpeed((1/(decreasePercentage+1))-1);
        baseSpeed = (baseSpeed / (1 + decreasePercentage) > 0) ? baseSpeed / (1 + decreasePercentage) : 0;
    }

}