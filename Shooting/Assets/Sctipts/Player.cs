using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float maxFlashTime;
    public float curFlashTime;
    public float maxShootingTime;
    public float curShootingTime;
    public float power;
    public bool godMode;
    public int blink;

    public bool isTouchTop;
    public bool isTouchBottom;
    public bool isTouchLeft;
    public bool isTouchRight;

    public GameObject bulletA;
    public GameObject bulletB;
    SpriteRenderer spriteRenderer;
    Animator anim;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        Move();
        Fire();
        Reload();
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
            speed = blink;
            curFlashTime = 0;
            GodMode();
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

    void GodMode()
    {
        godMode = true;
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        Invoke("ReturnGodMode",0.5f);
        
    }
    void ReturnGodMode()
    {
        godMode = false;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
     void Fire()
    {
        if (!Input.GetKey(KeyCode.Z)) return;
        if (curShootingTime < maxShootingTime) return;

        switch (power)
        {
            case 1:
             GameObject bullet = Instantiate(bulletA, transform.position, transform.rotation);
             Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
             rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
             break;
            case 2:
                GameObject bulletR = Instantiate(bulletA, transform.position+Vector3.right*0.1f, transform.rotation);
                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                rigidR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                GameObject bulletL = Instantiate(bulletA, transform.position+ Vector3.left * 0.1f, transform.rotation);
                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                rigidL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 3:
                GameObject bulletPL = Instantiate(bulletA, transform.position + Vector3.right * 0.2f, transform.rotation);
                Rigidbody2D rigidPL = bulletPL.GetComponent<Rigidbody2D>();
                rigidPL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                GameObject bulletPR = Instantiate(bulletA, transform.position + Vector3.left * 0.2f, transform.rotation);
                Rigidbody2D rigidPR = bulletPR.GetComponent<Rigidbody2D>();
                rigidPR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                GameObject bulletP = Instantiate(bulletB, transform.position, transform.rotation);
                Rigidbody2D rigidP = bulletP.GetComponent<Rigidbody2D>();
                rigidP.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
        }
        //power one
         
        curShootingTime = 0;
    }
     void Reload()
    {
        curShootingTime += Time.deltaTime;
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
        else if(collision.gameObject.tag == "EnemyBullet"&&godMode == false)
        {
            Destroy(gameObject);
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
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy"&&godMode ==false)
        {
            Destroy(gameObject);
        }
    }


}
