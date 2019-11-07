using Assets.Scripts.Common;
using Assets.Scripts.Projectile;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Units
{
    public abstract class Unit
    {
        public virtual int Value { get; set; }
        public virtual int Level { get; set; }

        public virtual int AlarmRange { get; set; }
        public virtual int AttackSpeed { get; set; }
        public virtual int CurrentHP { get; set; }
        public virtual int Damage { get; set; }
        public virtual int MaxHP { get; set; }
        public virtual int Range { get; set; }

        public virtual GameObject Waypoint { get; set; }
        public virtual Command CurrentCommand { get; set; } = Command.Idle;
        public virtual AttackType AttackType { get; set; }

        public GameObject projectile;
        public GameObject Target;
        public int attackCooldown = 0;

        protected bool IsEnemyInRange = false;

        public Rigidbody rigidbody;
        public Animator animator;
        public GameObject gameObject;
        public Transform transform;
        public NavMeshAgent navMeshAgent;
        public Func<GameObject, Vector3, Quaternion, GameObject> Instantiate { get; set; }

        public virtual void Attack(GameObject target)
        {
            if (target == null)
            {
                IssueOrder(Command.Idle, null);
                return;
            }

            var distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

            IsEnemyInRange = distanceToTarget <= Range;

            if (IsEnemyInRange)
            {
                navMeshAgent.isStopped = true;
                rigidbody.velocity = new Vector3();
                UpdateAnimations();

                if (attackCooldown <= 0)
                {
                    attackCooldown = AttackSpeed;
                    var targetUnit = target.gameObject.GetComponent<UnitImplementationContainer>().Unit;

                    if (AttackType == AttackType.Melee)
                    {
                        DealDamage(Damage, targetUnit);
                    }
                    else
                    {
                        FireProjectile(Damage, targetUnit);
                    }

                }
            }
            else
            {
                navMeshAgent.isStopped = false;
                UpdateAnimations();
                MoveToRange(target);
            }
        }
        public virtual void DealDamage(int damage, Unit to)
        {
            to.CurrentHP -= damage;
        }
        public virtual void LookAt(GameObject target)
        {
            Vector3 lookPos = Target.gameObject.transform.position - transform.position;

            lookPos.y = 0;

            Quaternion rotation = Quaternion.LookRotation(lookPos);

            if (Quaternion.Angle(transform.rotation, rotation) > 15)
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.5f);
        }
        public virtual void MoveAttacking(GameObject target)
        {
            if (target == null)
            {
                IssueOrder(Command.Idle, null);
                return;
            }
            //Temporary solution
            MoveTo(target);
        }
        public virtual void MoveTo(GameObject target)
        {
            if (Vector3.Distance(target.transform.position, transform.position) < 3)
            {
                navMeshAgent.velocity = Vector3.zero;
            }

            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(target.transform.position);
        }
        public virtual void ProcessAI()
        {
            if (CurrentCommand == Command.Idle)
            {
                IssueOrder(Command.MoveAttackingEverythingOnTheWay, Waypoint);
            }

            switch (CurrentCommand)
            {
                case Command.Move:
                    MoveTo(Target);
                    break;
                case Command.Attack:
                    Attack(Target);
                    break;
                case Command.MoveAttackingEverythingOnTheWay:
                    MoveAttacking(Target);
                    break;
                case Command.Idle:
                default:
                    break;
            }

            if (CurrentCommand != Command.Idle)
                LookAt(Target);
        }
        public virtual void FireProjectile(int damage, Unit targetUnit)
        {
            var spawnPoint = transform.position;
            spawnPoint.y += 3;
            var instance = Instantiate(projectile, spawnPoint, Quaternion.identity);
            var controller = instance.GetComponent<ProjectileController>();

            controller.Damage = Damage;
            controller.Emiter = this;
            controller.Target = targetUnit;
        }
        public virtual void MoveToRange(GameObject target)
        {
            if (Vector3.Distance(Target.transform.position, transform.position) <= Range)
                return;

            var destinationVector = Target.transform.position - transform.position;
            destinationVector.Normalize();

            var destination = transform.position + destinationVector * 2;

            if (Vector3.Distance(target.transform.position, transform.position) < 3)
            {
                navMeshAgent.velocity = Vector3.zero;
            }
            navMeshAgent.SetDestination(destination);
        }
        public virtual void OnDeath()
        {

        }
        public virtual void UpdateAnimations()
        {
            if (animator != null)
            {
                switch (CurrentCommand)
                {
                    case Command.Move:
                    case Command.MoveAttackingEverythingOnTheWay:
                        animator.SetBool("IsAttacking", false);
                        animator.SetBool("IsMoving", true);
                        break;
                    case Command.Attack:
                        if (IsEnemyInRange)
                        {
                            if (attackCooldown <= 0)
                            {
                                animator.SetBool("IsMoving", false);
                                animator.SetBool("IsAttacking", true);
                            }
                            else
                            {
                                animator.SetBool("IsAttacking", false);
                                animator.SetBool("IsMoving", false);
                            }
                        }
                        else
                        {
                            animator.SetBool("IsAttacking", false);
                            animator.SetBool("IsMoving", true);
                        }
                        break;
                    case Command.Idle:
                    default:
                        animator.SetBool("IsAttacking", false);
                        animator.SetBool("IsMoving", false);
                        break;
                }
            }
        }
        public virtual void IssueOrder(Command order, GameObject target)
        {
            Target = target;
            CurrentCommand = order;

            UpdateAnimations();
        }
        public virtual void OnAwake()
        {

        }
        public virtual void OnFixedUpdate()
        {
            attackCooldown -= 1;

            if (CurrentHP <= 0)
            {
                OnDeath();
                Destroy();
            }
        }

        public void Destroy()
        {
            GameObject.Destroy(gameObject);
        }
    }
}