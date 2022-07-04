using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{

    private List<Transform> rings = new List<Transform>();

    public Material activeRing;
    public Material inactiveRing;
    public Material finalRing;

    private int ringPassed = 0;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform t in transform)
        {
            Debug.Log("Ring");
            rings.Add(t);
            t.GetComponent<MeshRenderer>().material = inactiveRing;
        }

        rings[ringPassed].GetComponent<MeshRenderer>().material = activeRing;
        rings[ringPassed].GetComponent<Ring>().ActivateRing();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextRing()
    {
        ringPassed++;
        if(ringPassed == rings.Count)
        {
            Victory();
            return;
        }

        if (ringPassed == rings.Count -1 )
        {
            rings[ringPassed].GetComponent<MeshRenderer>().material = finalRing;
        }
        else
        {
            rings[ringPassed].GetComponent<MeshRenderer>().material = activeRing;
        }

        rings[ringPassed].GetComponent<Ring>().ActivateRing();

    }

    private void Victory()
    {
        return;
    }
}
