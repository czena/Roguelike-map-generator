using UnityEngine;

namespace ControllerScripts
{
	public class EnemyController : BaseObjects
	{

		private float _maxHitPoints;
		private float _currentHitPoints;
		private float _damage;
		private float _armor;
		private GameObject _target;
		private float _attackCooldown;
		private float _lastAttack;
		
		void Start()
		{
			_attackCooldown = 0.8f;
			_maxHitPoints = 10;
			_currentHitPoints = _maxHitPoints;
			_damage = 2;
			_armor = 0;
		}


		void Update()
		{
			if (_currentHitPoints <= 0)
			{
				Destroy(gameObject);
			}
		}

		public void TakeDamage(float dmg)
		{
			_currentHitPoints -= dmg - _armor;
		}
		
		
		
	}
}