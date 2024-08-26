using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelInfo : MonoBehaviour
{
    [SerializeField] Button _selectBtn;
    [SerializeField] Image _isSelectedImage;
    [SerializeField] string _levelName;

    [SerializeField] List<EnemyVehicleManager> enemyList;
    [SerializeField] int _enemyCount = 1;
    [SerializeField] float _enemySlideDistanceMod = 1;
    [SerializeField] float _enemyDmgMod = 1;
    [SerializeField] float _enemyHPMod = 1;

    public List<EnemyVehicleManager> EnemyList => enemyList;
    public int EnemyCount => _enemyCount;
    public float EnemySlideDistanceMod => _enemySlideDistanceMod;
    public float EnemyDmgMod => _enemyDmgMod;
    public float EnemyHPMod => _enemyHPMod;
    public Button SelectBtn => _selectBtn;
    public string LevelName => _levelName;

    public void Select()
    {
        _isSelectedImage.gameObject.SetActive(true);
    }

    public void Deselect()
    {
        _isSelectedImage.gameObject.SetActive(false);
    }
}
