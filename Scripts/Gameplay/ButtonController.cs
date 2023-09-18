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
    private Rigidbody2D rigidbody2d;

    public KeyCode keyToPress;
    // Start is called before the first frame update
    void Start()
    {
        theImg = GetComponent<Image>();
        animator = playerAnimate.GetComponent<Animator>();
        rigidbody2d = playerAnimate.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            theImg.sprite = pressedImg;
            animator.SetBool("isHit", true);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            float jumpVelocity = 50f;
            rigidbody2d.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
        }

        if (Input.GetKeyUp(keyToPress))
        {
            theImg.sprite = defaultImg;
            animator.SetBool("isHit", false);
        }
    }
}
