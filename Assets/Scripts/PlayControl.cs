using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class PlayControl : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI endText;

    public AudioSource backgroundMusic;
    public AudioSource introVoiceSound;
    public AudioSource ringCrossingSound;
    public AudioSource crashPlanetSound;
    public AudioSource crashAsteriodSound;
    public AudioSource energyLowSound;
    public AudioSource energyEmptySound;
    public AudioSource winnerSound;

    private float energy = 100f;
    private float secondsBetweenDecreases = 1f;
    private float decreasePercentage = 0.03f;
    private float increasePercentage = 0.3f;
    private float lowEnergyWarnStart = 50f;
    private float lastDecrease = 0;
    private float ringIncreaseSpeedPercentage = 1f;

    private bool musicFadeOutEnabled = false;
    private bool running = false;
    private int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(E_startSequence());
    }

    // Update is called once per frame
    void Update()
    {
        if (running)
        {
            // Decrease energy
            if (Time.realtimeSinceStartup - lastDecrease >= secondsBetweenDecreases)
            {
                energy -= (energy * decreasePercentage > 2) ? energy * decreasePercentage : 2; // minimus decrease value is 2%
                lastDecrease = Time.realtimeSinceStartup;
            }

            // End game if energy is empty
            if (energy <= 0)
            {
                energy = 0;
                GetComponent<Renderer>().material.color = Color.red;
                // Stop game
                StartCoroutine(E_stopSequence(energyEmptySound, "Keine Energie mehr"));
            }
            else if (energy <= lowEnergyWarnStart && !energyLowSound.isPlaying)
            {
                // Energy low start
                energyLowSound.Play();
            }
            else if (energy > lowEnergyWarnStart && energyLowSound.isPlaying)
            {
                // Energy up after low
                energyLowSound.Stop();
                GetComponent<Renderer>().material.color = Color.white;
            }

            if (energy <= lowEnergyWarnStart)
            {
                // Energy low running -> Blink
                GetComponent<Renderer>().material.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time, 1));
            }
        }


        // Fade out background music if requestet
        if (musicFadeOutEnabled)
        {
            float newVolume = backgroundMusic.volume - (0.2f * Time.deltaTime);  //change 0.01f to something else to adjust the rate of the volume dropping
            if (newVolume < 0.1f)
                backgroundMusic.Stop();
            else
                backgroundMusic.volume = newVolume;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "finishline")
        {
            score++;
            // Stop game
            StartCoroutine(E_stopSequence(winnerSound, "Gewonnen"));
        }
        if (collider.tag == "ring")
        {
            score++;
            energy = (energy + energy * increasePercentage < 100) ? energy + energy * decreasePercentage : 100;
            Destroy(collider.gameObject); // Destroy Ring
            StartCoroutine(E_playSoundShowText(ringCrossingSound, ringIncreaseSpeedPercentage));
        }
        else if (collider.tag == "planet")
        {
            score--;
            // Stop game
            StartCoroutine(E_stopSequence(crashPlanetSound, "Von Planet gefressen"));
        }
        else if (collider.tag == "asteroid")
        {
            score--;
            Destroy(collider.gameObject);
            StartCoroutine(E_playSoundShowText(crashAsteriodSound, 0));
        }
        Debug.Log(collider.tag);
    }

    IEnumerator E_playSoundShowText(AudioSource playSource, float increaseSpeedPercentage)
    {
        AcceleratorCable ac = gameObject.GetComponent<AcceleratorCable>();
        playSource.Play();
        scoreText.text = "Score " + score.ToString() + "\n" + Math.Round(energy) + "%";
        if (increasePercentage != 0)
            ac.IncreaseSpeed(ringIncreaseSpeedPercentage);
        yield return new WaitForSeconds(playSource.clip.length);
        if (increasePercentage != 0)
            ac.DecreaseSpeed(ringIncreaseSpeedPercentage);
        scoreText.text = "";
    }

    IEnumerator E_startSequence()
    {
        score = 0;
        PlayerPrefs.SetInt("gameScore", score);
        scoreText.text = "";
        endText.text = "";
        AcceleratorCable ac = gameObject.GetComponent<AcceleratorCable>();
        introVoiceSound.Play();
        yield return new WaitForSeconds(introVoiceSound.clip.length);
        backgroundMusic.Play();
        endText.text = "3";
        yield return new WaitForSeconds(0.5f);
        endText.text = "2";
        yield return new WaitForSeconds(0.5f);
        endText.text = "1";
        yield return new WaitForSeconds(0.5f);
        endText.text = "";
        ac.moveAllowed = true;
        lastDecrease = Time.realtimeSinceStartup;
        running = true;
    }

    IEnumerator E_stopSequence(AudioSource playSource, string stopMessage)
    {
        running = false;
        AcceleratorCable ac = gameObject.GetComponent<AcceleratorCable>();
        ac.moveAllowed = false;
        endText.text = stopMessage;
        scoreText.text = "Score " + score.ToString() + "\n" + Math.Round(energy) + "%";
        PlayerPrefs.SetInt("gameScore", score);
        if (energyLowSound.isPlaying)
            energyLowSound.Stop();
        musicFadeOutEnabled = true;
        playSource.Play();
        yield return new WaitForSeconds(playSource.clip.length + 2);
        SceneManager.LoadScene(sceneName: "MenuScene");
    }


}
