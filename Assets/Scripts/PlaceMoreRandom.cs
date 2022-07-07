using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceMoreRandom : MonoBehaviour
{
    // Game object that is copied
    public GameObject prefab;

    public float amountNewObjects = 20;
    public float minDistanceToOthers = 100;
    private float positionRange = 7000;

    
    void Start()
    {
        // Instantiate at position with zero rotation
        Instantiate(prefab, new Vector3(0, 0, 600), Quaternion.identity);

        for(int i = 0; i < amountNewObjects; i++)
        {
            // Create new object at empty detected position
            GameObject newObject = Instantiate(prefab, FindNewPos(), Quaternion.identity);
            // Add as child to parent -> better overview in scene
            newObject.transform.parent = gameObject.transform;
        }

    }

    /// <summary>
    /// Finds a position for the new object with a given distance to other objects
    /// </summary>
    /// <returns>new Vector3 position of the object</returns>
    private Vector3 FindNewPos()
    {
        Vector3 newPos;
        Collider[] neighbours = new Collider[0];
        int counter = 0;
        do
        {
            counter++;
            // new position by random values
            newPos = new Vector3(Random.Range(-positionRange, positionRange), Random.Range(-positionRange, positionRange), Random.Range(-positionRange, positionRange));
            // get neighbours inside minDistance
            neighbours = Physics.OverlapSphere(newPos, minDistanceToOthers);
        } while (neighbours.Length > 0 && counter < 50);

        if (counter == 50)
            newPos = new Vector3(0, 0, -20); // Default position after 50 trys

        return newPos;
    }
}
