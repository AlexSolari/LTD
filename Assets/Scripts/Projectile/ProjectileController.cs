using Assets.Scripts.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Projectile
{
    public class ProjectileController : MonoBehaviour
    {
        public Unit Emiter;
        public Unit Target;
        public int Damage;
        public int MoveSpeed = 95;

        private bool willBeDestroyed = false;

        private new Rigidbody rigidbody;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (willBeDestroyed)
                return;

            if (Target != null && Target.transform != null && transform != null)
            {

                var speedVector = Target.transform.position - transform.position;
                speedVector = speedVector.normalized * MoveSpeed;
                speedVector.y = 0;

                rigidbody.velocity = speedVector;
            }
            else
            {
                StartCoroutine(RemoveSelf());
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (willBeDestroyed)
                return;

            var collided = other.gameObject;

            if (collided == Target.gameObject)
            {
                Emiter.DealDamage(Damage, Target);
                RemoveSelf();

                StartCoroutine(RemoveSelf());
            }
        }

        private IEnumerator RemoveSelf()
        {
            willBeDestroyed = true;
            rigidbody.velocity = new Vector3();
            yield return new WaitForSeconds(2f);

            Destroy(gameObject);
        }
    }
}
