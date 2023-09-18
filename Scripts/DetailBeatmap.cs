using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DetailBeatmap : MonoBehaviour
{
    public GameObject chooseSong;
    public GameObject songDetail;
    public Image songDisc;
    public TMP_Text songTitle;
    public TMP_Text songAbout;
    public TMP_Text songWriter;
    public TMP_Text songSinger;
    public bool isDetail;

    private CanvasGroup canvasGroup;

    public static DetailBeatmap Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        songDetail.SetActive(false);
        isDetail = false;
        canvasGroup = chooseSong.GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            songDetail.SetActive(false);
            chooseSong.SetActive(true);
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            isDetail = false;
        }
    }

    public void GoToDetail()
    {
        songDetail.SetActive(true);
        isDetail = true;
        // Disable the panel (make it non-interactable)
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        string imgName = "Image/Lobby/"+PlayerPrefs.GetString("songDisc");
        var img = Resources.Load<Sprite>(imgName);
        songDisc.sprite = img;
        songTitle.text = PlayerPrefs.GetString("songName");
        songAbout.text = PlayerPrefs.GetString("songAbout");
        songWriter.text = "Penulis: " + PlayerPrefs.GetString("songWriter");
        songSinger.text = "Penyanyi: " + PlayerPrefs.GetString("songSinger");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("PlayGame");
    }

    public void Back()
    {
        SceneManager.LoadScene("StartGame");
    }
}
