using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    int score;
    bool isUsed = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString();
    }

    private void OnTriggerExit(Collider collider) {
        if(collider.tag == "ring" && !isUsed) {
            isUsed = true;
            score++;
            Destroy(collider.gameObject);
        }
        if(collider.tag == "enemy") {
            score--;
        }
    }
}
