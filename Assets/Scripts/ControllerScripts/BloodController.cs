using UnityEngine;

namespace ControllerScripts
{
	public class BloodController : MonoBehaviour
	{

		public Sprite[] BloodSprites;
	    private SpriteRenderer _sprite;
		

		private void Awake()
		{
			_sprite = gameObject.GetComponent<SpriteRenderer>();
			_sprite.sprite = BloodSprites[Random.Range(0, 3)];
			
		}

		private void Start()
		{
			Destroy(gameObject, 4);
		}

		private void Update()
		{
			var color = _sprite.color;
			color.a -= 0.2f * Time.deltaTime;
			color.a = Mathf.Clamp(color.a, 0, 1);
			_sprite.color = color;
		}
	}
}