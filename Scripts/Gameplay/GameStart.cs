using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    public float[] beatTempo;
    private float currentBeatTempo;

    public TMP_Text preGame;
    public GameObject enemy;
    public TMP_Text scoreText;
    public TMP_Text comboText;
    public AudioSource bgMusic;
    private float audioDuration;
    private bool startPlay = false;

    public GameObject pauseMenu;
    public GameObject gameOver;

    public static GameStart instance;

    public int currentScore;
    public int scorePerNote = 100;
    public int scorePerGreatNote = 125;
    public int scorePerPerfectNote = 150;
    private int combo;

    private float totalNotes;
    private float normalHits;
    private float greatHits;
    private float perfectHits;
    private float missedHits;

    public GameObject enemyParent;
    public GameObject[] enemyPrefabs; // Array to hold different enemy prefabs

    private float lastXPosition = 100f;
    public float spacingOffset = 0.5f;

    public HealthBar healthBar;
    public int maxHealth = 150;
    public int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        scoreText.text = "Score: 0";
        comboText.text = "COMBO: 0";
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        pauseMenu.SetActive(false);
        gameOver.SetActive(false);
        StartCoroutine(ReadyGo());
    }

    IEnumerator ReadyGo()
    {
        preGame.text = "Ready";
        yield return new WaitForSeconds(1f);
        preGame.text = "Go";
        yield return new WaitForSeconds(1f);
        preGame.gameObject.SetActive(false);
        startPlay = true;
        PlayMusic();
    }

    void PlayMusic()
    {
        string audioname = PlayerPrefs.GetString("songAudio");
        var audio_src = "Music/" + audioname;
        var audio = Resources.Load<AudioClip>(audio_src);

        bgMusic.clip = audio;

        audioDuration = bgMusic.clip.length;

        int currentSongIndex = PlayerPrefs.GetInt("SelectedSongIndex", 0); ; // You need to determine the index based on the selected song
        currentBeatTempo = beatTempo[currentSongIndex];
        Debug.Log(currentBeatTempo);

        // Calculate the total number of beats based on the song duration and beat tempo
        int totalBeats = Mathf.RoundToInt(audioDuration * (currentBeatTempo / 320f) * 2);
        float secondsPerBeat = 60f / beatTempo[currentSongIndex];

        // Instantiate random enemy prefabs at random positions
        for (int i = 0; i < totalBeats; i++)
        {
            // Calculate the time at which the beat should occur
            float beatTime = i * secondsPerBeat;

            // Randomly choose an enemy prefab from the array
            GameObject randomEnemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            float enemyWidth = randomEnemyPrefab.GetComponent<Image>().preferredWidth;

            // Set the X position within a specific range (adjust these values as needed)
            // Calculate a random X position that ensures no overlap with previous beats
            float xPos = lastXPosition + spacingOffset + enemyWidth;

            // Decide between -179 and 54.5 for the Y position
            float yPos = Random.Range(0, 2) == 0 ? -179f : 54.5f;

            // Instantiate the enemy prefab at the chosen position
            GameObject enemy = Instantiate(randomEnemyPrefab);
            enemy.transform.SetParent(enemyParent.transform);
            enemy.transform.localPosition = new Vector3(xPos, yPos, 0f);

            // Update the lastXPosition for the next instantiation
            lastXPosition = xPos;
        }

        bgMusic.Play();

        totalNotes = GameObject.FindGameObjectsWithTag("Enemies").Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (startPlay)
        {
            BeatScroller();
            if (bgMusic.time >= audioDuration)
            {
                // Music has ended, do something here (e.g., restart music, stop game, etc.)
                float totalHit = normalHits + greatHits + perfectHits;
                float percentHit = ((normalHits * scorePerNote) + (greatHits * scorePerGreatNote) + (perfectHits * scorePerPerfectNote)) / (totalNotes * scorePerPerfectNote) * 100f;

                string rankVal = "F";
                if (percentHit > 40)
                {
                    rankVal = "D";
                    if (percentHit > 55)
                    {
                        rankVal = "C";
                        if (percentHit > 70)
                        {
                            rankVal = "B";
                            if (percentHit > 85)
                            {
                                rankVal = "A";
                                if (percentHit > 95)
                                {
                                    rankVal = "S";
                                }
                            }
                        }
                    }
                }

                PlayerPrefs.SetString("normalHits", normalHits.ToString());
                PlayerPrefs.SetString("greatHits", greatHits.ToString());
                PlayerPrefs.SetString("perfectHits", perfectHits.ToString());
                PlayerPrefs.SetString("missedHits", missedHits.ToString());
                PlayerPrefs.SetString("percentHit", percentHit.ToString("F2"));
                PlayerPrefs.SetString("rankValue", rankVal);
                PlayerPrefs.SetString("finalScore", currentScore.ToString());
                SceneManager.LoadScene("PlayerResult");
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseGame();
        }

        if (currentHealth <= 0)
        {
            ToggleGameOver();
        }
    }

    public void ToggleGameOver()
    {
        // Check if the pause menu is active or not
        bool isOver = gameOver.activeSelf;

        // Toggle the pause menu and pause/unpause the game accordingly
        if (!isOver)
        {
            GameOver();
        }
    }

    public void TogglePauseGame()
    {
        // Check if the pause menu is active or not
        bool isPaused = pauseMenu.activeSelf;

        // Toggle the pause menu and pause/unpause the game accordingly
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOver.SetActive(true);
        bgMusic.Pause();
    }

    public void PauseGame()
    { 
        // Show the pause menu and pause the game
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        bgMusic.Pause();
    }

    public void ResumeGame()
    {
        // Hide the pause menu and resume the game
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        bgMusic.Play();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        print("Scene restarted");
        Time.timeScale = 1f;
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("Lobby");
    }

    void BeatScroller()
    {
        enemy.transform.position -= new Vector3(currentBeatTempo * 3 * Time.deltaTime, 0f, 0f);
    }

    public void NoteHit()
    {
        Debug.Log("Hit!");

        scoreText.text = "Score: "+currentScore;
        comboText.text = "COMBO " + combo + "X";
    }

    public void NormalHit()
    {
        Debug.Log("Normal Hit!");
        currentScore += scorePerNote;
        combo++;
        NoteHit();

        normalHits++;
    }

    public void GreatHit()
    {
        Debug.Log("Great Hit!");
        currentScore += scorePerGreatNote;
        combo++;
        NoteHit();

        greatHits++;
    }

    public void PerfectHit()
    {
        Debug.Log("Perfect Hit!");
        currentScore += scorePerPerfectNote;
        combo++;
        NoteHit();

        perfectHits++;
    }

    public void NoteMissed()
    {
        Debug.Log("Missed!");
        combo = 0;
        comboText.text = "COMBO " + combo + "X";

        missedHits++;

        currentHealth -= 10;
        healthBar.SetHealth(currentHealth);
    }
}
