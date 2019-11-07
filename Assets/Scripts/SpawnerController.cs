using Assets.Scripts.Common;
using Assets.Scripts.Units;
using Assets.Scripts.Units.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;

public class SpawnerController : MonoBehaviour
{
    public GameObject firstWaypoint;
    public GameObject[] waves;

    private int wave = 0;

    public void SpawnWave()
    {
        wave += 1;
        for (int i = 0; i < 15; i++)
        {
            var enemyToSpawn = (wave <= waves.Length) ? waves[wave - 1] : waves[waves.Length - 1];
            StartCoroutine(SpawnMobWithDelay(enemyToSpawn, 0.15f * i));
        }
    }

    IEnumerator SpawnMobWithDelay(GameObject enemyToSpawn, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        var spawnPoint = GetARandomTreePos();
        spawnPoint.y += 1;
        var target = firstWaypoint;

        var instance = Instantiate(enemyToSpawn, spawnPoint, Quaternion.identity);
        var container = instance.GetComponent<UnitImplementationContainer>();

        var unitToInitializeWith = Tables.WaveEnemies.ContainsKey(wave) ? Tables.WaveEnemies[wave] : Tables.WaveEnemies[Tables.WaveEnemies.Keys.LastOrDefault()];

        MethodInfo castMethod = typeof(UnitImplementationContainer).GetMethod("InitializeWith").MakeGenericMethod(unitToInitializeWith);
        castMethod.Invoke(container, new object[] { });

        container.Unit.Waypoint = target;
    }

    public Vector3 GetARandomTreePos()
    {
        var original = this.transform.position;

        original.x += Random.Range(-10f, 10);
        original.z += Random.Range(-4f, 4);

        return original;
    }
}
