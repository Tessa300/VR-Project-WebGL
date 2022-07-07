using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// Controles the energy level, score, colliders, audio, visuals
/// </summary>
public class PlayControl : MonoBehaviour
{
    // Score and energy level
    public TextMeshProUGUI scoreText;
    // Game over text
    public TextMeshProUGUI endText;

    // Audio sources
    public AudioSource backgroundMusic;
    public AudioSource introVoiceSound;
    public AudioSource ringCrossingSound;
    public AudioSource crashPlanetSound;
    public AudioSource crashAsteriodSound;
    public AudioSource energyLowSound;
    public AudioSource energyEmptySound;
    public AudioSource winnerSound;

    // Some parameters for overview and easy access
    private float secondsBetweenEnergyDecreases = 1f;
    private float decreaseEnergyPercentage = 0.03f;
    private float increaseEnergyConst = 30f;
    private float lowEnergyWarnStart = 40f; // max 100%
    private float ringIncreaseSpeedPercentage = 0.5f;

    // used for fade out background music after game end
    private bool musicFadeOutEnabled = false;
    // user has no control over the plane <- no energy
    private bool outOfControl = false;
    // game is running
    private bool running = false;

    // Players score +  energy level
    private int score = 0;
    private float energy = 100f;
    // Last energy decrease in sec from Time.realtimeSinceStartup
    private float lastEnergyDecrease = 0;


    void Start()
    {
        StartCoroutine(E_startSequence());
    }

    
    void Update()
    {
        if (running)
        {
            // Decrease energy
            if (Time.realtimeSinceStartup - lastEnergyDecrease >= secondsBetweenEnergyDecreases)
            {
                energy -= (energy * decreaseEnergyPercentage > 2) ? energy * decreaseEnergyPercentage : 2; // minimum decrease value is 2%
                lastEnergyDecrease = Time.realtimeSinceStartup;
            }

            if (energy <= 0)
            {
                // End game if energy is empty
                energy = 0;
                GameObject.Find("Paperplane").GetComponent<Renderer>().material.color = Color.red;
                outOfControl = true;
                // Stop game
                StartCoroutine(E_stopSequence(energyEmptySound, "Keine Energie mehr"));
            }
            else if (energy <= lowEnergyWarnStart && !energyLowSound.isPlaying)
            {
                // Energy low start (in AudioSource set on loop)
                energyLowSound.Play(); 
            }
            else if (energy > lowEnergyWarnStart && energyLowSound.isPlaying)
            {
                // Energy up after low
                energyLowSound.Stop();
                GameObject.Find("Paperplane").GetComponent<Renderer>().material.color = Color.white;
            }

            if (energy <= lowEnergyWarnStart && energy > 0)
            {
                // Energy low running -> Blink
                GameObject.Find("Paperplane").GetComponent<Renderer>().material.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time, 1));
            }
        }


        // Fade out background music (make quieter step by step) if requestet 
        if (musicFadeOutEnabled)
        {
            float newVolume = backgroundMusic.volume - (0.2f * Time.deltaTime);
            if (newVolume < 0.1f)
                backgroundMusic.Stop();
            else
                backgroundMusic.volume = newVolume;
        }

        // Player is out of control -> turns -> Not used because of Cybersickness
        //if (outOfControl)
        //    transform.Rotate(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360));
    }


    // Player collided with an object
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "finishline" && collider.gameObject.GetComponent<Ring>().IsActive())
        {
            // Collided with final ring
            score++;
            // Stop game
            StartCoroutine(E_stopSequence(winnerSound, "Gewonnen"));
        }
        else if (collider.tag == "ring" && collider.gameObject.GetComponent<Ring>().IsActive())
        {
            // Collided with now active ring
            score++;
            energy = (energy + increaseEnergyConst < 100) ? energy + increaseEnergyConst : 100;
            Destroy(collider.gameObject);
            StartCoroutine(E_playSoundShowText(ringCrossingSound, ringIncreaseSpeedPercentage));
        }
        else if (collider.tag == "planet")
        {
            // Collided with a planet
            score--;
            // Stop game
            StartCoroutine(E_stopSequence(crashPlanetSound, "An Planet zerschellt"));
        }
        else if (collider.tag == "asteroid")
        {
            // Collided with an asteroid
            score--;
            Destroy(collider.gameObject);
            StartCoroutine(E_playSoundShowText(crashAsteriodSound, 0));
        }
        Debug.Log(collider.tag);
    }


    //// IEnumerator definitions used for Coroutine to start one action after another finished
    //// For better overview also not order sensitive instructions were placed here

    /// <summary>
    /// Play sound and show score text while sound is playing
    /// </summary>
    /// <param name="playSource">AudioSource to play</param>
    /// <param name="increaseSpeedPercentage">Percentage value for temporary speed increase</param>
    /// <returns></returns>
    IEnumerator E_playSoundShowText(AudioSource playSource, float increaseSpeedPercentage)
    {
        AcceleratorCable ac = gameObject.GetComponent<AcceleratorCable>();
        playSource.Play();
        scoreText.text = "Score " + score.ToString() + "\n" + Math.Round(energy) + "%";
        if (increaseSpeedPercentage != 0)
            ac.IncreaseSpeed(increaseSpeedPercentage);
        yield return new WaitForSeconds(playSource.clip.length);
        if (increaseSpeedPercentage != 0)
            ac.DecreaseSpeed(increaseSpeedPercentage);
        scoreText.text = "";
    }

    /// <summary>
    /// Start sqeuence with intro voice, 321-counter, starts movement and init params adjustments
    /// </summary>
    /// <returns></returns>
    IEnumerator E_startSequence()
    {
        score = 0;
        PlayerPrefs.SetInt("gameScore", score);
        scoreText.text = "";
        endText.text = "";
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
        AcceleratorCable ac = gameObject.GetComponent<AcceleratorCable>();
        ac.moveAllowed = true;
        lastEnergyDecrease = Time.realtimeSinceStartup;
        running = true;
    }

    /// <summary>
    /// Sequence at the end of the game to stop
    /// Plays end of the game sound, starts the fading out of the background music, 
    /// shows message, stops movement, returns to menu scene
    /// </summary>
    /// <param name="playSource">End of the game sound</param>
    /// <param name="stopMessage"></param>
    /// <returns></returns>
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
