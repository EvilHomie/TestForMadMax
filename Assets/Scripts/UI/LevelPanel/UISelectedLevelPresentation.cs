using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISelectedLevelPresentation : MonoBehaviour
{
    public static UISelectedLevelPresentation Instance;
    [SerializeField] GameObject[] _difficulSculls;
    [SerializeField] TextMeshProUGUI _levelNumberText;
    [SerializeField] Image _levelImage;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void SetNewLevelPresentation(UILevelInfo uILevelInfo)
    {
        _levelImage.sprite = uILevelInfo.LevelImage.sprite;
        _levelNumberText.text = uILevelInfo.LevelName;

        //string[] levelNameParts = uILevelInfo.LevelName.Split('-');
        //int levelDifficult = int.Parse(levelNameParts[1]);

        //for (int i = 1; i <= _difficulSculls.Length; i++)
        //{
        //    if (i <= levelDifficult)
        //    {
        //        _difficulSculls[i - 1].SetActive(true);
        //    }
        //    else
        //    {
        //        _difficulSculls[i - 1].SetActive(false);
        //    }
        //}
    }

}
