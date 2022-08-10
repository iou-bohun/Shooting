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
    public int power;
    public int boom;
    public bool godMode;
    public int score;
    public int life;
    public int blink;
    public int maxPower;
    public int maxBoom;
    public int cBoom;
    

    public bool isBoomTime;
    public bool isTouchTop;
    public bool isTouchBottom;
    public bool isTouchLeft;
    public bool isTouchRight;
    public bool isHit;

    public GameObject BoomEfect;
    public GameObject bulletA;
    public GameObject bulletB;
    SpriteRenderer spriteRenderer;
    Animator anim;
    public GameManager manager;


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
        Boom();
    }

    //플레이어 이동/////////////////////////////
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
    /////////////////////////////////////////////

    // 플레이어 X키 무적///////////////////////////
    public void GodMode()
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
    //////////////////////////////////////////////////////
    
    // 플레이어 스폰 무적
    public void SponGodMode()
    {
        godMode = true;
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        Invoke("ReturnGodMode", 1f);
    }
    void SponReturnGodMode()
    {
        godMode = false;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
    ///////////////////////////////////////
    void Fire()
    {
        if (!Input.GetKey(KeyCode.Z)) return;
        if (curShootingTime < maxShootingTime) return;

        switch (power)
        {
            case 1:
             GameObject bullet = Instantiate(bulletA, transform.position+Vector3.forward*0.2f, transform.rotation);
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
       
         
        curShootingTime = 0;
    }

    void Boom()
    {
        if (!Input.GetKeyDown(KeyCode.C))
            return;
        if (isBoomTime)
            return;
        if (boom == 0)
            return;
        boom--;
        isBoomTime = true;
       manager.UpdateBoomIcon(boom);
        BoomEfect.SetActive(true);
        Invoke("BoomOff", 2f);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for(int index = 0; index< enemies.Length; index++)
        {
            Enemy enemyLogic = enemies[index].GetComponent<Enemy>();
            enemyLogic.OnHit(1000);
        }
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        for (int index = 0; index < bullets.Length; index++)
        {
            Destroy(bullets[index]);
        }
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
            if (isHit) return;
            isHit = true;
            life -=1;
            manager.UpdateLifeIcon(life);
            if(life == 0)
            {
               manager.GameOver();
            }
            else
            {
                manager.RespawnPlayer();
            }
            gameObject.SetActive(false);
            Destroy(collision.gameObject);
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
            life -= 1;
            manager.UpdateLifeIcon(life);
            if (life == 0)
            {
               manager.GameOver();
            }
            else
            {
                manager.RespawnPlayer();
            }
            gameObject.SetActive(false);
            Destroy(collision.gameObject);

        }
        else if(collision.gameObject.tag == "Item")
        {
            Item item = collision.gameObject.GetComponent<Item>();
            switch (item.type)
            {
                case "Boom":
                    if (boom == maxBoom)
                    {
                        score += 500;
                    }
                    else boom++;
                   manager.UpdateBoomIcon(boom);
                    Destroy(collision.gameObject);
                    break;
                case "Coin":
                    score += 1000;
                    Destroy(collision.gameObject);
                    
                    break;
                case "Power":
                    if(power == maxPower)
                    {
                        score += 500;
                    }
                    else power++;
                    Destroy(collision.gameObject);
                    break;

            }
        }
    }
    void BoomOff()
    {
        BoomEfect.SetActive(false);
        isBoomTime = false;
    }


}
