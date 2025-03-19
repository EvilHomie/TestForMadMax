using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TESTSurviveModStatistics : MonoBehaviour
{
    public static TESTSurviveModStatistics Instance;

    [SerializeField] TextMeshProUGUI _leftTimeForEnemyPowerUp;
    [SerializeField] TextMeshProUGUI _powerUpMod;
    [SerializeField] TextMeshProUGUI _enemyTir;

    [SerializeField] TextMeshProUGUI _playerWeaponDmg;
    [SerializeField] TextMeshProUGUI _playerWeaponFireRate;
    [SerializeField] TextMeshProUGUI _playerWeaponReloadTime;
    [SerializeField] TextMeshProUGUI _playerWeaponMagCapacity;

    [SerializeField] TextMeshProUGUI _playerVehicleHullHP;
    [SerializeField] TextMeshProUGUI _playerVehicleShieldHP;
    [SerializeField] TextMeshProUGUI _playerVehicleShieldRegRateHP;

    [SerializeField] TextMeshProUGUI _cardsPackAvailableAmount;

    [SerializeField] TMP_InputField _PUDelay;
    [SerializeField] TMP_InputField _PUValue;
    [SerializeField] TMP_InputField _KillCount;


    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

    }

    public void Init()
    {
        if (!GameConfig.Instance.IsTesting)
        {
            gameObject.SetActive(false);
        }
    }

    public void UpdateEnemyData(float leftPowerUpTime, float powerUpMod, EnemyLevel enemyTir)
    {
        if (!gameObject.activeSelf) return;
        _leftTimeForEnemyPowerUp.text = $"�������� {string.Format("{0:f}", leftPowerUpTime)}";
        _powerUpMod.text = $"���� ������ {powerUpMod}";
        _enemyTir.text = $"��� ������ {enemyTir}";
    }

    public void UpdatePlayerWeaponData(float playerWeaponDmg, float playerWeaponFireRate, float playerWeaponReloadTime, float playerWeaponMagCapacity)
    {
        if (!gameObject.activeSelf) return;
        _playerWeaponDmg.text = $"���� {playerWeaponDmg}";
        _playerWeaponFireRate.text = $"����-�� {playerWeaponFireRate}";
        _playerWeaponReloadTime.text = $"����� ��� {playerWeaponReloadTime}";
        _playerWeaponMagCapacity.text = $"������� {playerWeaponMagCapacity}";
    }

    public void UpdatePlayerVehicleData(float hullHP, float shieldHP, float shieldRegRate)
    {
        if (!gameObject.activeSelf) return;
        _playerVehicleHullHP.text = $"�� ������� {hullHP}";
        _playerVehicleShieldHP.text = $"�� ���� {shieldHP}";
        _playerVehicleShieldRegRateHP.text = $"��� ���� {shieldRegRate}";
    }

    public void UpdateCardsPack(int packAmount)
    {
        if (!gameObject.activeSelf) return;
        _cardsPackAvailableAmount.text = $"���-�� ����� {packAmount}";
    }

    public void SetDifficultValues()
    {
        float.TryParse(_PUDelay.text, out float updateDelay);
        float.TryParse(_PUValue.text, out float updateValue);
        int.TryParse(_KillCount.text, out int amount);

        SurviveModeManager.Instance.ChangeDifficultValues(updateDelay, updateValue, amount);
    }
}
