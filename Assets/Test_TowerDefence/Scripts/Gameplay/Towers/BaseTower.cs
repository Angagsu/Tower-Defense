using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.Tower
{
    public abstract class BaseTower : MonoBehaviour, IAttackableTower
    {
        public BaseMonster TargetedMonster => targetedMonster;
        public float Health => health;
        public bool IsDead => isDead;


        [SerializeField] protected BaseDetection detect;
        [SerializeField] protected BaseAttack attack;
        [SerializeField] protected TowerSFXHandler towerSFXHandler;

        [Space(10)]
        [SerializeField] protected Image healthBar;
        [SerializeField] private Transform towerRotatPart;

        [Space(5)]
        [SerializeField] protected float damage;
        [SerializeField] protected float attackRate;
        [SerializeField] protected float attackRange;
        [SerializeField] protected float turnSpeed;

        
        protected GameplayStates gameplayStateHandler;
        protected BaseMonster targetedMonster;
        protected Transform target;

        protected float attackCountdown = 0f;
        protected float health;
        protected bool isDead;
        protected bool isPaused;

        
        public virtual void Construct(GameplayStates gameplayStateHandler, ProjectilesFactoriesService projectilesFactoriesService)
        {
            this.gameplayStateHandler = gameplayStateHandler;

            this.gameplayStateHandler.Unpaused += OnGameplayPlay;
            this.gameplayStateHandler.Paused += OnGameplayPause;

            ConstructAttackType(projectilesFactoriesService);

            if (gameplayStateHandler.State == GameplayState.Play)
            {
                DetectTarget();
            }  
        }

        private void ConstructAttackType(ProjectilesFactoriesService projectilesFactoriesService)
        {
            if (attack is RangeAttack)
            {
                attack.Costruct(projectilesFactoriesService);
            }
        }

        protected virtual void Awake() { }
        
        protected virtual void OnGameplayPause() => isPaused = true;

        protected virtual void OnGameplayPlay() => isPaused = false;

        protected virtual void OnDisable()
        {
            gameplayStateHandler.Unpaused -= OnGameplayPlay;
            gameplayStateHandler.Paused -= OnGameplayPause;
        }

        protected virtual void Update() 
        {
            if (!isPaused)
            {
                DetectTarget();
            }
        }

        protected virtual void DetectTarget()
        {
            if ((target = detect.DetectTarget(attackRange, IsDead)))
            {
                targetedMonster = detect.DetectedMonster;

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
            attack.AttackTarget(target, damage, TargetedMonster);
            towerSFXHandler.PlayAttackSFX();
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