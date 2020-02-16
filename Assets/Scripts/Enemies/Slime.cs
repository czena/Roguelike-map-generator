namespace Enemies
{
	public class Slime : Enemy
	{
        public Slime()
        {
            MinLvl = 0;
            MaxLvl = 4;
        }

	    protected override void Awake()
		{
			MaxHitPoints = 10;
			Damage = 2;
			Armor = 1;
			AttackCooldown = 2;
			Exp = 3;

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