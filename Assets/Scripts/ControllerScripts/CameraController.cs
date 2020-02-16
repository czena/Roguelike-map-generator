using UnityEngine;
using UnityEngine.SceneManagement;

namespace ControllerScripts
{
	public class CameraController : MonoBehaviour
	{
		private PlayerController _playerController;
		private GameObject _player;
		//private Vector3 offset;

		private void Start()
		{
			if (SceneManager.GetActiveScene().name == "Maze")
			{
				gameObject.GetComponent<Camera>().orthographicSize = 100;
				gameObject.GetComponent<Camera>().transform.position = new Vector3(200, 150, -10);
			}
			else
				gameObject.GetComponent<Camera>().orthographicSize = 5;
		}

		private void Update()
		{
			if (_playerController == null)
			{
				_playerController = FindObjectOfType<PlayerController>();
			}
			
			_player = _playerController.gameObject;
			//offset = transform.position - player.transform.position;
			
		}

		private void LateUpdate()
		{
			if (SceneManager.GetActiveScene().name != "Maze")
				transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y, _player.transform.position.z - 1);
		}
		public void Remove()
		{
			Destroy(this);
		}
	}
}