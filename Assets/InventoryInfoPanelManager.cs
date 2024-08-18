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

    private void Start()
    {
        _itemNameText.text = Constants.ITEMNAME;
        _itemImage.gameObject.SetActive(false);
        foreach (var characteristicRow in characteristicRows)
        {
            characteristicRow.gameObject.SetActive(false);
        }
    }


    public void UpdateInfoPanel(IItemData itemData)
    {
        _itemImage.gameObject.SetActive(true);
        _itemNameText.text = itemData.ItemName;
        _itemImage.sprite = GameAssets.Instance.GameItems.ItemsSpritesAtlas.GetSprite(itemData.ItemName);

        if (itemData is WeaponData WData)
        {
            ShowWeaponInfo(WData);
        }
        else if (itemData is VehicleData VData)
        {
            ShowVehicleInfo(VData);
        }
    }

    void ShowWeaponInfo(WeaponData data)
    {
        characteristicRows[0].SetData(Constants.HULLDMG, $"{data.hullDmgByLvl * data.hullDmgCurLvl} {Constants.PERHIT}");
        characteristicRows[1].SetData(Constants.SHIELDDMG, $"{data.shieldDmgByLvl * data.shieldDmgCurLvl} {Constants.PERHIT}");
        characteristicRows[2].SetData(Constants.ROTATIONSPEED, $"{data.rotationSpeedByLvl * data.rotationSpeedCurLvl} {Constants.DGSINSECOND}");

        if (data.weaponType != WeaponType.Beam)
            characteristicRows[3].SetData(Constants.FIRERATE, $"{data.fireRateByLvl * data.fireRateCurtLvl} {Constants.INSECOND}");
        else characteristicRows[3].gameObject.SetActive(false);
    }

    void ShowVehicleInfo(VehicleData data)
    {
        characteristicRows[0].SetData(Constants.HULLHP, $"{data.hullHPByLvl * data.hullHPCurLvl} {Constants.UNIT}");
        characteristicRows[1].SetData(Constants.SHIELDHP, $"{data.shieldHPByLvl * data.shieldHPCurLvl} {Constants.UNIT}");
        characteristicRows[2].SetData(Constants.SHIELREGENRATE, $"{data.shieldRegenRateByLvl * data.shieldRegenCurtLvl} {Constants.UNIT}{Constants.INSECOND}");
        characteristicRows[3].SetData(Constants.WEAPONSCOUNT, $"{data.curWeaponsCount} {Constants.FROM} {data.MaxWeaponsCount}");
    }
}