using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIEnemyHpPanel : MonoBehaviour
{
    public static UIEnemyHpPanel Instance;

    [SerializeField] Slider _HullHPSlider;
    [SerializeField] Slider _ShieldHPSlider;

    Coroutine _hideCoroutine;


    EnemyVehicleManager _lastEnemyVehicleManager;

    public EnemyVehicleManager LastEnemyVehicleManager => _lastEnemyVehicleManager;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
    public void OnPlayerStartRaid()
    {
        DisableHPBars();
    }
    public void OnPlayerEndRaid()
    {
        DisableHPBars();
    }

    public void UpdateHPBars(float HPValue, float shieldValue, EnemyVehicleManager enemyVehicleManager)
    {
        _HullHPSlider.gameObject.SetActive(true);
        _ShieldHPSlider.gameObject.SetActive(shieldValue > 0);

        _HullHPSlider.value = HPValue;
        _ShieldHPSlider.value = shieldValue;

        if (HPValue <= 0)
        {
            DisableHPBars();
            return;
        }

        if (_lastEnemyVehicleManager != enemyVehicleManager)
            _lastEnemyVehicleManager = enemyVehicleManager;

        if (_hideCoroutine == null)
        {
            _hideCoroutine = StartCoroutine(HidePanelAfterDelay());
        }
        else
        {
            StopCoroutine(_hideCoroutine);
            _hideCoroutine = StartCoroutine(HidePanelAfterDelay());
        }
    }

    public void DisableHPBars()
    {
        if (!Application.isPlaying) return;
        _HullHPSlider.gameObject.SetActive(false);
        _ShieldHPSlider.gameObject.SetActive(false);
    }

    IEnumerator HidePanelAfterDelay()
    {
        yield return new WaitForSeconds(5);
        DisableHPBars();
        _hideCoroutine = null;
    }
}
