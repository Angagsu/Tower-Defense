using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.Tower
{
    public abstract class BaseTower : MonoBehaviour, IAttackableTower
    {
        public event Action TowerCrashed;
        public event Action TowerRepaired;

        [field: SerializeField] public AttackStats AttackStats { get; private set; }
        [field: SerializeField] public Transform PartForTargeting { get; private set; }
        public BaseMonster TargetedMonster => targetedMonster;
        public float Health => health;
        public bool IsCrashed => isCrashed;
        public bool IsDamaged => AttackStats.Health < AttackStats.MaxHealth;

        public BaseMonster CurrentAttackerMonster { get; private set; }

        [SerializeField] protected BaseDetection detect;
        [SerializeField] protected BaseAttack attack;
        [SerializeField] protected TowerSFXHandler towerSFXHandler;

        [Space(10)]
        [SerializeField] protected HealthBarInstance healthBarInstance;
        [SerializeField] private Transform towerRotatPart;

        [Space(5)]
        [SerializeField] protected float startHealth;
        [SerializeField] protected float damage;
        [SerializeField] protected float attackRate;
        [SerializeField] protected float attackRange;
        [SerializeField] protected float turnSpeed;
        


        protected TowerUpgradeLevelSO upgradeLevel;
        protected GameplayStates gameplayStateHandler;
        protected BaseMonster targetedMonster;
        protected Transform target;

        protected float attackCountdown = 0f;
        protected float health;
        protected bool isCrashed;
        protected bool isPaused;
        protected bool hasPassiveSkill;
        protected List<float> passiveSkillsList;

        protected float magicResistStartValue;
        protected float physicResistStartValue;

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

        public virtual void SetParams(TowerSO towerSO)
        {
            upgradeLevel = towerSO.CurrentUpgradeLevel;

            SetupAttackStats(towerSO.CurrentUpgradeLevel.AttackStats);

            attackRate = upgradeLevel.AttackRate;
            attackRange = upgradeLevel.AttackRange;

            if (upgradeLevel.PassiveSkillsAccessibility.Capacity != 0)
            {
                hasPassiveSkill = true;
            }
        }
        protected virtual void SetupAttackStats(AttackStats attackStats)
        {
            AttackStats.Health = attackStats.Health;
            AttackStats.MaxHealth = attackStats.MaxHealth;

            AttackStats.DamageData = attackStats.DamageData;

            AttackStats.PhysicalResistance = attackStats.PhysicalResistance;
            AttackStats.MagicalResistance = attackStats.MagicalResistance;
            

            physicResistStartValue = AttackStats.PhysicalResistance;
            magicResistStartValue = AttackStats.MagicalResistance;
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
            if ((target = detect.DetectTarget(attackRange, IsCrashed)))
            {
                targetedMonster = detect.DetectedMonster;

                if (attackCountdown <= 0)
                {
                    attackCountdown = attackRate;
                    
                    AttackTarget(target);   
                }
            }

            attackCountdown -= Time.deltaTime;
        }

        protected virtual void AttackTarget(Transform target) 
        {
            attack.AttackWithPassiveSkill(AttackStats, target, damage, hasPassiveSkill, passiveValue_1: passiveSkillsList[0], passiveValue_2: passiveSkillsList[1], TargetedMonster, TargetedMonster);
            towerSFXHandler.PlayAttackSFX();
        }

        protected virtual void Crash()
        {
            isCrashed = true;
            enabled = false;

            if (CurrentAttackerMonster)
            {
                CurrentAttackerMonster.RejectCurrentAttackerHero();
            }

            TowerCrashed?.Invoke();
            healthBarInstance.gameObject.SetActive(false);  
        }

        public virtual void Repair()
        {
            AttackStats.Health = AttackStats.MaxHealth;

            healthBarInstance.gameObject.SetActive(true);
            healthBarInstance.SetMaterialProperty((AttackStats.Health / AttackStats.MaxHealth));

            TowerRepaired?.Invoke();
            RestoreStats();

            isCrashed = false;
            enabled = true;
        }

        public virtual void TakeDamage(AttackStats enemy)
        {
            DoDamage.ApplyDamage(AttackStats, enemy.DamageData);

            healthBarInstance.gameObject.SetActive(true);

            healthBarInstance.SetMaterialProperty((AttackStats.Health / AttackStats.MaxHealth));

            if(AttackStats.Health < AttackStats.MaxHealth / 2)
            {
                ReduceStats();
            }

            if (AttackStats.Health <= 0 && !isCrashed)
            {
                Crash();
            }
        }

        protected void LockOnTarget()
        {
            Vector3 direction = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 rotation = Quaternion.Lerp(towerRotatPart.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            towerRotatPart.rotation = Quaternion.Euler(0, rotation.y, 0);
        }

        public void SetCurrentAttackerMonster(BaseMonster baseMonster)
        {
            CurrentAttackerMonster = baseMonster;
        }

        public void ClearCurrentAttackerMonster(BaseMonster baseMonster)
        {
            if (CurrentAttackerMonster == baseMonster)
            {
                CurrentAttackerMonster = null;
            }
        }

        protected void ReduceStats()
        {
            attackRate = upgradeLevel.AttackRate * 0.7f;
            attackRange = upgradeLevel.AttackRange * 0.7f;

            AttackStats.DamageData.DamageAmount = upgradeLevel.AttackStats.DamageData.DamageAmount * 0.7f;
        }

        protected void RestoreStats()
        {
            attackRate = upgradeLevel.AttackRate;
            attackRange = upgradeLevel.AttackRange;
 
            AttackStats.DamageData.DamageAmount = upgradeLevel.AttackStats.DamageData.DamageAmount;
        }

        public int GetRepairCost()
        {
            float damagePercent = (AttackStats.Health / AttackStats.MaxHealth);
            int repairCost = (int)(upgradeLevel.BuyCost - (upgradeLevel.BuyCost * damagePercent));

            return repairCost;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}