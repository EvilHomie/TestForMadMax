using DamageNumbersPro;
using System.Collections.Generic;
using UnityEngine;

public class EventTextPanel : MonoBehaviour
{
    public static EventTextPanel Instance;

    [SerializeField] DamageNumber _numberPrefab;
    [SerializeField] RectTransform _rectParent;
    [SerializeField] RectTransform _DNHolder;
    [SerializeField] float _dmHeight;

    [SerializeField] List<DamageNumber> _existNumbers = new();

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void ShowEventText(float number, string text)
    {
        int firstFreeIndex;
        firstFreeIndex = _existNumbers.FindIndex(element => element == null);
        Debug.Log(firstFreeIndex);
        if (firstFreeIndex == -1)
        {
            _existNumbers.Add(null);
            firstFreeIndex = _existNumbers.Count - 1;
        }


        _existNumbers[firstFreeIndex] = _numberPrefab.ShowEventTextNumberAndLeftText(_rectParent, firstFreeIndex * _dmHeight * Vector2.down, number, text);
    }


    public void ShowEventPanel(UpgradeCardData upgradeCardData)
    {
        string plusSimbol;
        if (upgradeCardData.ChangeValue > 0)
        {
            plusSimbol = "+";
        }
        else plusSimbol = "";


        switch (upgradeCardData.CharacteristicsName)
        {
            case CharacteristicsName.WeaponKineticDmg:
                ShowEventText(upgradeCardData.ChangeValue, $"{TextConstants.DAMAGE} {plusSimbol}");
                break;
            case CharacteristicsName.WeaponFireRate:
                ShowEventText(upgradeCardData.ChangeValue, $"{TextConstants.FIRERATE} {plusSimbol}");
                break;
            case CharacteristicsName.WeaponReloadTime:
                ShowEventText(upgradeCardData.ChangeValue, $"{TextConstants.RELOADTIME} {plusSimbol}");
                break;
            case CharacteristicsName.WeaponMagCapacity:
                ShowEventText(upgradeCardData.ChangeValue, $"{TextConstants.CAPACITY} {plusSimbol}");
                break;
            case CharacteristicsName.VehicleHullHP:
                ShowEventText(upgradeCardData.ChangeValue, $"{TextConstants.HULLHP} {plusSimbol}");
                break;
            case CharacteristicsName.VehicleShieldHP:
                ShowEventText(upgradeCardData.ChangeValue, $"{TextConstants.SHIELDHP} {plusSimbol}");
                break;
            case CharacteristicsName.VehicleShieldRegRate:
                ShowEventText(upgradeCardData.ChangeValue, $"{TextConstants.SHIELREGENRATE} {plusSimbol}");
                break;
            default:
                break;
        }
    }



    //void Update()
    //{
    //    //On leftclick.
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        ShowEventText(0.5f, "awdawd");
    //    }
    //}
}

public static class DamageNumberExention
{
    public static DamageNumber ShowEventTextNumberAndLeftText(this DamageNumber damageNumber, RectTransform rectParent, Vector2 anchoredPosition, float number, string text)
    {
        DamageNumber newDN = damageNumber.Spawn();


        newDN.SetAnchoredPosition(rectParent, anchoredPosition);

        newDN.number = number;
        newDN.enableNumber = number != 0;
        newDN.leftText = text;

        newDN.enableLeftText = true;
        return newDN;
    }
}