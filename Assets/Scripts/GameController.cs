using Assets.Scripts.Building;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject[] spawnRegions;

    private int enemiesAlive = 0;
    private int waveNumber = 0;
    private bool waveInProgress = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitiateWaveStart());

        Assets.Scripts.EventHandler.Current.SubscribeToEvent(Assets.Scripts.EventHandler.Events.Wave_Finished, () =>
        {
            waveInProgress = false;
            StartCoroutine(InitiateWaveStart());
        });

        Assets.Scripts.EventHandler.Current.SubscribeToEvent(Assets.Scripts.EventHandler.Events.Wave_SpawnNew, () =>
        {
            waveInProgress = true;
            SpawnWave();
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

    private void ClearSpawnedTowers()
    {
        var buildSpots = GameObject.FindGameObjectsWithTag("BuildPosition");

        foreach (var spot in buildSpots)
        {
            var handler = spot.GetComponent<BuildSelectorHandler>();

            if (handler.spawnedTower != null)
            {
                handler.spawnedTower.SetActive(false);
                Destroy(handler.spawnedTower);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!waveInProgress && Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10000f, LayerMask.GetMask("BuildHandlers")))
            {
                var gameObjectThatWasHit = hit.collider.gameObject;
                var component = gameObjectThatWasHit.GetComponent<BuildSelectorHandler>();

                if (component != null)
                {
                    component.Build();
                }
            }
        }
    }

    private IEnumerator InitiateWaveStart()
    {
        ClearSpawnedTowers();
        for (var countdown = 10; countdown >= 0; countdown--)
        {
            PrepareForNewWave(countdown);
            yield return new WaitForSeconds(1f);
        }
        SpawnTowers();
        Assets.Scripts.EventHandler.Current.Dispatch(Assets.Scripts.EventHandler.Events.Wave_SpawnNew);
    }

    private void SpawnTowers()
    {
        var buildSpots = GameObject.FindGameObjectsWithTag("BuildPosition");

        foreach (var spot in buildSpots)
        {
            var handler = spot.GetComponent<BuildSelectorHandler>();

            if (handler.buildedTower != null)
            {
                handler.Spawn();
            }
        }
    }

    void SpawnWave()
    {
        waveNumber += 1;
        SetWave(waveNumber);

        enemiesAlive = 15;
        foreach (var item in spawnRegions)
        {
            var controller = item.GetComponent<SpawnerController>();

            controller.SpawnWave();
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
        GameObject.FindGameObjectWithTag("UI_Wave").GetComponent<Text>().text = $"Wave #{number}";
    }

    internal void PrepareForNewWave(int number)
    {
        GameObject.FindGameObjectWithTag("UI_Wave").GetComponent<Text>().text = $"Next wave in {number}";
    }
}
