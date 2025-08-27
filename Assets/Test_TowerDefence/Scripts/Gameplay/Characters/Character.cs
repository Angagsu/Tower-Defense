using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public abstract class Character : MonoBehaviour
    {
        [field: SerializeField] public Transform PartForTargeting { get; private set; }

        [SerializeField] protected float damage;
        [SerializeField] protected float moveSpeed;
        [SerializeField] protected float attackRate;
        [SerializeField] protected float attackRange;
        [SerializeField] protected float turnSpeed;
        [SerializeField] protected GPUInstancingEnabler instancingEnabler;


        protected float health;
        protected bool isDead;

        public float Health => health;
        public bool IsDead => isDead;

        public virtual void OnRevive() { }

        public abstract void TakeDamage(float amount);

        protected abstract void Move(Transform target);

        protected abstract void Attack(Transform target);

        protected abstract void DetectTarget();

        protected abstract void OnDie();
    }
}