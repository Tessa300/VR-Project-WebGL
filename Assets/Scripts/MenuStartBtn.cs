using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuStartBtn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Set init highscores
        if (!PlayerPrefs.HasKey("highscore_1"))
        {
            PlayerPrefs.SetInt("highscore_1", 0);
            PlayerPrefs.SetInt("highscore_2", 0);
            PlayerPrefs.SetInt("highscore_3", 0);
            PlayerPrefs.Save();
        }

        // Check if new score is a highscore and set score to text object
        if (PlayerPrefs.HasKey("gameScore"))
        {
            GameObject.Find("Score").SetActive(true);
            if (PlayerPrefs.GetInt("gameScore") >= PlayerPrefs.GetInt("highscore_1"))
            {
                PlayerPrefs.SetInt("highscore_3", PlayerPrefs.GetInt("highscore_2"));
                PlayerPrefs.SetInt("highscore_2", PlayerPrefs.GetInt("highscore_1"));
                PlayerPrefs.SetInt("highscore_1", PlayerPrefs.GetInt("gameScore"));
                PlayerPrefs.Save();
                GameObject.Find("Score").GetComponent<UnityEngine.UI.Text>().text = "New first place: ";
            }
            else if (PlayerPrefs.GetInt("gameScore") >= PlayerPrefs.GetInt("highscore_2"))
            {
                PlayerPrefs.SetInt("highscore_3", PlayerPrefs.GetInt("highscore_2"));
                PlayerPrefs.SetInt("highscore_2", PlayerPrefs.GetInt("gameScore"));
                PlayerPrefs.Save();
                GameObject.Find("Score").GetComponent<UnityEngine.UI.Text>().text = "New second place: ";
            }
            else if (PlayerPrefs.GetInt("gameScore") > PlayerPrefs.GetInt("highscore_3"))
            {
                PlayerPrefs.SetInt("highscore_3", PlayerPrefs.GetInt("gameScore"));
                PlayerPrefs.Save();
                GameObject.Find("Score").GetComponent<UnityEngine.UI.Text>().text = "New third place: ";
            }
            else
            {
                GameObject.Find("Score").GetComponent<UnityEngine.UI.Text>().text = "No new record: ";
            }

            GameObject.Find("Score").GetComponent<UnityEngine.UI.Text>().text += PlayerPrefs.GetInt("gameScore").ToString();
        }
        else
        {
            GameObject.Find("Score").SetActive(false);
        }

        // Set highscore texts
        GameObject.Find("Podium_1/Score").GetComponent<TMPro.TextMeshPro>().text = PlayerPrefs.GetInt("highscore_1").ToString();
        GameObject.Find("Podium_2/Score").GetComponent<TMPro.TextMeshPro>().text = PlayerPrefs.GetInt("highscore_2").ToString();
        GameObject.Find("Podium_3/Score").GetComponent<TMPro.TextMeshPro>().text = PlayerPrefs.GetInt("highscore_3").ToString();

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
