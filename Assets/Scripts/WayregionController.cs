using Assets.Scripts;
using Assets.Scripts.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayregionController : MonoBehaviour
{
    public bool isFinal;
    public GameObject next;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        var container = other.gameObject.GetComponent<UnitImplementationContainer>();

        if (container == null)
            return;

        if (isFinal)
        {
            Destroy(other.gameObject);

            Player.Current.Health -= 1;
        }

        if (next != null)
        {
            var unit = container.Unit;

            if (unit != null && unit.gameObject.tag == "Unit")
            {
                unit.Waypoint = next;
                if (unit.CurrentCommand == Assets.Scripts.Common.Command.Move || unit.CurrentCommand == Assets.Scripts.Common.Command.MoveAttackingEverythingOnTheWay)
                    unit.IssueOrder(Assets.Scripts.Common.Command.Idle, null);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
