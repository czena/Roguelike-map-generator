using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private PlayerUI _playerUi;
		
		
    public string PlayerName;
    public float AttackCooldown;
    public float RangeDamage;
    public float MeleeDamage;
    public float Armor;
    public float MaxHitPoints;
    public float CurrentHitPoints;
    public float MaxEnergy;
    public float CurrentEnergy;
    public float AttackDistance;
    public float RangeAttackCooldown;
    public float Lvl;
    public float Strength;
    public float Agility;
    public float Intellect;
    public float CurrentExperince;
    public float MaxExperience;
    public float StatPoints;
    public int CurrentStage;
    public int CurrentSection;
		
    public static PlayerStats Instance;
		
    private void Awake()
    {
        Debug.Log("AwakeGameManager");
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        _playerUi = FindObjectOfType<PlayerUI>().GetComponent<PlayerUI>();
			
        Strength = 1;
        Agility = 1;
        Intellect = 1;

        CurrentExperince = 0;
        Lvl = 1;
        StatPoints = 0;
			
        PlayerName = name;
			
        AttackCooldown = 2 - 1 * 1 / Agility;
        RangeDamage = 5;
        MeleeDamage = 1 + Strength;
        Armor = Agility;
        MaxHitPoints = 20 + Strength * 5;
        CurrentHitPoints = MaxHitPoints;
        MaxEnergy = 10 + Intellect * 2;
        CurrentEnergy = MaxEnergy;
        AttackDistance = 3f;
        RangeAttackCooldown = 1;
        MaxExperience = 10;

        CurrentSection = 1;
        CurrentStage = 1;
    }


    //private void Start()
    //{
			
    //}

    private void Update()
    {
        AttackCooldown = 1;
        RangeDamage = 5;
        MeleeDamage = 1 + Strength;
        Armor = Agility;
        MaxHitPoints = 20 + Strength * 5;
        MaxEnergy = 10 + Intellect * 2;
        AttackDistance = 3f;
        RangeAttackCooldown = 1;
			
        if (CurrentExperince >= MaxExperience)
        {
            Lvl ++;
            CurrentExperince -= MaxExperience;
            MaxExperience = 10 * Lvl * Lvl;
            StatPoints += 5;
            _playerUi.LvlUpButton.gameObject.SetActive(true);
        }

        if (CurrentHitPoints > MaxHitPoints)
        {
            CurrentHitPoints = MaxHitPoints;
        }

        if (CurrentEnergy > MaxEnergy)
        {
            CurrentEnergy = MaxEnergy;
        }
    }
}