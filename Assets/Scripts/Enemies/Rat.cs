namespace Enemies
{
	public class Rat : Enemy
	{
        public Rat()
        {
            MinLvl = 0;
            MaxLvl = 3;
        }

	    protected override void Awake()
		{
			MaxHitPoints = 6;
			Damage = 1;
			Armor = 0;
			AttackCooldown = 0.5f;
			Exp = 2;

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