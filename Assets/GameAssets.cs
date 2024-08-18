using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets Instance;

    [Header("GAME Items Data")]
    [SerializeField] GameItems _gameItems;
    [SerializeField] UpgradeCosts _weaponUpgradeCosts;
    [SerializeField] UpgradeCosts _vehicleUpgradeCosts;
    [SerializeField] UpgradeCosts _vehicleSlotCosts;
    [SerializeField] List<ResSprite> _resSprites;

    public GameItems GameItems => _gameItems;
    public UpgradeCosts WeaponUpgradeCosts => _weaponUpgradeCosts;
    public UpgradeCosts VehicleUpgradeCosts => _vehicleUpgradeCosts;

    public UpgradeCosts VehicleSlotCosts => _vehicleSlotCosts;
    public List<ResSprite> ResSprites => _resSprites;


    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
        Application.targetFrameRate = 1000;
    }
}
