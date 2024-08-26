using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaponsSwitcher : MonoBehaviour
{
    public static UIWeaponsSwitcher Instance;

    [SerializeField] Sprite _weaponBGNotSelected;
    [SerializeField] Sprite _weaponBGSelected;
    [SerializeField] List<InRaidWeaponSlot> _inRaidWeaponsSlots;

    int _lastSelectedWeaponIndex = 1;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
    private void Start()
    {
        foreach (var uiSlot in _inRaidWeaponsSlots)
        {
            uiSlot.selectBtn.onClick.AddListener(delegate { OnSelectWeapon(uiSlot.slotIndex); });
        }
    }

    public void OnPlayerStartRaid()
    {
        foreach (var uiSlot in _inRaidWeaponsSlots)
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

        InRaidWeaponSlot slot = _inRaidWeaponsSlots.Find(slot => slot.slotIndex == 1);
        slot.selectBtn.image.sprite = _weaponBGSelected;
    }

    public void OnSelectWeapon(int weaponIndex)
    {
        if (_lastSelectedWeaponIndex == weaponIndex) return;

        InRaidWeaponSlot newSlot = _inRaidWeaponsSlots.Find(slot => slot.slotIndex == weaponIndex);
        newSlot.selectBtn.image.sprite = _weaponBGSelected;

        InRaidWeaponSlot previousSlot = _inRaidWeaponsSlots.Find(slot => slot.slotIndex == _lastSelectedWeaponIndex);
        previousSlot.selectBtn.image.sprite = _weaponBGNotSelected;

        _lastSelectedWeaponIndex = weaponIndex;
        PlayerWeaponManager.Instance.ChangeWeapon(weaponIndex);
    }
}

[Serializable]
class InRaidWeaponSlot
{
    public int slotIndex;
    public Button selectBtn;
    public Image weaponImage;
}