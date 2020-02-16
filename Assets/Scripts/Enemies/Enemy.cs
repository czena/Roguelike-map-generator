using System.Collections;
using ControllerScripts;
using UnityEngine;
using UnityEngine.Events;
using MazeGenerator;

namespace Enemies
{
	public abstract class Enemy : BaseObjects
	{
	    private PlayerController _playerController;
	    private GameObject _player;
	    private GameManager _gameManager;
	    private PlayerStats _playerStats;
		protected float MaxHitPoints;
		[SerializeField]
		protected float CurrentHitPoints;
		protected float Damage;
		protected float Armor;
		protected float LastAttack;
		protected float AttackCooldown;
		protected float Step;
		protected float Exp;
	    protected Point _startPoint;

	    private bool _isFight;
	    private bool _isMove;

		public int MinLvl { get; set; }
	    public int MaxLvl { get; set; }

        public UnityAction<float> EnemyDead;
		public UnityAction<float> EnemyAttack;

		public GameObject Blood;
		
		protected override void Awake()
		{
			base.Awake();
			CurrentHitPoints = MaxHitPoints;

		}

		protected void Start()
		{
			_playerController = FindObjectOfType<PlayerController>();
		    _gameManager = FindObjectOfType<GameManager>();
		    _playerStats = FindObjectOfType<PlayerStats>();
			_playerController.HeroMeleeAttack += TakeMeleeDamage;
			_playerController.HeroRangeAttack += TakeRangeDamage;
			_player = _playerController.gameObject;
            _startPoint=new Point((int)transform.position.x, (int)transform.position.y);
			_isFight = false;
			_isMove = false;
		}

		protected void Update()
		{
		    if (_playerController == null)
		    {
		        _playerController = FindObjectOfType<PlayerController>();
		    }
            if (CurrentHitPoints <= 0)
			{
				EnemyDead(Exp);
                _gameManager.SectionGenerator.DestroyEnemy(_playerStats.CurrentSection, _playerStats.CurrentStage, _startPoint.X, _startPoint.Y);
				Destroy(gameObject);
			    if (_playerController != null)
			    {
			        _playerController.HeroMeleeAttack -= TakeMeleeDamage;
			        _playerController.HeroRangeAttack -= TakeRangeDamage;
			    }
			}
		}

		protected void TakeMeleeDamage(Transform goTransform, Vector3 target, float dmg)
		{
			if (gameObject.transform == goTransform)
			{
				if (_isFight == false)
				{
					StartCoroutine(Attack(target));
					_isFight = true;
				}
				_isFight = true;
				CurrentHitPoints -= dmg;
				Instantiate(Blood, new Vector3(transform.position.x + Random.Range(-0.4f, 0.4f), transform.position.y + Random.Range(-0.4f, 0.4f)), Quaternion.AngleAxis(Random.Range(0, 90), Vector3.forward));
			}
			
		}

	    public void UndescribableEvents()
	    {
	        if (_playerController != null)
	        {
	            _playerController.HeroMeleeAttack -= TakeMeleeDamage;
	            _playerController.HeroRangeAttack -= TakeRangeDamage;
	        }
        }

        protected void TakeRangeDamage(Transform goTransform, Vector3 target, float dmg)
		{
			if (gameObject.transform == goTransform)
			{
				if (_isMove == false)
				{
					Debug.Log("Time for coroutine");
					StartCoroutine(Move(target, Step));
					_isMove = true;
					
				}
				CurrentHitPoints -= dmg;
				Instantiate(Blood, new Vector3(transform.position.x + Random.Range(-0.4f, 0.4f), transform.position.y + Random.Range(-0.4f, 0.4f)), Quaternion.AngleAxis(Random.Range(0, 90), Vector3.forward));
			}
		}
		
		private IEnumerator Attack(Vector3 target)
		{
			bool forward = true;
			Vector3 startPoint;
			startPoint = transform.position;
			yield return new WaitForSeconds(AttackCooldown / 10);
			while (_player != null)
			{
				if (forward)
				{
					transform.position = Vector3.MoveTowards(transform.position, target, 5 * Time.deltaTime);
					if (Vector3.Distance(transform.position, target) <= 0.5f)
					{
						EnemyAttack(Damage);
						Debug.Log("EnemyBack!");
						forward = false;
					}
				}
				else
				{
					Debug.Log("EnemyBack!");
					transform.position = Vector3.MoveTowards(transform.position, startPoint, 5 * Time.deltaTime);
					if (transform.position == startPoint)
					{
						yield return new WaitForSeconds(AttackCooldown);
						forward = true;
					}
				}
				yield return new WaitForEndOfFrame();
			}
		}
		
		private IEnumerator Move(Vector3 endPoint, float step)
		{
			do
			{
				Debug.Log("step");
				transform.position = Vector3.MoveTowards(transform.position, endPoint, step * Time.deltaTime);
				yield return new WaitForEndOfFrame();
			} 
			while (Vector3.Distance(endPoint, transform.position) > 1);
			
			Debug.Log("melee fight");
		}
	}
}