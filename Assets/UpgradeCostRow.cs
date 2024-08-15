using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCostRow : MonoBehaviour
{
    [SerializeField] Image _resourcesImage;
    [SerializeField] TextMeshProUGUI _updateCostText;

    public void SetData(Sprite resSprite, int price)
    {
        gameObject.SetActive(true);
        _resourcesImage.sprite = resSprite;
        _updateCostText.text = $" X {price}";
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
