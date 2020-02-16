namespace Enemies
{
	public class Snail : Enemy
	{
        public Snail()
        {
            MinLvl = 0;
            MaxLvl = 6;
        }

	    protected override void Awake()
		{
			MaxHitPoints = 20;
			Damage = 3;
			Armor = 4;
			AttackCooldown = 4;
			Exp = 5;

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