using Assets.Scripts.Building;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject enemyToSpawn;
    public GameObject[] spawnRegions;

    private int enemiesAlive = 0;
    private int waveNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnWave());

        Assets.Scripts.EventHandler.Current.SubscribeToEvent(Assets.Scripts.EventHandler.Events.Wave_Finished, () =>
        {
            StartCoroutine(SpawnWave());
        });

        Assets.Scripts.EventHandler.Current.SubscribeToEvent(Assets.Scripts.EventHandler.Events.Wave_SpawnNew, () =>
        {
            Spawn();
        });

        Assets.Scripts.EventHandler.Current.SubscribeToEvent(Assets.Scripts.EventHandler.Events.Wave_EnemyDied, () =>
        {
            enemiesAlive -= 1;
            if (enemiesAlive <= 0)
            {
                Assets.Scripts.EventHandler.Current.Dispatch(Assets.Scripts.EventHandler.Events.Wave_Finished);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10000f, LayerMask.GetMask("BuildHandlers")))
            {
                var gameObjectThatWasHit = hit.collider.gameObject;
                var component = gameObjectThatWasHit.GetComponent<BuildSelectorHandler>();

                if (component != null)
                {
                    component.Spawn();
                }
            }
        }
    }

    private IEnumerator SpawnWave()
    {
        yield return new WaitForSeconds(5f);
        Assets.Scripts.EventHandler.Current.Dispatch(Assets.Scripts.EventHandler.Events.Wave_SpawnNew);
    }

    void Spawn()
    {
        waveNumber += 1;
        SetWave(waveNumber);

        enemiesAlive = 15;
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

    internal void SetWave(int number)
    {
        GameObject.FindGameObjectWithTag("UI_Wave").GetComponent<Text>().text = $"Wave: {number}";
    }

}
