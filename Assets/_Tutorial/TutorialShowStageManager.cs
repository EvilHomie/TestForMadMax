using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialShowStageManager : MonoBehaviour
{
    RectTransform _tutorialArrowRT;
    RectTransform _canvasRT;
    RectTransform _tutorialTextRT;
    TutorialTextPanel _tutorialTextPanel;
    RectTransform _clickableAreaRT;
    Image _clickableAreaBlockRaycastImage;
    Button _fullScreenConfirmButton;

    float _transformationDuration = 1f;
    float _scaleFactor;
    public void Init(RectTransform tutorialArrow, TutorialTextPanel tutorialTextPanel, RectTransform clickableAreaRT, Image clickableAreaBlockRaycastImage, float transformationDuration, Button fullScreenConfirmButton)
    {
        _tutorialTextPanel = tutorialTextPanel;
        _tutorialArrowRT = tutorialArrow;
        _tutorialTextRT = tutorialTextPanel.GetComponent<RectTransform>();
        _clickableAreaRT = clickableAreaRT;
        _clickableAreaBlockRaycastImage = clickableAreaBlockRaycastImage;
        _canvasRT = transform.root.GetComponent<RectTransform>();
        _scaleFactor = _canvasRT.localScale.x;
        _transformationDuration = transformationDuration;
        _fullScreenConfirmButton = fullScreenConfirmButton;


        _tutorialArrowRT.gameObject.SetActive(false);
        _tutorialTextRT.gameObject.SetActive(false);
        _fullScreenConfirmButton.gameObject.SetActive(false);
    }

    public void ConfigureStage(TutorialStage stage)
    {
        _fullScreenConfirmButton.gameObject.SetActive(false);
        _clickableAreaRT.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(AreaTransformation(stage));
        StartCoroutine(ConfigureTextAndArrow(stage));
    }

    void ConfigureTutorialArrow(TutorialStage stage)
    {
        if (stage.withArrow)
        {
            Vector3 localPos = (Vector3)_clickableAreaRT.rect.size / 2 + (Vector3)_tutorialArrowRT.rect.size / 2;
            Vector3 offset = localPos + (Vector3)_tutorialArrowRT.rect.size / 2;

            if (stage.arrowSidePositions == SidePositions.TopRight)
            {
                _tutorialArrowRT.localScale = new Vector3(1, 1, 1);
                _tutorialArrowRT.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else if (stage.arrowSidePositions == SidePositions.BottomRight)
            {
                localPos.y = -localPos.y;
                offset.y = -offset.y;
                _tutorialArrowRT.localScale = new Vector3(1, -1, 1);
                _tutorialArrowRT.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else if (stage.arrowSidePositions == SidePositions.TopLeft)
            {
                localPos.x = -localPos.x;
                offset.x = -offset.x;
                _tutorialArrowRT.localScale = new Vector3(-1, 1, 1);
                _tutorialArrowRT.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else if (stage.arrowSidePositions == SidePositions.BottomLeft)
            {
                localPos.x = -localPos.x;
                localPos.y = -localPos.y;
                offset.x = -offset.x;
                offset.y = -offset.y;
                _tutorialArrowRT.localScale = new Vector3(-1, -1, 1);
                _tutorialArrowRT.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else if (stage.arrowSidePositions == SidePositions.MidleLeft)
            {
                localPos.x = -localPos.x;
                localPos.y = 0;
                offset.x = -offset.x;
                offset.y = 0;
                _tutorialArrowRT.localScale = new Vector3(1, 1, 1);
                _tutorialArrowRT.localRotation = Quaternion.Euler(0, 0, 135f);
            }
            else if (stage.arrowSidePositions == SidePositions.MidleRight)
            {
                localPos.y = 0;
                offset.y = 0;
                _tutorialArrowRT.localScale = new Vector3(1, 1, 1);
                _tutorialArrowRT.localRotation = Quaternion.Euler(0, 0, -45f);
            }
            else if (stage.arrowSidePositions == SidePositions.TopMidle)
            {
                localPos.x = 0;
                offset.x = 0;
                _tutorialArrowRT.localScale = new Vector3(1, 1, 1);
                _tutorialArrowRT.localRotation = Quaternion.Euler(0, 0, 45f);
            }


            _tutorialArrowRT.localPosition = localPos;
            _tutorialArrowRT.gameObject.SetActive(true);

            StartCoroutine(ArrowAnimation(localPos, offset));
        }
        else
        {
            _tutorialArrowRT.gameObject.SetActive(false);
        }
    }

    void ConfigureTutorialText(TutorialStage stage)
    {
        string text = TextConstants.TUTORIALSTAGESTEXTS[stage.stageName];


        if (!string.IsNullOrEmpty(text))
        {
            _tutorialTextPanel.SetText(text);
            _tutorialTextPanel.gameObject.SetActive(true);

            if (stage.textPivotPositions == SidePositions.TopRight)
            {
                _tutorialTextRT.pivot = new Vector2(1, 1);
                _tutorialTextRT.anchorMin = new Vector2(1, 1);
                _tutorialTextRT.anchorMax = new Vector2(1, 1);
                _tutorialTextRT.localPosition = new Vector3(_canvasRT.sizeDelta.x / 2, _canvasRT.sizeDelta.y / 2, 0) + (Vector3)stage.textPanelAditionOffset;
            }
            else if (stage.textPivotPositions == SidePositions.BottomRight)
            {
                _tutorialTextRT.pivot = new Vector2(1, 0);
                _tutorialTextRT.anchorMin = new Vector2(1, 0);
                _tutorialTextRT.anchorMax = new Vector2(1, 0);
                _tutorialTextRT.localPosition = new Vector3(_canvasRT.sizeDelta.x / 2, -_canvasRT.sizeDelta.y / 2, 0) + (Vector3)stage.textPanelAditionOffset;
            }
            else if (stage.textPivotPositions == SidePositions.TopLeft)
            {
                _tutorialTextRT.pivot = new Vector2(0, 1);
                _tutorialTextRT.anchorMin = new Vector2(0, 1);
                _tutorialTextRT.anchorMax = new Vector2(0, 1);
                _tutorialTextRT.localPosition = new Vector3(-_canvasRT.sizeDelta.x / 2, _canvasRT.sizeDelta.y / 2, 0) + (Vector3)stage.textPanelAditionOffset;
            }
            else if (stage.textPivotPositions == SidePositions.BottomLeft)
            {
                _tutorialTextRT.pivot = new Vector2(0, 0);
                _tutorialTextRT.anchorMin = Vector2.zero;
                _tutorialTextRT.anchorMax = Vector2.zero;
                _tutorialTextRT.localPosition = new Vector3(-_canvasRT.sizeDelta.x / 2, -_canvasRT.sizeDelta.y / 2, 0) + (Vector3)stage.textPanelAditionOffset;
            }
            else if (stage.textPivotPositions == SidePositions.ScreenCenter)
            {
                _tutorialTextRT.pivot = new Vector2(0.5f, 0.5f);
                _tutorialTextRT.anchorMin = new Vector2(0.5f, 0.5f);
                _tutorialTextRT.anchorMax = new Vector2(0.5f, 0.5f);
                _tutorialTextRT.localPosition = (Vector3)stage.textPanelAditionOffset;
            }
            else if (stage.textPivotPositions == SidePositions.BottomMidle)
            {
                _tutorialTextRT.pivot = new Vector2(0.5f, 0f);
                _tutorialTextRT.anchorMin = new Vector2(0.5f, 0f);
                _tutorialTextRT.anchorMax = new Vector2(0.5f, 0f);
                _tutorialTextRT.localPosition = (Vector3)stage.textPanelAditionOffset;
            }

            _fullScreenConfirmButton.gameObject.SetActive(!stage.stageConfirmViaScript);
        }
        else
        {
            _tutorialTextPanel.gameObject.SetActive(false);
        }
        _tutorialTextPanel.ConfirmationButton.gameObject.SetActive(!stage.stageConfirmViaScript);

    }

    public void OnPassStage()
    {
        StopAllCoroutines();
        _tutorialArrowRT.gameObject.SetActive(false);
        _tutorialTextRT.gameObject.SetActive(false);
        _clickableAreaRT.gameObject.SetActive(false);
        _tutorialTextPanel.ConfirmationButton.gameObject.SetActive(false);
    }

    IEnumerator ArrowAnimation(Vector3 startPos, Vector3 endPos)
    {
        while (true)
        {
            float t = (Mathf.Cos(Time.unscaledTime * 5) + 1) / 8;
            _tutorialArrowRT.transform.localPosition = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
    }

    IEnumerator AreaTransformation(TutorialStage stage)
    {
        RectTransform targetRT = stage.selectedUIElement;
        if (targetRT == null)
        {
            _clickableAreaRT.position = Vector3.zero;
            _clickableAreaRT.sizeDelta = Vector3.zero;
            yield break;
        }
        _clickableAreaRT.pivot = targetRT.pivot;
        _clickableAreaRT.anchorMin = targetRT.pivot;
        _clickableAreaRT.anchorMax = targetRT.pivot;
        _clickableAreaRT.position = _canvasRT.position;
        _clickableAreaRT.sizeDelta = _canvasRT.rect.size;

        _clickableAreaBlockRaycastImage.enabled = true;
        Vector3 startPos = _clickableAreaRT.position;
        Vector2 startSize = _clickableAreaRT.sizeDelta;
        float t = 0;
        while (t <= 1)
        {
            yield return new WaitForEndOfFrame();
            t += Time.unscaledDeltaTime / _transformationDuration;
            _clickableAreaRT.position = Vector3.Lerp(startPos, targetRT.position, t);
            _clickableAreaRT.sizeDelta = Vector3.Lerp(startSize, targetRT.rect.size, t);
        }
        _clickableAreaBlockRaycastImage.enabled = stage.blockRaycastInCenter;

        while (true)
        {
            _clickableAreaRT.position = targetRT.position;
            yield return null;
        }
    }

    IEnumerator ConfigureTextAndArrow(TutorialStage stage)
    {
        if (stage.selectedUIElement != null)
        {
            yield return new WaitForSecondsRealtime(_transformationDuration);
        }
        ConfigureTutorialArrow(stage);
        ConfigureTutorialText(stage);
    }
}
