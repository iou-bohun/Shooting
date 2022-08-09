using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public int health;
    public Sprite[] sprites;
    public string enemyName;
    public int score;
    public bool isdead;

    public float maxShootingTime;
    public float curShootingTime;
    public GameObject player;
    public GameObject itemPower;
    public GameObject itemBoom;
    public GameObject itemCoin;


    public GameObject bulletA;
    public GameObject bulletB;

    SpriteRenderer spriterenderer;
    

    private void Awake()
    {
        spriterenderer = GetComponent<SpriteRenderer>();
        
    }
    private void Update()
    {
        Fire();
        Reload();
    }

    public void OnHit(int dmg)
    {
        health -= dmg;
        spriterenderer.sprite = sprites[1];
        Invoke("ReturnSprite",0.1f);

        if (health <= 0)
        {
            if (isdead) return;
            isdead = true;
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += score;

            //아이템 드랍
            int ran = Random.Range(0,10);
            if (ran < 3) Debug.Log("Nothing");
            else if (ran < 5)
            {
                Instantiate(itemCoin, transform.position, itemCoin.transform.rotation);
            }
            else if (ran < 8)
            {
                Instantiate(itemPower, transform.position, itemPower.transform.rotation);
            }
            else if (ran < 10)
            {
                Instantiate(itemBoom, transform.position, itemBoom.transform.rotation);
            }
            Destroy(gameObject);
        }

    }
    void ReturnSprite()
    {
        spriterenderer.sprite = sprites[0];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "EnemyBorder")
        {
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.dmg);
            Destroy(collision.gameObject);
        }
    }
    void Fire()
    {
        if (curShootingTime < maxShootingTime) return;
        if(enemyName == "S")
        {
            GameObject bullet = Instantiate(bulletA, transform.position,transform.rotation);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            rigid.AddForce(Vector2.down * 10, ForceMode2D.Impulse);
        }
        else if(enemyName == "L")
        {
            GameObject bulletR = Instantiate(bulletB, transform.position + Vector3.right * 0.4f, transform.rotation);
            GameObject bulletL = Instantiate(bulletB, transform.position + Vector3.left * 0.4f,transform.rotation);
            Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
            Vector3 dirvec = player.transform.position - transform.position;
            rigidR.AddForce(dirvec.normalized*5, ForceMode2D.Impulse);
            rigidL.AddForce(dirvec.normalized*5, ForceMode2D.Impulse);
        }
        curShootingTime = 0;
    }
    void Reload()
    {
        curShootingTime += Time.deltaTime;
    }
}
