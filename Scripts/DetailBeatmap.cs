using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DetailBeatmap : MonoBehaviour
{
    public GameObject chooseSong;
    public GameObject songDetail;
    public TMP_Text songTitle;
    public TMP_Text songAbout;
    public TMP_Text songWriter;
    public TMP_Text songSinger;

    public static DetailBeatmap Instance { get; private set; }

    private void Awake()
    {
        // Check if an instance already exists
        if (Instance == null)
        {
            // If not, set the instance to this script
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an instance already exists, destroy this duplicate instance
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        songDetail.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            songDetail.SetActive(false);
            chooseSong.SetActive(true);
        }
    }

    public void GoToDetail()
    {
        songDetail.SetActive(true);
        songTitle.text = PlayerPrefs.GetString("songName");
        songAbout.text = PlayerPrefs.GetString("songAbout");
        songWriter.text = "Penulis: " + PlayerPrefs.GetString("songWriter");
        songSinger.text = "Penyanyi: " + PlayerPrefs.GetString("songSinger");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("PlayGame");
    }
}
