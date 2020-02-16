public class EnemyStats : BaseObjects
{
		
    private float _maxHitPoints;
    private float _currentHitPoints;
    private float _damage;
    private float _armor;
		
    void Start()
    {
        _maxHitPoints = 10;
        _currentHitPoints = _maxHitPoints;
        _damage = 2;
        _armor = 0;
    }


    void Update()
    {
        if (_currentHitPoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float dmg)
    {
        _currentHitPoints -= dmg - _armor;
    }
		
}