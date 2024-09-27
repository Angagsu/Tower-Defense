using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public abstract class Character : MonoBehaviour
    {
        [SerializeField] protected float damage;
        [SerializeField] protected float moveSpeed;
        [SerializeField] protected float attackRate;
        [SerializeField] protected float attackRange;
        [SerializeField] protected float turnSpeed;
        [SerializeField] protected Image healthBar;

        protected float health;
        protected bool isDead;

        public float Health => health;
        public bool IsDead => isDead;

        public virtual void Revive() { }

        public abstract void TakeDamage(float amount);

        protected abstract void Move(Transform target);

        protected abstract void Attack(Transform target);

        protected abstract void DetectTarget();

        protected abstract void Die();
    }
}