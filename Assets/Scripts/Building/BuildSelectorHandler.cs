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
        public int cooldown = 0;
        public GameObject towerToSpawn;

        public GameObject meleeTower;
        public GameObject rangedTower;

        private Type type;

        private void Awake()
        {
            EventHandler.Current.SubscribeToEvent(EventHandler.Events.ChooseTower_Ranged, () => {
                towerToSpawn = rangedTower;
                type = typeof(TowerRanged);
            });
            EventHandler.Current.SubscribeToEvent(EventHandler.Events.ChooseTower_Melee, () => {
                towerToSpawn = meleeTower;
                type = typeof(Tower);
            });

            EventHandler.Current.SubscribeToEvent(EventHandler.Events.OnBuildMenuToggle, () => gameObject.SetActive(!gameObject.activeSelf));
            gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            cooldown -= 1;
        }

        public void Spawn()
        {
            if (type == null)
                return;

            var price = TowerPriceTable.Prices[type];
            if (Player.Current.Gold < price)
                return;

            if (cooldown > 0)
                return;

            var spawnPos = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);

            var instance = Instantiate(towerToSpawn, spawnPos, Quaternion.identity);
            var container = instance.GetComponent<UnitImplementationContainer>();

            cooldown = 200;
            MethodInfo castMethod = typeof(UnitImplementationContainer).GetMethod("InitializeWith").MakeGenericMethod(type);
            castMethod.Invoke(container, new object[] { });
            Player.Current.Gold -= price;
        }
    }
}
