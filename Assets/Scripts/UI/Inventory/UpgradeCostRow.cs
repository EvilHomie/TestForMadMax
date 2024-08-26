using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCostRow : MonoBehaviour
{
    [SerializeField] Image _resourcesImage;
    [SerializeField] TextMeshProUGUI _updateCostText;

    public void SetData(Sprite resSprite, int price, Color color)
    {
        gameObject.SetActive(true);
        _resourcesImage.sprite = resSprite;
        _updateCostText.text = $" X {price}";
        _updateCostText.color = color;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
