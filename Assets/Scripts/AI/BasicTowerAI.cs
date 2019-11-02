using Assets.Scripts.AI;
using Assets.Scripts.Common;
using Assets.Scripts.Units;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BasicTowerAI : BaseAI
{
    public BasicTowerAI()
    {
        SearchTag = "Unit";
    }

    protected override GameObject FindTarget(GameObject[] possibleTargets, int range)
    {
        GameObject result = null;
        var targetsInRange = new List<KeyValuePair<float, GameObject>>();

        foreach (var target in possibleTargets)
        {
            var distance = Vector3.Distance(unit.gameObject.transform.position,
                         target.gameObject.transform.position);

            if (distance <= range)
            {
                targetsInRange.Add(new KeyValuePair<float, GameObject>(distance, target.gameObject));
            }
        }

        if (targetsInRange.Any())
            result = targetsInRange.OrderBy(x => x.Key).FirstOrDefault().Value;

        return result;
    }
}
