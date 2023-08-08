using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseSong : MonoBehaviour
{
    private void Start()
    {
        // Add an onClick event to the button to handle the click event
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnClickButton);
        }
    }

    private void OnClickButton()
    {
        // Call the DetailBeatmap's SetSongDetail method with the song name as the parameter
        DetailBeatmap.Instance.GoToDetail();
    }
}
