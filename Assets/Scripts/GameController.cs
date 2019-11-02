using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject enemyToSpawn;
    public GameObject[] spawnRegions;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemy", 1f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void SpawnEnemy()
    {
        foreach (var item in spawnRegions)
        {
            var controller = item.GetComponent<SpawnerController>();

            controller.Spawn(enemyToSpawn);
        }   
    }

    internal void SetHP(int amount)
    {
        GameObject.FindGameObjectWithTag("UI_HP").GetComponent<Text>().text = $"HP: {amount}";
    }

    internal void SetGold(int amount)
    {
        GameObject.FindGameObjectWithTag("UI_Gold").GetComponent<Text>().text = $"Gold: {amount}";
    }

}
