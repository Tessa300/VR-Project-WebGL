using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceMoreRandom : MonoBehaviour
{
    public GameObject prefab;

    public float amountNewObjects = 20;
    private float positionRange = 7000;
    public float minDistanceToOthers = 100;


    // Start is called before the first frame update
    void Start()
    {
        // Instantiate at position (0, 0, 0) and zero rotation.
        Instantiate(prefab, new Vector3(0, 0, 600), Quaternion.identity);

        for(int i = 0; i < amountNewObjects; i++)
        {
            GameObject newObject = Instantiate(prefab, FindNewPos(), Quaternion.identity);
            newObject.transform.parent = gameObject.transform;
        }

    }

    private Vector3 FindNewPos()
    {
        Vector3 newPos;
        Collider[] neighbours = new Collider[0];
        int counter = 0;
        do
        {
            counter++;
            newPos = new Vector3(Random.Range(-positionRange, positionRange), Random.Range(-positionRange, positionRange), Random.Range(-positionRange, positionRange));
            // get neighbours inside minDistance
            neighbours = Physics.OverlapSphere(newPos, minDistanceToOthers);
        } while (neighbours.Length > 0 && counter < 50);

        if (counter == 50)
            newPos = new Vector3(0, 0, -20); // Default possition

        return newPos;
    }
}
