using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponsSwitcher : MonoBehaviour
{
    public static WeaponsSwitcher Instance;

    [SerializeField] Sprite _weaponBGNotSelected;
    [SerializeField] Sprite _weaponBGSelected;

    [SerializeField] List<UIWeaponSlot> UIWeaponSlots;



    int _lastSelectedWeaponIndex = 1;



    //[SerializeField] Button _weaponButton_0;
    //[SerializeField] Image _weaponImage_0;

    //[SerializeField] Button _weaponButton_1;
    //[SerializeField] Image _weaponImage_1;



    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
    private void Start()
    {
        foreach (var uiSlot in UIWeaponSlots)
        {
            uiSlot.selectBtn.onClick.AddListener(delegate { OnSelectWeapon(uiSlot.slotIndex); });
        }
    }

    //public void OnPlayerEndRaid()
    //{
    //    _weaponButton_0.image.sprite = _weaponBGSelected;
    //}

    public void OnPlayerStartRaid()
    {
        foreach (var uiSlot in UIWeaponSlots)
        {
            PlayerData.Instance.EquipedItems.TryGetValue(uiSlot.slotIndex, out string weaponName);
            if (weaponName != null)
            {
                uiSlot.selectBtn.gameObject.SetActive(true);
                uiSlot.weaponImage.sprite = GameAssets.Instance.GameItems.ItemsSpritesAtlas.GetSprite(weaponName);
                if(uiSlot.slotIndex == 1) uiSlot.selectBtn.image.sprite = _weaponBGSelected;
                else uiSlot.selectBtn.image.sprite = _weaponBGNotSelected;
            }
            else uiSlot.selectBtn.gameObject.SetActive(false);
        }

        UIWeaponSlot slot = UIWeaponSlots.Find(slot => slot.slotIndex == 1);
        slot.selectBtn.image.sprite = _weaponBGSelected;
    }

    public void OnSelectWeapon(int weaponIndex)
    {
        if (_lastSelectedWeaponIndex == weaponIndex) return;

        UIWeaponSlot newSlot = UIWeaponSlots.Find(slot => slot.slotIndex == weaponIndex);
        newSlot.selectBtn.image.sprite = _weaponBGSelected;

        UIWeaponSlot previousSlot = UIWeaponSlots.Find(slot => slot.slotIndex == _lastSelectedWeaponIndex);
        previousSlot.selectBtn.image.sprite = _weaponBGNotSelected;

        _lastSelectedWeaponIndex = weaponIndex;
        PlayerWeaponManager.Instance.ChangeWeapon(weaponIndex);
    }
}

[Serializable]
class UIWeaponSlot
{
    public int slotIndex;
    public Button selectBtn;
    public Image weaponImage;
}