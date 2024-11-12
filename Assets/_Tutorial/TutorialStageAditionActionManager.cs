using UnityEngine;

public class TutorialStageAditionActionManager : MonoBehaviour
{
    RectTransform _clickableAreaRT;

    public void Init(RectTransform clickableAreaRT)
    {
        _clickableAreaRT = clickableAreaRT;
    }


    

     

    public void AditionStageActionOnEnable(StageName stageName)
    {
        if (stageName == StageName.UpgradeRotateSpeed)
        {
            GameFlowManager.Instance.Unpause(TutorialManager.Instance);
        }
    }

    public void AditionStageActionOnDisable(StageName stageName)
    {      
        if (stageName == StageName.CloseInventory)
        {
            Cursor.visible = true;
        }  
        else if (stageName == StageName.WishGoodluck)
        {
            Cursor.visible = true;
        }
    }

    
}
