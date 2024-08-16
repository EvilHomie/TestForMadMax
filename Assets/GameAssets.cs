using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets Instance;

    [Header("GAME Items Data")]
    [SerializeField] GameItems _gameItems;
    [SerializeField] UpgradeCosts _upgradeCosts;
    [SerializeField] List<ResSprite> _resSprites;

    public GameItems GameItems => _gameItems;
    public UpgradeCosts UpgradeCosts => _upgradeCosts;
    public List<ResSprite> ResSprites => _resSprites;


    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
        Application.targetFrameRate = 1000;
    }
}
