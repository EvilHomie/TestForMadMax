using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILevelInfo : MonoBehaviour
{
    ScrollRect _scrollRect;     

    [SerializeField] Button _selectBtn;
    [SerializeField] Image _levelImage;
    [SerializeField] Image _isSelectedImage;
    [SerializeField] TextMeshProUGUI _unlockStatusText;
    [SerializeField] TextMeshProUGUI _levelNumberText;
    LevelParameters _levelParameters;
    public Button SelectBtn => _selectBtn;
    public LevelParameters LevelParameters => _levelParameters;

    public void Init(ScrollRect scrollRect, LevelParameters levelParameters)
    {
        _levelParameters = levelParameters;
        _scrollRect = scrollRect;
        _levelImage.sprite = levelParameters.LevelImage;
        _levelNumberText.text = levelParameters.LevelName;

        Deselect();
        if (PlayerData.Instance.UnlockedLevelsNames.Contains(_levelParameters.LevelName))
        {
            UnlockLevel();
        }
        else LockLevel();
    }

    public void Select()
    {
        _isSelectedImage.gameObject.SetActive(true);
    }

    public void Deselect()
    {
        _isSelectedImage.gameObject.SetActive(false);
    }

    public void LockLevel()
    {
        _unlockStatusText.text = TextConstants.LOCKED;
        _unlockStatusText.color = Color.red;
    }

    public void UnlockLevel()
    {
        _unlockStatusText.text = TextConstants.UNLOCKED;
        _unlockStatusText.color = Color.green;
    }

    public IEnumerator Shake(float duration, float shakeIntensity)
    {
        _scrollRect.vertical = false;
        Vector3 defPos = transform.position;
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            Vector3 randomOffset = Random.insideUnitSphere * shakeIntensity;
            transform.position = defPos + randomOffset;
            yield return null;
        }
        transform.position = defPos;
        _scrollRect.vertical = true;
    }
}
