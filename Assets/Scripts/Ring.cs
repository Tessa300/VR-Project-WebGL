using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{

    private Objective objectiveScript;
    private bool ringActive = false;

    private void Start()
    {
        objectiveScript = FindObjectOfType<Objective>();
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
            Debug.Log("Test");
            objectiveScript.NextRing();
        }
    }



}
