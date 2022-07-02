using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuStartBtn : MonoBehaviour
{
    public TextMeshPro textGameScore;
    public TextMeshPro textHighscore1;
    public TextMeshPro textHighscore2;
    public TextMeshPro textHighscore3;


    // Start is called before the first frame update
    void Start()
    {
        // Set init highscores
        if (!PlayerPrefs.HasKey("highscore_1"))
        {
            PlayerPrefs.SetInt("highscore_1", 0);
            PlayerPrefs.SetInt("highscore_2", 0);
            PlayerPrefs.SetInt("highscore_3", 0);
            PlayerPrefs.SetInt("gameScore", 0);
            PlayerPrefs.Save();
        }

        int highscore1 = PlayerPrefs.GetInt("highscore_1");
        int highscore2 = PlayerPrefs.GetInt("highscore_2");
        int highscore3 = PlayerPrefs.GetInt("highscore_3");
        int gameScore = PlayerPrefs.GetInt("gameScore");

        // Check if new score is a highscore and set score to text object
        if (gameScore != 0 && gameScore >= highscore3)
        {

            if (gameScore >= highscore1)
            {
                highscore3 = highscore2;
                highscore2 = highscore1;
                highscore1 = gameScore;
                textGameScore.text = "Neuer erster Platz: ";
            }
            else if (gameScore >= highscore2)
            {
                highscore3 = highscore2;
                highscore2 = gameScore;
                textGameScore.text = "Neuer zweiter Platz: ";
            }
            else // (gameScore >= highscore3)
            {
                highscore3 = gameScore;
                textGameScore.text = "Neuer dritter Platz: ";
            }

            PlayerPrefs.SetInt("highscore_1", highscore1);
            PlayerPrefs.SetInt("highscore_2", highscore2);
            PlayerPrefs.SetInt("highscore_3", highscore3);
            PlayerPrefs.Save();

            textGameScore.text += gameScore.ToString();
        }
        else
        {
            textGameScore.text = "Kein neuer Rekord " + gameScore.ToString();
        }

        // Set highscore texts
        textHighscore1.text = highscore1.ToString();
        textHighscore2.text = highscore2.ToString();
        textHighscore3.text = highscore3.ToString();
    }

   

    // Update is called once per frame
    void Update()
    {
        float x = Input.acceleration.x;
        float speed = 20f;
        transform.Translate(x * speed, 0, 0);

        // set left bound
        if (transform.position.x < -200)
        {
            Vector3 pos = transform.position;
            pos.x = -200;
            transform.position = pos;
        }
            

        // right bound reached -> start
        if (transform.position.x >= 207)
        {
            // start
            SceneManager.LoadScene(sceneName: "SampleScene");
        }
    }
}
