using Assets.Scripts.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public GameObject firstWaypoint;

    public void Spawn(GameObject enemyToSpawn)
    {
        for (int i = 0; i < 15; i++)
        {
            var spawnPoint = GetARandomTreePos();
            spawnPoint.y += 1;
            var target = firstWaypoint;

            var instance = Instantiate(enemyToSpawn, spawnPoint, Quaternion.identity);
            var container = instance.GetComponent<UnitImplementationContainer>();
            container.InitializeWith<UnitBase>();

            container.Unit.Waypoint = target;
        }
        
    }

    public Vector3 GetARandomTreePos()
    {
        var original = this.transform.position;

        original.x += Random.Range(-10f, 10);
        original.z += Random.Range(-4f, 4);

        return original;
    }
}
