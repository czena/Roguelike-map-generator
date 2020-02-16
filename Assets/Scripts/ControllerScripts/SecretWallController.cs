using UnityEngine;

namespace ControllerScripts
{
	public class SecretWallController : BaseObjects
	{
		private GameObject _player;
		public Sprite[] Sprites;
	    private GameManager _gM;
	    private bool _isActive;


		// Use this for initialization
		void Start()
		{
			_player = FindObjectOfType<PlayerController>().gameObject;
			SpriteRenderer.sprite = Sprites[0];
		    _isActive = false;
		    _gM = FindObjectOfType<GameManager>();
		}

		// Update is called once per frame
		void Update()
		{
			    if (Vector3.Distance(_player.transform.position, transform.position) < 1)
			    {
				    SpriteRenderer.sprite = Sprites[1];
			        if (_isActive == false)
			        {
			            _isActive = true;
                        _gM.SectionGenerator.DisplaySecretRoom(FindObjectOfType<PlayerStats>().CurrentSection, FindObjectOfType<PlayerStats>().CurrentStage);
			        }
			    }
			    else
			    {
				    SpriteRenderer.sprite = Sprites[0];
			    }
		}
	}
}