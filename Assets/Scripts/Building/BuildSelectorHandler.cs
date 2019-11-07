using Assets.Scripts.Common;
using Assets.Scripts.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Building
{
    public class BuildSelectorHandler : MonoBehaviour
    {
        public GameObject spawnedTower;
        public GameObject buildedTower;

        public Material AvailibleMaterial;
        public Material OccupiedMaterial;

        public GameObject towerToSpawn;
        public Type typeOfTower;

        public GameObject meleeTower;
        public GameObject rangedTower;

        private Type type;
        private bool isVisible = true;
        private new MeshRenderer renderer;
        private GameObject towerDummy;

        private void Awake()
        {
            renderer = GetComponent<MeshRenderer>();

            EventHandler.Current.SubscribeToEvent(EventHandler.Events.ChooseTower_Ranged, () => {
                towerToSpawn = rangedTower;
                type = typeof(TowerRanged);
            });
            EventHandler.Current.SubscribeToEvent(EventHandler.Events.ChooseTower_Melee, () => {
                towerToSpawn = meleeTower;
                type = typeof(Tower);
            });

            EventHandler.Current.SubscribeToEvent(EventHandler.Events.OnBuildMenuToggle, () => ToggleVisibility());

            EventHandler.Current.SubscribeToEvent(EventHandler.Events.Wave_Finished, ToggleDummy);
            EventHandler.Current.SubscribeToEvent(EventHandler.Events.Wave_SpawnNew, ToggleDummy);

            ToggleVisibility();
        }

        private void ToggleDummy()
        {
            if (buildedTower != null)
            {
                var spawnPos = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);

                if (towerDummy == null)
                {
                    towerDummy = Instantiate(buildedTower, spawnPos, Quaternion.identity);
                }
                else
                {
                    Destroy(towerDummy);
                }
            }
        }

        private void ToggleVisibility()
        {
            var currentColor = renderer.material.color;

            if (!isVisible)
            {
                renderer.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, 0.3f);
            }
            else
            {
                renderer.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, 0f);
            }

            isVisible = !isVisible;
        }

        private void FixedUpdate()
        {
        }

        public void Build()
        {
            if (!isVisible)
                return;

            var price = Tables.TowerPrices[type];

            if (type == null)
                return;

            if (Player.Current.Gold < price)
                return;

            if (buildedTower != null)
                return;

            typeOfTower = type;
            buildedTower = towerToSpawn;
            Player.Current.Gold -= price;
            GetComponent<MeshRenderer>().material = OccupiedMaterial;

            ToggleDummy();
        }

        public void Spawn()
        {
            var spawnPos = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);

            spawnedTower = Instantiate(buildedTower, spawnPos, Quaternion.identity);
            var container = spawnedTower.GetComponent<UnitImplementationContainer>();

            MethodInfo castMethod = typeof(UnitImplementationContainer).GetMethod("InitializeWith").MakeGenericMethod(typeOfTower);
            castMethod.Invoke(container, new object[] { });
        }
    }
}
