using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickableArea : MonoBehaviour/*, IPointerClickHandler*/
{
    //[SerializeField] TutorialManagerV1 _tutorialManager;
    //Image _tutorialMaskImage;
    //List<GameObject> _tutorialTargetObjects;
    ////[SerializeField ] Image _blockRaycastImage;

    //public List<GameObject> TutorialTargetObjects { get => _tutorialTargetObjects; set => _tutorialTargetObjects = value; }
    //private void Start()
    //{
    //    _tutorialMaskImage = GetComponent<Image>();
    //    _tutorialMaskImage.alphaHitTestMinimumThreshold = 1;
    //}



    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    //_blockRaycastImage.raycastTarget = false;
    //    //_tutorialMaskImage.raycastTarget = false;

    //    List<RaycastResult> raycastResults = new();
    //    EventSystem.current.RaycastAll(eventData, raycastResults);

    //    if (raycastResults.Count > 0)
    //    {
    //        CheckConfirmationStageObjects(raycastResults, out GameObject matchedObject);
    //        if (matchedObject != null )
    //        {
    //            OnCkickRequiredElement(eventData, matchedObject);
    //        }
    //    }
    //    else return;
    //}

    //void CheckConfirmationStageObjects(List<RaycastResult> raycastResults, out GameObject matchedObject)
    //{
    //    Debug.LogWarning("CHECK");
    //    matchedObject = null;
    //    foreach (RaycastResult raycastResult in raycastResults)
    //    {
    //        foreach (GameObject targetObject in _tutorialTargetObjects)
    //        {
    //            if (raycastResult.gameObject == targetObject)
    //            {
    //                matchedObject = targetObject;
    //                break;
    //            }
    //            else
    //            {
    //                continue;
    //            }
    //        }
    //    }
    //}


    //void OnCkickRequiredElement(PointerEventData eventData, GameObject matchedObject)
    //{
    //    Debug.LogWarning("REPEAT CLICK");
    //    ExecuteEvents.Execute(matchedObject, eventData, ExecuteEvents.pointerClickHandler);
    //    _tutorialManager.OnPassStage();
    //}
    ////void OnClickInsideMaskZone(PointerEventData eventData)
    ////{
    ////    List<RaycastResult> raycastResults = new();
    ////    EventSystem.current.RaycastAll(eventData, raycastResults);

    ////    RaycastResult result = raycastResults.Find(obj => 1 << obj.gameObject.layer == InteractionLayerMask.value && obj.gameObject.GetComponent<TMP_SelectionCaret>() == null);

    ////    if (result.gameObject != null)
    ////    {
    ////        Debug.Log(result.gameObject.name);
    ////        ExecuteEvents.Execute(result.gameObject, eventData, ExecuteEvents.submitHandler);
    ////    }
    ////    Debug.Log(raycastResults.Count);
    ////}
}
