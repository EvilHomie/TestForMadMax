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

    [SerializeField] TextMeshProUGUI _equipText;
    [SerializeField] TextMeshProUGUI _unlockText;
    [SerializeField] TextMeshProUGUI _characteristicsText;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void Init()
    {
        _itemNameText.text = TextConstants.ITEMNAME;
        _equipText.text = TextConstants.EQUIP;
        _unlockText.text = TextConstants.UNLOCK;
        _characteristicsText.text = TextConstants.CHARACTERISTICS;

        _itemImage.gameObject.SetActive(false);
        foreach (var characteristicRow in characteristicRows)
        {
            characteristicRow.gameObject.SetActive(false);
        }
    }


    public void UpdateInfoPanel(IItemData itemData)
    {
        _itemImage.gameObject.SetActive(true);
        _itemNameText.text = itemData.TranslatedItemName;
        _itemImage.sprite = GameAssets.Instance.GameItems.ItemsSpritesAtlas.GetSprite(itemData.DeffItemName);

        if (itemData is WeaponData WData)
        {
            _characteristicsText.text = TextConstants.CHARACTERISTICS;
            ShowWeaponInfo(WData);
        }
        else if (itemData is VehicleData VData)
        {
            _characteristicsText.text = TextConstants.CHARACTERISTICS;
            ShowVehicleInfo(VData);
        }
        else if (itemData is WeaponSchemeData WSData)
        {
            _characteristicsText.text = TextConstants.UNLOCKCOST;
            ShowUnlockInfo(WSData);
        }
    }

    void ShowWeaponInfo(WeaponData data)
    {
        characteristicRows[0].SetData(TextConstants.HULLDMG, $"{data.hullDmgByLvl * data.hullDmgCurLvl} {TextConstants.PERHIT}");
        characteristicRows[1].SetData(TextConstants.SHIELDDMG, $"{data.shieldDmgByLvl * data.shieldDmgCurLvl} {TextConstants.PERHIT}");
        characteristicRows[2].SetData(TextConstants.ROTATIONSPEED, $"{data.rotationSpeedByLvl * data.rotationSpeedCurLvl} {TextConstants.DGSINSECOND}");

        if (data.weaponType != WeaponType.Beam)
            characteristicRows[3].SetData(TextConstants.FIRERATE, $"{data.fireRateByLvl * data.fireRateCurtLvl} {TextConstants.INSECOND}");
        else characteristicRows[3].gameObject.SetActive(false);
    }

    void ShowVehicleInfo(VehicleData data)
    {
        characteristicRows[0].SetData(TextConstants.HULLHP, $"{data.hullHPByLvl * data.hullHPCurLvl} {TextConstants.UNIT}");
        characteristicRows[1].SetData(TextConstants.SHIELDHP, $"{data.shieldHPByLvl * data.shieldHPCurLvl} {TextConstants.UNIT}");
        characteristicRows[2].SetData(TextConstants.SHIELREGENRATE, $"{data.shieldRegenRateByLvl * data.shieldRegenCurtLvl} {TextConstants.UNIT}{TextConstants.INSECOND}");
        characteristicRows[3].SetData(TextConstants.WEAPONSCOUNT, $"{data.curWeaponsCount} {TextConstants.FROM} {data.maxWeaponsCount}");
    }

    void ShowUnlockInfo(WeaponSchemeData data)
    {
        foreach (var characteristicRow in characteristicRows)
        {
            characteristicRow.gameObject.SetActive(false);
        }
    }
}