namespace Enemies
{
	public class Mole : Enemy
	{
        public Mole()
        {
            MinLvl = 1;
            MaxLvl = 8;
        }

	    protected override void Awake()
		{
			MaxHitPoints = 3;
			Damage = 1;
			Armor = 2;
			AttackCooldown = 0.5f;
			Exp = 8;

			Step = 1;
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
