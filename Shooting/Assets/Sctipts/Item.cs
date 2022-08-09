using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string type;
    Rigidbody2D rigid;
    public GameObject BoomEfect;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.down * 1;
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BulletBorder")
        {
            Destroy(gameObject);
        }

    }

}
