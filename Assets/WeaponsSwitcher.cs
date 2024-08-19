using UnityEngine;
using UnityEngine.UI;

public class WeaponsSwitcher : MonoBehaviour
{
    public static WeaponsSwitcher Instance;

    [SerializeField] Button _weaponButton_0;
    [SerializeField] Image _weaponImage_0;
    [SerializeField] Button _weaponButton_1;
    [SerializeField] Image _weaponImage_1;

    [SerializeField] Sprite _weaponBGNotSelected;
    [SerializeField] Sprite _weaponBGSelected;


    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
    private void Start()
    {
        _weaponButton_0.onClick.AddListener(delegate { OnSelectWeapon(0); });
        _weaponButton_1.onClick.AddListener(delegate { OnSelectWeapon(1); });
    }

    public void OnPlayerEndRaid()
    {
        _weaponButton_0.image.sprite = _weaponBGSelected;
    }

    public void OnPlayerStartRaid()
    {
        PlayerData.Instance.EquipedItems.TryGetValue(1, out string weaponName1);
        if (weaponName1 != null)
        {
            _weaponImage_0.gameObject.SetActive(true);
            _weaponImage_0.sprite = GameAssets.Instance.GameItems.ItemsSpritesAtlas.GetSprite(weaponName1);
        }
        else _weaponImage_0.gameObject.SetActive(false);

        PlayerData.Instance.EquipedItems.TryGetValue(2, out string weaponName2);
        if (weaponName2 != null)
        {
            _weaponImage_1.gameObject.SetActive(true);
            _weaponImage_1.sprite = GameAssets.Instance.GameItems.ItemsSpritesAtlas.GetSprite(weaponName2);
        }
        else _weaponImage_1.gameObject.SetActive(false);
    }


    public void OnSelectWeapon(int index)
    {
        PlayerWeaponManager.Instance.ChangeWeapon(index);
        if (index == 0 && _weaponImage_0.gameObject.activeSelf)
        {
            _weaponButton_0.image.sprite = _weaponBGSelected;
            _weaponButton_1.image.sprite = _weaponBGNotSelected;
        }
        else if (index == 1 && _weaponImage_1.gameObject.activeSelf)
        {
            _weaponButton_0.image.sprite = _weaponBGNotSelected;
            _weaponButton_1.image.sprite = _weaponBGSelected;
        }
    }
}
