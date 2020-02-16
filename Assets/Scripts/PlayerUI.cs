using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private PlayerStats _playerStats;
    private GameObject _playerStatBar;
    private GameObject _playerStatWindow;
    public GameObject HpBar;
    public GameObject EnergyBar;
    public GameObject ExpBar;

    public Text StrengthValue;
    public Text AgilityValue;
    public Text IntellectValue;
    public Text MdValue;
    public Text RdValue;
    public Text ArmorValue;
    public Text AsValue;
    public Text EpValue;
    public Text AccuracyValue;
    public Text CdValue;
    public Text CcValue;

    public Text StageText;
    public Text SectionText;

    public Button LvlUpButton;
    public Button StrengthUpButton;
    public Button AgilityUpButton;
    public Button IntellectUpButton;
    public Button AcceptButton;

    //private void Awake()
    //{

    //}

    private void Update()
    {
        if (_playerStats == null)
        {
            _playerStats = FindObjectOfType<PlayerStats>().GetComponent<PlayerStats>();
            _playerStatWindow = GameObject.FindWithTag("StatWindow").gameObject;
            _playerStatBar = GameObject.FindWithTag("HPBar").gameObject;

            HpBar = _playerStatBar.transform.Find("HP_Bar").gameObject;
            EnergyBar = _playerStatBar.transform.Find("Energy_Bar").gameObject;
            ExpBar = _playerStatBar.transform.Find("Exp_Bar").gameObject;

            _playerStatWindow.SetActive(false);
            LvlUpButton.gameObject.SetActive(false);
            StrengthUpButton.gameObject.SetActive(false);
            AgilityUpButton.gameObject.SetActive(false);
            IntellectUpButton.gameObject.SetActive(false);
            //acceptButton.gameObject.SetActive(false);
        }
			
			

        if (_playerStats.StatPoints > 0)
        {
            StrengthUpButton.gameObject.SetActive(true);
            AgilityUpButton.gameObject.SetActive(true);
            IntellectUpButton.gameObject.SetActive(true);
        }
        else
        {
            StrengthUpButton.gameObject.SetActive(false);
            AgilityUpButton.gameObject.SetActive(false);
            IntellectUpButton.gameObject.SetActive(false);
        }
			
        StrengthValue.text = _playerStats.Strength.ToString(CultureInfo.InvariantCulture);
        AgilityValue.text = _playerStats.Agility.ToString(CultureInfo.InvariantCulture);
        IntellectValue.text = _playerStats.Intellect.ToString(CultureInfo.InvariantCulture);
        MdValue.text = _playerStats.MeleeDamage.ToString(CultureInfo.InvariantCulture);
        RdValue.text = _playerStats.RangeDamage.ToString(CultureInfo.InvariantCulture);
        ArmorValue.text = _playerStats.Armor.ToString(CultureInfo.InvariantCulture);
        AsValue.text = _playerStats.AttackCooldown.ToString(CultureInfo.InvariantCulture);

        StageText.text = "Stage: " + _playerStats.CurrentStage;
        SectionText.text = "Section: " + _playerStats.CurrentSection;

        HpBar.GetComponent<Image>().fillAmount = _playerStats.CurrentHitPoints / _playerStats.MaxHitPoints;
        EnergyBar.GetComponent<Image>().fillAmount = _playerStats.CurrentEnergy / _playerStats.MaxEnergy;
        ExpBar.GetComponent<Image>().fillAmount = _playerStats.CurrentExperince / _playerStats.MaxExperience;
    }

    public void OnClickLvlUpButton()
    {
        LvlUpButton.gameObject.SetActive(false);
        _playerStatWindow.SetActive(true);
    }

    public void OnClickStrUpButton()
    {
        _playerStats.StatPoints--;
        _playerStats.Strength++;
        Debug.Log("Strength++");
    }
    public void OnClickAgiUpButton()
    {
        _playerStats.StatPoints--;
        _playerStats.Agility++;
    }
    public void OnClickIntUpButton()
    {
        _playerStats.StatPoints--;
        _playerStats.Intellect++;
    }

    public void OnClickAcceptButton()
    {
        _playerStatWindow.SetActive(false);
    }
}