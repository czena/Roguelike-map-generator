using System.Collections;
using Enemies;
using UnityEngine;
using UnityEngine.Events;

namespace ControllerScripts
{
	public class PlayerController : BaseObjects
	{

		private PlayerStats _playerStats;
		private int _mode;
		private bool _canMove = true;
		private bool _move = false;
		public GameObject Enemy;
		[SerializeField] private GameObject _bullet;
		private Transform _playerCheckArea;
		//private Transform _bulletSpawner;
		
		private Transform _nextStageTile;
		private Transform _backStageTile;
		
		private Transform _nextSectionTile;
		private Transform _backSectionTile;
		
		private Vector3 _destinationPoint;


	    private GameManager _gameManager;

		public UnityAction<Transform, Vector3, float> HeroMeleeAttack;
		public UnityAction<Transform, Vector3, float> HeroRangeAttack;
		public UnityAction<Transform> OpenChest;
		
		public Sprite[] Sprites = new Sprite[4];

		protected override void Awake()
		{
			base.Awake();
			Debug.Log("AwakeMover");
		}

		private void Start()
		{
			_playerStats = gameObject.GetComponent<PlayerStats>();
            _gameManager= FindObjectOfType<GameManager>();
            _playerCheckArea = gameObject.transform.Find("CheckArea").transform;
			//_bulletSpawner = gameObject.transform.Find("Gun").transform;
			SpriteRenderer.sprite = Sprites[0];
			_destinationPoint = transform.position;
		}

		void Update()
		{
			//Примитивная система перемещения персонажа с проверкой на проходимость.
			if (_canMove)
			{
				if (Input.GetKey(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
				{
					SpriteRenderer.sprite = Sprites[2];
					_playerCheckArea.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
					_destinationPoint = new Vector3(transform.position.x, transform.position.y + 1, 0);
					PlayerCheckMove(transform, _playerCheckArea);
				}

				else if (Input.GetKey(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
				{
					SpriteRenderer.sprite = Sprites[0];
					_playerCheckArea.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
					_destinationPoint = new Vector3(transform.position.x, transform.position.y - 1, 0);
					PlayerCheckMove(transform, _playerCheckArea);
				}

				else if (Input.GetKey(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
				{
					SpriteRenderer.sprite = Sprites[1];
					_playerCheckArea.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
					_destinationPoint = new Vector3(transform.position.x - 1, transform.position.y, 0);
					PlayerCheckMove(transform, _playerCheckArea);
				}

				else if (Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
				{
					SpriteRenderer.sprite = Sprites[3];
					_playerCheckArea.rotation = Quaternion.Euler(new Vector3(0, 0, 270));
					_destinationPoint = new Vector3(transform.position.x + 1, transform.position.y, 0);
					PlayerCheckMove(transform, _playerCheckArea);
				}
				
				else if (Input.GetKeyDown(KeyCode.Space))
				{
					PlayerCheckEnemy(_playerStats.AttackDistance);
				}

				else if (Input.GetKeyDown(KeyCode.Q))
				{
					Heal();
				}
			}

			if (_playerStats.CurrentHitPoints <= 0)
			{
				Destroy(gameObject);
			}
		}

		private void PlayerCheckMove(Transform pos, Transform dir)
		{
			RaycastHit2D hit = Physics2D.Raycast(pos.position, dir.TransformDirection(Vector2.up), 1);

			if (hit.collider != null)
			{
				if (hit.collider.CompareTag("Wall"))
				{
					
				}
				//else if (hit.collider.CompareTag("NPC"))
				//{

				//}
				else if (hit.collider.CompareTag("Enemy"))
				{
					Enemy = hit.collider.gameObject;
					hit.collider.GetComponent<Enemy>().EnemyDead += EndFight;
					hit.collider.GetComponent<Enemy>().EnemyAttack += TakeDamage;
					StartCoroutine(Attack(hit.collider.transform));
				}
				else if (hit.collider.CompareTag("EndPoint"))
				{
					_nextStageTile = hit.collider.transform;
					StartCoroutine(Move(_destinationPoint, 5));
				}
				else if (hit.collider.CompareTag("StartPoint"))
				{
					_backStageTile = hit.collider.transform;
					StartCoroutine(Move(_destinationPoint, 5));
				}
				else if (hit.collider.CompareTag("EndSectionPoint"))
				{
					_nextSectionTile = hit.collider.transform;
					StartCoroutine(Move(_destinationPoint, 5));
				}
				else if (hit.collider.CompareTag("StartSectionPoint"))
				{
					_backSectionTile = hit.collider.transform;
					StartCoroutine(Move(_destinationPoint, 5));
				}
				else if (hit.collider.CompareTag("Chest"))
				{
					OpenChest(hit.collider.transform);
				}
				else
				{
					StartCoroutine(Move(_destinationPoint, 5));
				}
			}
			else
			{
				Debug.Log("Wall = null");
				StartCoroutine(Move(_destinationPoint, 5));
			}
		}
		
		private void PlayerCheckEnemy(float dist)
		{	
			Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, dist);

			float nearestTarget = dist;
			
						
			for (int i = 0; i < targets.Length; i++)
			{				
				if (targets[i].CompareTag("Enemy"))
				{
					if (Vector3.Distance(targets[i].transform.position, transform.position) < nearestTarget)
					{
						Enemy = targets[i].gameObject;
						nearestTarget = Vector3.Distance(targets[i].transform.position, transform.position);
					}
					Debug.Log(targets[i].transform);
				}
			}

			if (Enemy != null)
			{
				RaycastHit2D hit = Physics2D.Linecast(transform.position, Enemy.transform.position);
				if (hit.collider != null)
				{
					if (hit.collider.CompareTag("Enemy"))
					{
						if (Vector3.Distance(Enemy.transform.position, transform.position) > 1f)
						{
							Enemy.GetComponent<Enemy>().EnemyDead += EndFight;
							Enemy.GetComponent<Enemy>().EnemyAttack += TakeDamage;
							StartCoroutine(RangeAttack());
						}
						Debug.Log(nearestTarget);
					}
				}
			}
		}
		
		private void TakeDamage(float dmg)
		{
			_playerStats.CurrentHitPoints -= dmg;
		}

		private IEnumerator Move(Vector3 endPoint, float step)
		{
			_canMove = false;
			do
			{
				transform.position = Vector3.MoveTowards(transform.position, endPoint, step * Time.deltaTime);
				yield return new WaitForEndOfFrame();
			} while (transform.position != endPoint);
			
			if (transform.position == endPoint)
			{
				if (_nextStageTile != null)
				{
					if (new Vector2(transform.position.x, transform.position.y) == new Vector2(_nextStageTile.transform.position.x, _nextStageTile.transform.position.y))
					{
						_playerStats.CurrentStage++;
                        _gameManager.SectionGenerator.DisplayLevel(_playerStats.CurrentStage, _playerStats.CurrentSection);
						gameObject.transform.position += _playerCheckArea.up * 2;
					}
					
				}
				if (_backStageTile != null)
				{
					if (_playerStats.CurrentStage != 1)
					{
						if (new Vector2(transform.position.x, transform.position.y) == new Vector2(_backStageTile.transform.position.x, _backStageTile.transform.position.y))
						{
							_playerStats.CurrentStage--;
						    _gameManager.SectionGenerator.DisplayLevel(_playerStats.CurrentStage, _playerStats.CurrentSection);
                            gameObject.transform.position += _playerCheckArea.up * 2;
						}
					}
					else
					{
						if (new Vector2(transform.position.x, transform.position.y) == new Vector2(_backStageTile.transform.position.x, _backStageTile.transform.position.y))
						{
							_playerStats.CurrentStage = _playerStats.CurrentSection - 1;
							_playerStats.CurrentSection--;
						    _gameManager.SectionGenerator.DisplayLevel(_playerStats.CurrentStage, _playerStats.CurrentSection);
                            gameObject.transform.position += _playerCheckArea.up * 2;
						}
					}
				}
				if (_nextSectionTile != null)
				{
					if (new Vector2(transform.position.x, transform.position.y) == new Vector2(_nextSectionTile.transform.position.x, _nextSectionTile.transform.position.y))
					{
						_playerStats.CurrentStage = 1;
						_playerStats.CurrentSection++;
					    _gameManager.SectionGenerator.DisplayLevel(_playerStats.CurrentStage, _playerStats.CurrentSection);
                        gameObject.transform.position += _playerCheckArea.up * 2;
					}
					
				}
				if (_backSectionTile != null && _playerStats.CurrentSection != 1)
				{
					if (new Vector2(transform.position.x, transform.position.y) == new Vector2(_backSectionTile.transform.position.x, _backSectionTile.transform.position.y))
					{
						_playerStats.CurrentStage = _playerStats.CurrentSection - 1;
						_playerStats.CurrentSection--;
					    _gameManager.SectionGenerator.DisplayLevel(_playerStats.CurrentStage, _playerStats.CurrentSection);
                        gameObject.transform.position += _playerCheckArea.up * 2;
					}
					
				}
				yield return new WaitForSeconds(0.02f);
				_canMove = true;
			}
		}
		
		private void EndFight(float exp)
		{
			_playerStats.CurrentExperince += exp;
			Enemy = null;
			StartCoroutine(Move(_destinationPoint, 5));
		}
		
		private IEnumerator Attack(Transform target)
		{
			bool forward = true;
			_canMove = false;
			Vector3 startPoint;
			startPoint = transform.position;

			while (Enemy != null)
			{
				if (forward)
				{
					transform.position = Vector3.MoveTowards(transform.position, target.position, 5 * Time.deltaTime);
					if (Vector3.Distance(transform.position, target.position) <= 0.5f)
					{
						HeroMeleeAttack(target.transform, startPoint, _playerStats.MeleeDamage);
                        
						forward = false;
					}
				}
				else
				{
					transform.position = Vector3.MoveTowards(transform.position, startPoint, 5 * Time.deltaTime);
					if (transform.position == startPoint)
					{
						yield return new WaitForSeconds(_playerStats.AttackCooldown);
						forward = true;
					}
				}

				yield return new WaitForEndOfFrame();
			}
		}
		
		private IEnumerator RangeAttack()
		{
			_canMove = false;
			while (Enemy != null && Vector3.Distance(Enemy.transform.position, transform.position) > 1f)
			{
				HeroRangeAttack(Enemy.transform, transform.position, _playerStats.RangeDamage);
				yield return new WaitForSeconds(_playerStats.RangeAttackCooldown);
			}
			
			if (Enemy != null && Vector3.Distance(Enemy.transform.position, transform.position) <= 1f)
			{
				StartCoroutine(Attack(Enemy.transform));
			}
			
			yield return new WaitForEndOfFrame();
		}

		private void Heal()
		{
			if (_playerStats.CurrentEnergy >= 10)
			{
				_playerStats.CurrentHitPoints += 10;
				_playerStats.CurrentEnergy -= 10;
			}
		}
	}
}
