using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float maxFlashTime;
    public float curFlashTime;

    public bool isTouchTop;
    public bool isTouchBottom;
    public bool isTouchLeft;
    public bool isTouchRight;

    Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        Move();
    }
    public void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if (( isTouchRight && h == 1)  || (isTouchLeft && h == -1))
            h = 0;
        float v = Input.GetAxisRaw("Vertical");
        if ((isTouchBottom && v == -1) || (isTouchTop && v == 1))
            v = 0;
        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.X) && curFlashTime > maxFlashTime)
        {
            speed = 150;
            curFlashTime = 0;
        }
        else speed = 4;
        transform.position = curPos + nextPos;
        curFlashTime += Time.deltaTime;

        if(Input.GetButtonDown("Horizontal")||
            Input.GetButtonUp("Horizontal"))
            {
            anim.SetInteger("Input", (int)h);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;

            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;

            }
        }
    }
}
