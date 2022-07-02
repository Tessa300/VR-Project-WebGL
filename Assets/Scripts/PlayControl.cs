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
    public AudioSource crashAsteriodSound; // TODO: use
    public AudioSource energyLowSound;
    public AudioSource energyEmptySound;
    public AudioSource winnerSound; // TODO: use

    private float energy = 100f;
    private float secondsBetweenDecreases = 1f;
    private float decreasePercentage = 0.03f;
    private float increasePercentage = 0.3f;
    private float lowEnergyWarnStart = 50f;
    private float lastDecrease = 0;

    private bool musicFadeOutEnabled = false;
    private bool running = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(E_startSequence());
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score " + PlayerPrefs.GetInt("gameScore").ToString() + "\n" + Math.Round(Time.realtimeSinceStartup) + "s\n " + Math.Round(energy) +"%";
        if (running)
        {
            // Decrease energy
            if (Time.realtimeSinceStartup - lastDecrease >= secondsBetweenDecreases)
            {
                energy -= (energy * decreasePercentage > 2) ? energy * decreasePercentage : 2;
                lastDecrease = Time.realtimeSinceStartup;
            }

            // End game if energy is empty
            if (energy <= 0)
            {
                // Stop game
                StartCoroutine(E_stopSequence(energyEmptySound, "Keine Energie mehr"));
            }else if (energy <= lowEnergyWarnStart && !energyLowSound.isPlaying) // Warn if energy is to low
                energyLowSound.Play();
            else if(energy > lowEnergyWarnStart && energyLowSound.isPlaying) // Stop warn
                energyLowSound.Stop();
        }


        // Fade out background music if requestet
        if(musicFadeOutEnabled)
        {
            float newVolume = backgroundMusic.volume - (0.2f * Time.deltaTime);  //change 0.01f to something else to adjust the rate of the volume dropping
            if (newVolume < 0.1f)
                backgroundMusic.Stop();
            else
                backgroundMusic.volume = newVolume;
        }
    }

    private void OnTriggerEnter(Collider collider) {
        int score = PlayerPrefs.GetInt("gameScore");
        if (collider.tag == "ring") {
            ringCrossingSound.Play(); // Sound
            score++; // Increase Score
            energy = (energy + energy * increasePercentage < 100) ? energy + energy * decreasePercentage : 100;
            Destroy(collider.gameObject); // Destroy Ring
        }
        if(collider.tag == "enemy") {
            score--;
            // Stop game
            StartCoroutine(E_stopSequence(crashPlanetSound, "Von Planet gefressen"));
        }
        if(collider.tag == "asteroid") {
            score--;
            Destroy(collider.gameObject);
        }
        PlayerPrefs.SetInt("gameScore", score);
        Debug.Log(collider.tag);
    }

    IEnumerator E_startSequence()
    {
        PlayerPrefs.SetInt("gameScore", 0);
        scoreText.text = "0";
        endText.text = "";
        AcceleratorCable ac = gameObject.GetComponent<AcceleratorCable>();
        introVoiceSound.Play();
        yield return new WaitForSeconds(introVoiceSound.clip.length + 1);
        backgroundMusic.Play();
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
        if (energyLowSound.isPlaying)
            energyLowSound.Stop();
        musicFadeOutEnabled = true;
        playSource.Play();
        yield return new WaitForSeconds(playSource.clip.length + 2);
        SceneManager.LoadScene(sceneName: "MenuScene");
    }


}
