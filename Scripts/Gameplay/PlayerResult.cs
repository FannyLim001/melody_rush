using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerResult : MonoBehaviour
{
    public TMP_Text accuracy, score, perfectHit, greatHit, normalHit, missedHit, Rank;
    // Start is called before the first frame update
    void Start()
    {
        normalHit.text = PlayerPrefs.GetString("normalHits");
        greatHit.text = PlayerPrefs.GetString("greatHits");
        perfectHit.text = PlayerPrefs.GetString("perfectHits");
        missedHit.text = PlayerPrefs.GetString("missedHits");
        accuracy.text = PlayerPrefs.GetString("percentHit");
        Rank.text = PlayerPrefs.GetString("rankValue");
        score.text = PlayerPrefs.GetString("finalScore");
    }

    public void Back()
    {
        SceneManager.LoadScene("Lobby");
    }
}
