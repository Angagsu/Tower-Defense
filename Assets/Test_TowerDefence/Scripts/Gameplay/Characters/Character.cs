using UnityEngine;


namespace Assets.Scripts
{
    public abstract class Character : MonoBehaviour, IAttackable
    {
        [field: SerializeField] public Transform PartForTargeting { get; private set; }
        [field: SerializeField] public AttackStats AttackStats {  get; protected set; }

        [Space(15)]
        [SerializeField] protected float damage;
        [SerializeField] protected float moveSpeed;
        [SerializeField] protected float attackRate;
        [SerializeField] protected float attackRange;
        [SerializeField] protected float turnSpeed;
        [SerializeField] protected HealthBarInstance healthBarInstance;
        [SerializeField] protected HealthBarInstance armorBar;


        protected float health;
        protected bool isDead;

        public float Health => health;
        public bool IsDead => isDead;


        public virtual void TakeDamage(AttackStats enemy) { }

        public virtual void TakePureDamage(float amount) { }

        protected abstract void Move(Transform target);

        protected abstract void Attack(Transform target);

        protected abstract void DetectTarget();

        protected abstract void OnDie();

        public virtual void OnRevive() { }

        public virtual void TakePeriodicDamage(AttackStats attackStats, float periodicDamage, float periodicDamageDuration) { }
        public virtual void GetStun(float passiveSkill_2) { }
        public virtual void Decelerate(float passiveSkill_1, float passiveSkill_2) {}
    }
}