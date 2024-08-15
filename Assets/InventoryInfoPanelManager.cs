using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryInfoPanelManager : MonoBehaviour
{
    public static InventoryInfoPanelManager Instance;

    [SerializeField] TextMeshProUGUI _itemNameText;
    [SerializeField] Image _itemImage;
    [SerializeField] List<CharacteristicRow> characteristicRows;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }


    public void UpdateInfoPanel(IItemData itemData)
    {
        _itemNameText.text = itemData.ItemName;
        _itemImage.sprite = itemData.ItemSprite;

        if (itemData is WeaponData data)
        {
            ShowWeaponInfo(data);
        }
    }

    void ShowWeaponInfo(WeaponData data)
    {
        characteristicRows[0].SetData("Hull Damage", $"{data.hullDmgByLvl * data.hullDmgCurLvl} Per Hit");
        characteristicRows[1].SetData("Shield Damage", $"{data.shieldDmgByLvl * data.shieldDmgCurLvl} Per Hit");
        characteristicRows[2].SetData("Rotation Speed", $"{data.rotationSpeedByLvl * data.rotationSpeedCurLvl} DGS/Second");

        if (data.weaponType != WeaponType.Beam)
            characteristicRows[3].SetData("FireRate", $"{data.fireRateByLvl * data.fireRateCurtLvl} /Second");
        else characteristicRows[3].gameObject.SetActive(false);
    }
}