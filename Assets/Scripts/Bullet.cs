using Assets.Scripts;
using Assets.Scripts.Tower;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	private Transform target;

	[SerializeField] private float speed = 70f; 
	[SerializeField] private float explosionRadius = 0f;
	[SerializeField] private GameObject impactEffect;

	private Camera cameraMain;
	private float damage;

    private void Start()
    {
		cameraMain = Camera.main;
    }

    public void SetTarget(Transform _target, float damage)
	{
		target = _target;
		this.damage = damage;
	}

	
	void Update()
	{
		if (target == null)
		{
			Destroy(gameObject);
			return;
		}

		Vector3 dir = target.position - transform.position;
		float distanceThisFrame = speed * Time.deltaTime;

		if (dir.magnitude <= distanceThisFrame)
		{
			HitTarget();
			return;
		}

		transform.Translate(dir.normalized * distanceThisFrame, Space.World);
		transform.LookAt(target);

	}

	void HitTarget()
	{
		GameObject effectIns = Instantiate(impactEffect, transform.position, transform.rotation);
		Vector3 direction = effectIns.transform.position - target.transform.position;
		effectIns.transform.position = target.position + direction.normalized;
		effectIns.transform.rotation = Quaternion.LookRotation(direction);
		//effectIns.transform.LookAt(camera.transform);
		Destroy(effectIns, 1f);

		if (explosionRadius > 0f)
		{
			Explode();
		}
		else
		{
			Damage(target);
			DamageToTower(target);
		}

		Destroy(gameObject);
	}

	void Explode()
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
		foreach (Collider collider in colliders)
		{
			if (collider.gameObject.TryGetComponent(out BaseMonster monster))
			{
				Damage(collider.transform);
			}

            if (collider.gameObject.TryGetComponent<IAttackable>(out var tower)) 
            {
				tower.TakeDamage(damage);
				DamageToTower(collider.transform);
            }
		}
	}

	void Damage(Transform target)
	{
		if (target.TryGetComponent<Character>(out var enemy))
		{
			enemy.TakeDamage(damage);
		}
	}

	private void DamageToTower(Transform target)
    {
        if (target.TryGetComponent<IAttackableTower>(out var tower))
        {
			tower.TakeDamage(damage);
        }
    }
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, explosionRadius);
	}
}
