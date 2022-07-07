using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratorCable : MonoBehaviour
{
    public bool moveAllowed = false;
    private CharacterController controller;
    private float speed  = 40.0f;
    private float rotateSpeedX = 20.0f; // right - left
    private float rotateSpeedY = 10.0f; // up - down
    private float maxPlaneRotation = 15f;
    private float inputX;
    private float inputZ;
    private Vector3 moveVector;
    private Vector3 yaw;
    private Vector3 pitch;
    private  Vector3 direction;


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
        moveVector = transform.forward * speed;

        // Gather players Input

        // Get the delta direction
        setInputX();
        setInputZ();
        setYaw();
        setPitch();
        setDirection();   
    
        if (direction.sqrMagnitude > 1)
            direction.Normalize();


        if(checkUpDownLimitation()){
            addDirection(direction);
            transform.rotation = Quaternion.LookRotation(moveVector);
        }
       

        // move character
        controller.Move(moveVector * Time.deltaTime);

        // rotate the plane
        rotateCharacter();

    }

    public void rotateCharacter()
    {
        //float valPlaneRotation = GameObject.Find("Paperplane").transform.rotation.eulerAngles.z;
        float valPlaneRotation = GameObject.Find("Main Camera").transform.rotation.eulerAngles.z;
        if (valPlaneRotation > 360 - maxPlaneRotation || valPlaneRotation < maxPlaneRotation // > 340 || < 20
            || (valPlaneRotation < 360 - maxPlaneRotation && valPlaneRotation > 180 && inputX > 0) // < 340 & > 180 & -
            || (valPlaneRotation > maxPlaneRotation && valPlaneRotation < 180 && inputX < 0)) // > 20 && < 180 & +
        {
            GameObject.Find("Main Camera").transform.Rotate(0, 0, inputX);
            GameObject.Find("Paperplane").transform.Rotate(0, 0, inputX * -1);
        }
    }

    public void setPitch()
    {
        pitch = inputZ * transform.up * rotateSpeedY * Time.deltaTime;
    }

    public void setYaw()
    {
        yaw = inputX * transform.right * rotateSpeedX * Time.deltaTime;
    }

    public void setDirection()
    {
        direction = yaw + pitch;
    }

    public void setInputX()
    {
        inputX =  Input.acceleration.x;
    }

    public void setInputZ()
    {
        inputZ = Input.acceleration.z;
    }
    public void addDirection(Vector3 direction)
    {
        moveVector += direction;
    }

    public bool checkUpDownLimitation()
    {
        float maxXvalue = Quaternion.LookRotation(moveVector + direction).eulerAngles.x;
        if(maxXvalue < 90 && maxXvalue > 70 || maxXvalue > 270 && maxXvalue < 290)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void IncreaseSpeed(float increasePercentage)
    {
        speed = (speed * (1 + increasePercentage) > 0) ? speed * (1 + increasePercentage) : 0;
    }

    public void DecreaseSpeed(float decreasePercentage)
    {
        // OR: IncreaseSpeed((1/(decreasePercentage+1))-1);
        speed = (speed / (1 + decreasePercentage) > 0) ? speed / (1 + decreasePercentage) : 0;
    }

}