using Assets.Scripts.Building;
using Assets.Scripts.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI
{
    public abstract class BaseAI : MonoBehaviour
    {
        protected string SearchTag { get; set; }

        protected List<GameObject> PossibleTargets { get; set; } = new List<GameObject>();

        protected bool initialized = false;
        protected Unit unit;
        
        void FixedUpdate()
        {
            if (!initialized)
            {
                initialized = true;
                unit = GetComponent<UnitImplementationContainer>().Unit;
            }

            var orderedTargets = GameObject.FindGameObjectsWithTag(SearchTag);
            var target = FindTarget(orderedTargets, unit.AlarmRange);

            if (target != null)
                unit.IssueOrder(Common.Command.Attack, target);

            unit.ProcessAI();
            PossibleTargets.Clear();

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

        protected abstract GameObject FindTarget(GameObject[] possibleTargets, int range);
    }
}
