using UnityEngine;

public class PlayerSpawner : BaseObjects
{

    //[SerializeField] private GameObject _player;

    protected override void Awake()
    {
        Instantiate(FindObjectOfType<GameManager>().ResManager.GetPlayer(), transform.position, Quaternion.identity);
    }

    /*
		void CreatePlayer ()
		{
			{
				//Debug.Log("Start Создается персонаж");
				GameObject player=Instantiate(_player, transform.position, Quaternion.identity) as GameObject;
				player.name = _player.name;
			}
		}
		*/
}