using UnityEngine;

namespace ControllerScripts
{
	public class ChestController : BaseObjects
	{

		public Sprite[] Sprites;
		private PlayerController _playerController;
		
		void Start()
		{
			_playerController = FindObjectOfType<PlayerController>();
			SpriteRenderer.sprite = Sprites[0];
			_playerController.OpenChest += ChestOpened;
		}

		
		//void Update()
		//{

		//}

		private void ChestOpened(Transform chestPos)
		{
			if (transform == chestPos)
			{
				SpriteRenderer.sprite = Sprites[1];
			}
		}
	}
}