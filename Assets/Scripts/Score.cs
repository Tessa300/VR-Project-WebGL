using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    int score;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString();
    }

    private void OnTriggerEnter(Collider collider) {
        if(collider.tag == "ring") {
            score++;
            Destroy(collider.gameObject);
        }
        if(collider.tag == "enemy") {
            score--;
        }
        Debug.Log(collider.tag);
    }
}
