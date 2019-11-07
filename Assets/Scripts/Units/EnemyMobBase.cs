using Assets.Scripts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Units
{
    public class EnemyMobBase : Unit
    {
        public override void OnDeath()
        {
            if (CurrentHP <= 0)
            {
                Player.Current.Gold += Value;
            }

            EventHandler.Current.Dispatch(EventHandler.Events.Wave_EnemyDied);
        }
    }
}
