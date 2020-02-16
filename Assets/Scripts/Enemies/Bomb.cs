
namespace Enemies
{
	public class Bomb : Enemy
	{
        public Bomb()
        {
            MinLvl = 7;
            MaxLvl = 9;
        }

        private new void Awake()
		{
			MaxHitPoints = 10;
			Damage = 20;
			Armor = 3;
			AttackCooldown = 7f;
			Step = 1;
			Exp = 15;
			base.Awake();
		}

		private new void Start()
		{
			base.Start();			
		}
		
		private new void Update()
		{
			base.Update();
		}
	}
}
