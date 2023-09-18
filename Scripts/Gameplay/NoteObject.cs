using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteObject : MonoBehaviour
{
    public bool canBePressed;
    private float holdStartTime;
    private bool isHolding;
    public float holdThreshold = 4f;
    public Color holdingColor;

    public KeyCode[] keyToPress;

    public GameObject hitEffect, greatEffect, perfectEffect, missedEffect;
    GameObject text;

    private GameObject parent;
    private Image img;

    // Start is called before the first frame update
    void Start()
    {
        parent = GameObject.FindWithTag("PopUp");
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress[0]) || Input.GetKeyDown(keyToPress[1]))
        {
            if (canBePressed)
            {
                if (gameObject.CompareTag("Enemies"))
                {
                    gameObject.SetActive(false);

                    if (Mathf.Abs(transform.position.x) > 500)
                    {
                        GameStart.instance.NormalHit();
                        text = Instantiate(hitEffect);
                        text.transform.SetParent(parent.transform);
                        text.transform.localPosition = hitEffect.transform.localPosition;

                        Destroy(text, 1f);

                        Debug.Log("normal hit: " + transform.position.x);
                    }
                    else if (Mathf.Abs(transform.position.x) > 495)
                    {
                        GameStart.instance.GreatHit();
                        text = Instantiate(greatEffect);
                        text.transform.SetParent(parent.transform);
                        text.transform.localPosition = greatEffect.transform.localPosition;

                        Destroy(text, 1f);

                        Debug.Log("great hit: " + transform.position.x);
                    }
                    else if (Mathf.Abs(transform.position.x) > 490)
                    {
                        GameStart.instance.PerfectHit();
                        text = Instantiate(perfectEffect);
                        text.transform.SetParent(parent.transform);
                        text.transform.localPosition = perfectEffect.transform.localPosition;

                        Destroy(text, 1f);

                        Debug.Log("perfect hit: " + transform.position.x);
                    }
                }

                if (gameObject.CompareTag("LongEnemies"))
                {
                    // Start holding the long note
                    isHolding = true;
                    holdStartTime = Time.time;
                    img.color = holdingColor;
                    Debug.Log("Hold started for long note!");
                }
            }
        }

    // Check if the key is being held down for long notes
    if (Input.GetKey(keyToPress[0]) || Input.GetKey(keyToPress[1]))
    {
        if (isHolding && gameObject.CompareTag("LongEnemies"))
        {
            // Calculate the hold duration
            float holdDuration = Time.time - holdStartTime;
                Debug.Log(holdDuration);

            // Check if the hold duration has reached the desired threshold for the successful hold action
            if (holdDuration * 1.5 >= holdThreshold)
            {
                // Successful hold action for the long note
                Debug.Log("Successful hold action for long note!");
                // ... Your long note hold action code goes here
                GameStart.instance.PerfectHit();
                text = Instantiate(perfectEffect);
                text.transform.SetParent(parent.transform);
                text.transform.localPosition = perfectEffect.transform.localPosition;

                Destroy(text, 1f);
                gameObject.SetActive(false);

                // Reset the isHolding flag to prevent repeating the action
                isHolding = false;
            }
            else
            {
                // Update the UI or visual indicator to show that the key is being held for the long note
                Debug.Log("Holding the long note...");
                // ... Your code to indicate the long note is being held goes here
            }
        }
    }

    if (Input.GetKeyUp(keyToPress[0]) || Input.GetKeyUp(keyToPress[1]))
    {
        if (isHolding && gameObject.CompareTag("LongEnemies"))
        {
            // Note release event for long notes
            // Handle the release logic here, if needed
            // For example, check if the hold duration was successful for a "Hold and Release" mechanic.
            isHolding = false;
        }
    }
}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Activator")
        {
            canBePressed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Activator" && gameObject.activeSelf)
        {
            canBePressed = false;
            GameStart.instance.NoteMissed();
            text = Instantiate(missedEffect);
            text.transform.SetParent(parent.transform);
            text.transform.localPosition = missedEffect.transform.localPosition;

            Destroy(text, 1f);
        }
    }
}
