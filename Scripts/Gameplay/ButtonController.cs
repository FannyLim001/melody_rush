using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    private Image theImg;
    public Sprite defaultImg;
    public Sprite pressedImg;
    public GameObject playerAnimate;
    private Animator animator;

    public KeyCode keyToPress;
    // Start is called before the first frame update
    void Start()
    {
        theImg = GetComponent<Image>();
        animator = playerAnimate.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            theImg.sprite = pressedImg;
            animator.SetBool("isHit", true);
        }

        if (Input.GetKeyUp(keyToPress))
        {
            theImg.sprite = defaultImg;
            animator.SetBool("isHit", false);
        }
    }
}
