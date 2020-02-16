using UnityEngine;

namespace ControllerScripts
{
	public class BulletController : BaseObjects
	{
		private PlayerController _playerController;

	    protected override void Awake()
		{
			_playerController = FindObjectOfType<PlayerController>();
		}

		//private void Start()
		//{
		//	//transform.LookAt(_playerController.enemy.transform.position);
		//}

		private void Update()
		{
			if (_playerController.Enemy != null)
			{
				transform.position = Vector3.MoveTowards(transform.position, _playerController.Enemy.transform.position, 10 * Time.deltaTime);
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag("Enemy"))
			{
				Destroy(gameObject);
			}
		}
	}
}