using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.Tower
{
    public abstract class BaseTower : MonoBehaviour, IAttackableTower
    {
        [SerializeField] protected BaseDetection detect;
        [SerializeField] protected BaseAttack attack; 

        [Space(10)]
        [SerializeField] protected Image healthBar;
        [SerializeField] private Transform towerRotatPart;

        [Space(5)]
        [SerializeField] protected float damage;
        [SerializeField] protected float attackRate;
        [SerializeField] protected float attackRange;
        [SerializeField] protected float turnSpeed;

        protected Transform target;
        protected float attackCountdown = 0f;

        protected float health;
        protected bool isDead;

        public float Health => health;
        public bool IsDead => isDead;


        protected virtual void Awake() 
        {
            DetectTarget();
        }

        protected virtual void Update() 
        {
            DetectTarget();
        }

        protected virtual void DetectTarget()
        {
            if ((target = detect.DetectTarget(attackRange, IsDead)))
            {
                LockOnTarget();

                if (attackCountdown <= 0)
                {
                    attackCountdown = 1 / attackRate;

                    AttackTarget(target);
                }
            }
            attackCountdown -= Time.deltaTime;
        }

        protected virtual void AttackTarget(Transform target) 
        {
            attack.AttackTarget(target, damage);
        }

        protected virtual void Crash() { }

        public virtual void ReBuild() { }

        public virtual void TakeDamage(float damageAmount) { }

        protected void LockOnTarget()
        {
            Vector3 direction = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 rotation = Quaternion.Lerp(towerRotatPart.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            towerRotatPart.rotation = Quaternion.Euler(0, rotation.y, 0);
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}