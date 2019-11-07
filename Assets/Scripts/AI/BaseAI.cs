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

            if (unit == null)
                return;

            var orderedTargets = GameObject.FindGameObjectsWithTag(SearchTag);
            var target = FindTarget(orderedTargets, unit.AlarmRange);

            if (target != null)
                unit.IssueOrder(Common.Command.Attack, target);

            unit.ProcessAI();
            PossibleTargets.Clear();
        }

        protected abstract GameObject FindTarget(GameObject[] possibleTargets, int range);
    }
}
