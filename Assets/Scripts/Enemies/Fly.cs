namespace Enemies
{
	public class Fly : Enemy
	{
        public Fly()
        {
            MinLvl = 5;
            MaxLvl = 9;
        }

	    protected override void Awake()
		{
			MaxHitPoints = 5;
			Damage = 2;
			Armor = 4;
			AttackCooldown = 1f;
			Step = 1;
			Exp = 10;
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
