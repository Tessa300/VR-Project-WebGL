using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingController : MonoBehaviour
{

    private List<Transform> rings = new List<Transform>();

    public Material activeRing;
    public Material inactiveRing;
    public Material finalRing;
    private int processedRings = 0;

    // Start is called before the first frame update
    void Start()
    {
         // process all rings from upper class Level1
        foreach(Transform child in transform)
        {
            Debug.Log("Ring");
            rings.Add(child);
            child.GetComponent<MeshRenderer>().material = inactiveRing;
        }

        rings[processedRings].GetComponent<MeshRenderer>().material = activeRing;
        rings[processedRings].GetComponent<Ring>().ActivateRing();

    }

    public void NextRing()
    {
        processedRings++;
        if(processedRings == rings.Count)
        {
            return;
        }

        if (processedRings == rings.Count -1 )
        {
            rings[processedRings].GetComponent<MeshRenderer>().material = finalRing;
        }
        else
        {
            rings[processedRings].GetComponent<MeshRenderer>().material = activeRing;
        }

        rings[processedRings].GetComponent<Ring>().ActivateRing();

    }

}
