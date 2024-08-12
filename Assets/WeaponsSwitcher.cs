using UnityEngine;
using UnityEngine.UI;

public class WeaponsSwitcher : MonoBehaviour
{
    public static WeaponsSwitcher Instance;

    [SerializeField] Button _weaponButton_0;
    [SerializeField] Button _weaponButton_1;

    [SerializeField] Sprite _weaponBGNotSelected;
    [SerializeField] Sprite _weaponBGSelected;


    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        _weaponButton_0.onClick.AddListener(delegate { OnSelectWeapon(0); });
        _weaponButton_1.onClick.AddListener(delegate { OnSelectWeapon(1); });
    }

    public void OnPlayerEndRaid()
    {
        _weaponButton_0.image.sprite = _weaponBGSelected;
    }


    public void OnSelectWeapon(int index)
    {
        PlayerWeaponPointManager.Instance.ChangeWeapon(index);
        if (index == 0)
        {
            _weaponButton_0.image.sprite = _weaponBGSelected;
            _weaponButton_1.image.sprite = _weaponBGNotSelected;
        }
        else if (index == 1) 
        {
            _weaponButton_0.image.sprite = _weaponBGNotSelected;
            _weaponButton_1.image.sprite = _weaponBGSelected;
        }
    }
}
