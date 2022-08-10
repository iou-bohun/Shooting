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
    public ObjectMAnager objManager;


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
                GameObject bullet = objManager.MakeObj("BulletPlayerA");
                bullet.transform.position = transform.position;
             Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
             rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
             break;
            case 2:
                GameObject bulletR = objManager.MakeObj("BulletPlayerA");
                bulletR.transform.position = transform.position +Vector3.right*0.1f;
                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                rigidR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                GameObject bulletL = objManager.MakeObj("BulletPlayerA");
                bulletL.transform.position = transform.position + Vector3.left * 0.1f;
                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                rigidL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 3:
                GameObject bulletPL = objManager.MakeObj("BulletPlayerA");
                bulletPL.transform.position = transform.position + Vector3.left * 0.2f;
                Rigidbody2D rigidPL = bulletPL.GetComponent<Rigidbody2D>();
                rigidPL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                GameObject bulletPR = objManager.MakeObj("BulletPlayerA");
                bulletPR.transform.position = transform.position + Vector3.right * 0.2f;
                Rigidbody2D rigidPR = bulletPR.GetComponent<Rigidbody2D>();
                rigidPR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                GameObject bulletP = objManager.MakeObj("BulletPlayerB");
                bulletP.transform.position = transform.position;
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

        GameObject[] enemiesL = objManager.GetPool("EnemyL");
        GameObject[] enemiesM = objManager.GetPool("EnemyM");
        GameObject[] enemiesS = objManager.GetPool("EnemyS");
        for (int index = 0; index< enemiesL.Length; index++)
        {
            if (enemiesL[index].activeSelf)
            {
                Enemy enemyLogic = enemiesL[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }
        for (int index = 0; index < enemiesM.Length; index++)
        {
            if (enemiesM[index].activeSelf)
            {
                Enemy enemyLogic = enemiesM[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }
        for (int index = 0; index < enemiesS.Length; index++)
        {
            if (enemiesS[index].activeSelf)
            {
                Enemy enemyLogic = enemiesS[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }
        GameObject[] bulletsA = objManager.GetPool("BulletEnemyA");
        GameObject[] bulletsB = objManager.GetPool("BulletEnemyB");
        for (int index = 0; index < bulletsA.Length; index++)
        {
            if (bulletsA[index].activeSelf)
            {
                bulletsA[index].SetActive(false);
            }
        }
        for (int index = 0; index < bulletsB.Length; index++)
        {
            if (bulletsB[index].activeSelf)
            {
                bulletsB[index].SetActive(false);
            }
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
            collision.gameObject.SetActive(false);
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
            collision.gameObject.SetActive(false);

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
                    collision.gameObject.SetActive(false);
                    break;
                case "Coin":
                    score += 1000;
                    collision.gameObject.SetActive(false);

                    break;
                case "Power":
                    if(power == maxPower)
                    {
                        score += 500;
                    }
                    else power++;
                    collision.gameObject.SetActive(false);
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
