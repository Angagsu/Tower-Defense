
using UnityEngine;

public class Bullet : MonoBehaviour
{
	private Transform target;

	[SerializeField] private float speed = 70f; 
	[SerializeField] private int damageToEnemy = 50;
	[SerializeField] private int damageToHero = 25;
	[SerializeField] private float explosionRadius = 0f;
	[SerializeField] private GameObject impactEffect;
	private Camera cameraMain;

    private void Start()
    {
		cameraMain = Camera.main;
    }

    public void BulletSeek(Transform _target)
	{
		target = _target;
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
		GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
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
			DamageToDefenders(target);
		}

		Destroy(gameObject);
	}

	void Explode()
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
		foreach (Collider collider in colliders)
		{
			if (collider.tag == "Enemy")
			{
				Damage(collider.transform);
			}

            if (collider.tag == "ArcherHero" || collider.tag == "KnightHero")
            {
				DamageToHeroes(collider.transform);
            }

            if (collider.tag == "Defender")
            {
				DamageToDefenders(collider.transform);
            }
		}
	}

	void Damage(Transform enemy)
	{
		Enemy e = enemy.GetComponent<Enemy>();

		if (e != null)
		{
			e.AmountOfDamagetoEnemy(damageToEnemy);
		}
        else
        {
			DamageToHeroes(target);
		}
	}

	private void DamageToHeroes(Transform hero)
    {
		Hero h = hero.GetComponent<Hero>();

        if (h != null)
        {
			h.AmountOfDamagetoHero(damageToHero);
        }
    }

	private void DamageToDefenders(Transform defender)
    {
		SworderDefender def = defender.GetComponent<SworderDefender>();

        if (def != null)
        {
			def.AmountOfDamagetoDefender(damageToHero);
        }
    }
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, explosionRadius);
	}
}
