using System.Collections.Generic;
using UnityEngine;

public class LevelProgressPresentation : MonoBehaviour
{
    [SerializeField] Transform _elementHolder;
    [SerializeField] LevelProgressElement _elementPF;

    [SerializeField] Sprite _notFilledElement;
    [SerializeField] Sprite _filledElement;
    [SerializeField] Sprite _bossEvent;


    public void ConfigPanel(List<LevelParameters> chapterLevelsParameters, int unlockLevelsInChapterAmount, int selectedLevelNumber)
    {
        int comlitedLevelsAmount;
        if (unlockLevelsInChapterAmount >= chapterLevelsParameters.Count)
        {
            comlitedLevelsAmount = unlockLevelsInChapterAmount;
        }
        else
        {
            comlitedLevelsAmount = unlockLevelsInChapterAmount - 1;
        }

        foreach (Transform child in _elementHolder)
        {
            Destroy(child.gameObject);
        }


        for (int i = 1; i <= chapterLevelsParameters.Count; i++)
        {
            LevelProgressElement element = Instantiate(_elementPF, _elementHolder);

            if (i <= comlitedLevelsAmount)
            {
                if (chapterLevelsParameters[i - 1].Boss != null)
                {
                    element.ConfigElement(_filledElement, _bossEvent);
                }
                else
                {
                    element.ConfigElement(_filledElement);
                }
            }            
            else
            {
                if (chapterLevelsParameters[i - 1].Boss != null)
                {
                    element.ConfigElement(_notFilledElement, _bossEvent);
                }
                else
                {
                    element.ConfigElement(_notFilledElement);
                }
            }

            if (i == selectedLevelNumber)
            {
                element.SelectedlevelIcon.gameObject.SetActive(true);
            }

        }

        //for (int i = 0; i < _elementsImages.Length; i++)
        //{
        //    if (i < selectedLevelNumber)
        //    {
        //        _elementsImages[i].sprite = _filledElement;
        //    }
        //    else
        //    {
        //        _elementsImages[i].sprite = _notFilledElement;
        //    }
        //}
    }
}
