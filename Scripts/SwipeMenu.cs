using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwipeMenu : MonoBehaviour
{
    public GameObject scrollbar;
    public GameObject swipeMenuButtonPrefab;
    public int totalPrefab;

    private GameObject[] instantiatedButtons;
    private Scrollbar scrollBarComponent; // Reference to the Scrollbar component
    private float[] pos;
    private float scroll_pos = 0;
    private float distanceBetweenItems;

    private int currentFocusedIndex = -1;
    private GameObject currentFocusedButton;

    private string[] songAbout;
    private string[] songWriter;
    private string[] songSinger;

    void Start()
    {
        songAbout = new string[totalPrefab];
        songWriter = new string[totalPrefab];
        songSinger = new string[totalPrefab];

        scrollBarComponent = scrollbar.GetComponent<Scrollbar>();
        distanceBetweenItems = 1f / (totalPrefab - 1f);

        pos = new float[totalPrefab];
        for (int i = 0; i < totalPrefab; i++)
        {
            pos[i] = distanceBetweenItems * i;
        }

        instantiatedButtons = new GameObject[totalPrefab];

        // Instantiate swipe menu buttons based on the totalPrefab count
        for (int i = 0; i < totalPrefab; i++)
        {
            instantiatedButtons[i] = Instantiate(swipeMenuButtonPrefab, transform);
            // Get the SwipeMenuButtonData for this button
            SwipeMenuButtonData buttonData = instantiatedButtons[i].GetComponent<SwipeMenuButtonData>();

            // Set the label text for the button (assuming you have a TMP_Text component for the label)
            TMP_Text labelText = instantiatedButtons[i].GetComponentInChildren<TMP_Text>();
            if (labelText != null && buttonData != null)
            {
                labelText.text = buttonData.songName[i];
            }

            songAbout[i] = buttonData.songAbout[i];
            songWriter[i] = buttonData.songWriter[i];
            songSinger[i] = buttonData.songSinger[i];

            // Assign the audio clip to the button's AudioSource component
            AudioSource audioSource = instantiatedButtons[i].GetComponentInChildren<AudioSource>();
            if (audioSource != null && buttonData != null && i < buttonData.audioClip.Length)
            {
                audioSource.clip = buttonData.audioClip[i];
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            scroll_pos = scrollBarComponent.value;
        }
        else
        {
            // Handle keyboard arrow key scroll input
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                scroll_pos -= distanceBetweenItems;
                scroll_pos = Mathf.Clamp01(scroll_pos);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                scroll_pos += distanceBetweenItems;
                scroll_pos = Mathf.Clamp01(scroll_pos);
            }

            // Disable all buttons first (except for the currently focused one)
            for (int j = 0; j < totalPrefab; j++)
            {
                if (instantiatedButtons[j] != currentFocusedButton)
                {
                    instantiatedButtons[j].GetComponent<Button>().interactable = false;
                }
            }

            for (int i = 0; i < totalPrefab; i++)
            {
                if (scroll_pos < pos[i] + (distanceBetweenItems / 2) && scroll_pos > pos[i] - (distanceBetweenItems / 2))
                {
                    scrollBarComponent.value = Mathf.Lerp(scrollBarComponent.value, pos[i], 0.1f);

                    // Set the scale of the focused button to 1 and others to 0.8
                    for (int j = 0; j < totalPrefab; j++)
                    {
                        float scale = (i == j) ? 1f : 0.8f;
                        instantiatedButtons[j].transform.localScale = new Vector2(scale, scale);
                    }

                    // Play audio when a button is focused
                    if (currentFocusedIndex != i)
                    {
                        // A new button has come into focus, stop the audio for the previously focused button
                        if (currentFocusedIndex >= 0)
                        {
                            AudioSource prevAudioSource = instantiatedButtons[currentFocusedIndex].GetComponentInChildren<AudioSource>();
                            if (prevAudioSource != null && prevAudioSource.isPlaying)
                            {
                                prevAudioSource.Stop();
                            }
                        }

                        // Play audio for the currently focused button
                        AudioSource audioSource = instantiatedButtons[i].GetComponentInChildren<AudioSource>();
                        if (audioSource != null && audioSource.clip != null)
                        {
                            audioSource.Play();
                        }

                        string labelText = instantiatedButtons[i].GetComponentInChildren<TMP_Text>().text;
                        PlayerPrefs.SetString("songName", labelText);

                        PlayerPrefs.SetString("songAbout", songAbout[i]);
                        PlayerPrefs.SetString("songWriter", songWriter[i]);
                        PlayerPrefs.SetString("songSinger", songSinger[i]);
                        PlayerPrefs.SetInt("SelectedSongIndex", i);

                        string audioText = instantiatedButtons[i].GetComponentInChildren<AudioSource>().clip.name;
                        PlayerPrefs.SetString("songAudio", audioText);

                        currentFocusedIndex = i; // Update the currently focused index
                    }

                    // Enable the currently focused button
                    currentFocusedButton = instantiatedButtons[i];
                    currentFocusedButton.GetComponent<Button>().interactable = true;
                }
            }
        }
    }
}
