using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTextPanel : MonoBehaviour
{
    RectTransform _textRT;
    TextMeshProUGUI _stageText;
    Button _confirmationButton;

    public Button ConfirmationButton => _confirmationButton;

    private void Awake()
    {
        _textRT = GetComponent<RectTransform>();
        _stageText = transform.GetComponentInChildren<TextMeshProUGUI>();
        _confirmationButton = transform.GetComponentInChildren<Button>();
    }

    public void SetText(string tutorialStageText)
    {
        string formatingText = Regex.Unescape(tutorialStageText);
        _stageText.text = formatingText;
    }
}
