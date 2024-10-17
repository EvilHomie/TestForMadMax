using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIChangeLevelButton : MonoBehaviour
{
    public static UIChangeLevelButton Instance;
    [SerializeField] TextMeshProUGUI _levelNumberText;
    [SerializeField] Image _levelImage;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void UpdateSelectedLevel(UILevelInfo uILevelInfo)
    {
        _levelImage.sprite = uILevelInfo.LevelParameters.LevelImage;
        _levelNumberText.text = uILevelInfo.LevelParameters.LevelName;
    }
}
