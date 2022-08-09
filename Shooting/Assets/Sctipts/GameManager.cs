using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemyObject;
    public Transform[] spawnPoints;
    public float maxSpawnDelay;
    public float curSpawnDelay;

    public GameObject player;
    public Text scoreText;
    public Image[] lifeImage;
    public Image[] boomImage;

    public GameObject gameOver;
    
    void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if (curSpawnDelay > maxSpawnDelay)
        {
            SpawnEnemy();
            maxSpawnDelay = Random.Range(0.5f, 2f);
            curSpawnDelay = 0;
        }
        //#.UI Score Update
        Player playerLogic = player.GetComponent<Player>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score);
    }
    void SpawnEnemy()
    {
        int ranEnemy = Random.Range(0, 3);
        int ranPoint = Random.Range(0, 9);
        GameObject enemy = Instantiate(enemyObject[ranEnemy],
                                          spawnPoints[ranPoint].position,
                                          spawnPoints[ranPoint].rotation);
        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player;
        if (ranPoint == 5 || ranPoint == 6)
        {
            enemy.transform.Rotate(Vector3.back*90);
            rigid.velocity = new Vector2(enemyLogic.speed * (-1.2f), -1);
        }
        else if (ranPoint == 7 || ranPoint == 8)
        {
            enemy.transform.Rotate(Vector3.forward * 90);
            rigid.velocity = new Vector2(enemyLogic.speed*(1.2f), -1);
        }
        else
        {
            rigid.velocity = new Vector2(0, enemyLogic.speed * (-1));
        }
    }
        
    public void RespawnPlayer()
    {
        Invoke("RespawnPlayerExe",2f);
    }
    public void RespawnPlayerExe()
    {
        player.transform.position = Vector3.down * 3.5f;
        player.SetActive(true);
        Player playerLogic = player.GetComponent<Player>();
        playerLogic.isHit = false;
        playerLogic.SponGodMode();

    }
    public void UpdateLifeIcon(int life)
    {
        for (int index = 0; index < 3; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 0);
        }
        for (int index=0; index <life; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 1);
        }
    }
    public void GameOver()
    {
        gameOver.SetActive(true);
    }
    public void GameReTry()
    {
        SceneManager.LoadScene(0);
    }

    public void UpdateBoomIcon(int boom)
    {
        for (int index = 0; index < boomImage.Length; index++)
        {
            boomImage[index].color = new Color(1, 1, 1, 0);
        }
        for (int index = 0; index < boom; index++)
        {
            boomImage[index].color = new Color(1, 1, 1, 1);
        }
    }
}

