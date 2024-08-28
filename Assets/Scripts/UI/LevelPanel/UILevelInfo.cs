using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILevelInfo : MonoBehaviour
{
    [SerializeField] ScrollRect _scrollRect;     

    [SerializeField] Button _selectBtn;
    [SerializeField] Image _levelImage;
    [SerializeField] Image _isSelectedImage;
    [SerializeField] string _levelName;
    [SerializeField] TextMeshProUGUI _unlockStatusText;

    [SerializeField] List<EnemyVehicleManager> enemyList;
    [SerializeField] int _enemyCount = 1;
    [SerializeField] float _enemySlideDistanceMod = 1;
    [SerializeField] float _enemyDmgMod = 1;
    [SerializeField] float _enemyHPMod = 1;

    [Header("OPTIONAL")]
    [SerializeField] EnemyVehicleManager _bossEnemyVehicle;

    public List<EnemyVehicleManager> EnemyList => enemyList;
    public int EnemyCount => _enemyCount;
    public float EnemySlideDistanceMod => _enemySlideDistanceMod;
    public float EnemyDmgMod => _enemyDmgMod;
    public float EnemyHPMod => _enemyHPMod;
    public Button SelectBtn => _selectBtn;
    public string LevelName => _levelName;

    public Image LevelImage => _levelImage;

    public EnemyVehicleManager BossEnemyVehicle => _bossEnemyVehicle;

    public void Select()
    {
        _isSelectedImage.gameObject.SetActive(true);
    }

    public void Deselect()
    {
        _isSelectedImage.gameObject.SetActive(false);
    }

    public void LockLevel()
    {
        _unlockStatusText.text = TextConstants.LOCKED;
        _unlockStatusText.color = Color.red;
    }

    public void UnlockLevel()
    {
        _unlockStatusText.text = TextConstants.UNLOCKED;
        _unlockStatusText.color = Color.green;
    }

    public IEnumerator Shake(float duration, float shakeIntensity)
    {
        _scrollRect.vertical = false;
        Vector3 defPos = transform.position;
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            Vector3 randomOffset = Random.insideUnitSphere * shakeIntensity;
            transform.position = defPos + randomOffset;
            yield return null;
        }
        transform.position = defPos;
        _scrollRect.vertical = true;
    }
}
