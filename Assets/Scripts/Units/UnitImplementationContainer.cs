using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Units
{
    public class UnitImplementationContainer : MonoBehaviour
    {
        public GameObject projectile;

        public Unit Unit { get; set; }
        public bool Initialized { get; set; } = false;
        public bool UnitInitialized { get; set; } = false;
        
        public void InitializeWith<TUnit>()
            where TUnit : Unit
        {
            Unit = Activator.CreateInstance<TUnit>();

            Unit.rigidbody = GetComponent<Rigidbody>();
            Unit.transform = GetComponent<Transform>();
            Unit.animator = gameObject.GetComponentInChildren<Animator>();
            Unit.gameObject = gameObject;
            Unit.navMeshAgent = GetComponent<NavMeshAgent>();
            Unit.projectile = projectile;
            Unit.Instantiate = Instantiate;
            Initialized = true;
        }

        private void FixedUpdate()
        {
            if (!Initialized)
                return;

            if (Unit == null)
                return;

            if (!UnitInitialized)
            {
                Unit.OnAwake();
                UnitInitialized = true;
            }

            Unit.OnFixedUpdate();
        }
    }
}
