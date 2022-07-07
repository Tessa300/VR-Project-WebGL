using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{

    private RingController ringController;
    private bool ringActive = false;

    private void Start()
    {
        ringController = FindObjectOfType<RingController>();
    }

    public void ActivateRing()
    {
        Debug.Log("Aktiviere ersten Ring");
        ringActive = true;
    }
    private void OnTriggerEnter(Collider collider)
    {
        
        if(ringActive)
        {
            ringController.NextRing();
        }
    }

    public bool IsActive()
    {
        return ringActive;
    }



}
