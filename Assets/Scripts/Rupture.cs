using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rupture : Resource
{
    public bool         spawnEnemies = false;
    [ShowIf("spawnEnemies")]
    public int          maxEnemies = 1;
    [ShowIf("spawnEnemies")]
    public float        timeBetweenEnemies = 1.0f;
    [ShowIf("spawnEnemies")]
    public GameObject[] enemyPrefabs;

    float               elapsedTime = 0.0f;
    List<GameObject>    enemies;
    SpriteRenderer      spriteRenderer;
    Color               color;

    override public bool canDump
    {
        get
        {
            return (resourceAmmount < 1.0f);
        }
    }

    void Start()
    {
        enemies = new List<GameObject>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        color = spriteRenderer.color;
    }

    void Update()
    {
        enemies.RemoveAll((e) => e == null);

        elapsedTime += Time.deltaTime;
        if (elapsedTime > timeBetweenEnemies)
        {
            elapsedTime -= timeBetweenEnemies;

            if (enemies.Count < maxEnemies)
            {
                SpawnEnemy();
            }
        }
    }

    void SpawnEnemy()
    {
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        GameObject newEnemy = Instantiate(enemyPrefab, transform.position - Vector3.forward * Random.Range(0.1f, 0.2f), transform.rotation);

        enemies.Add(newEnemy);
    }


    public override void Drain(float r)
    {
        resourceAmmount = Mathf.Clamp(resourceAmmount - r, 0.0f, 1.0f);

        spriteRenderer.color = Color.Lerp(color.ChangeAlpha(0), color, resourceAmmount);
    }
}
