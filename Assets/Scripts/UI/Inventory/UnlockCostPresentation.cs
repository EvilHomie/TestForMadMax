using TMPro;
using UnityEngine;

public class UnlockCostPresentation : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _scrapMetalCost;
    [SerializeField] TextMeshProUGUI _wiresCost;
    [SerializeField] TextMeshProUGUI _cooperCost;

    public void SetData(int scrapMCost, int wireCost, int cooperCost)
    {
        _scrapMetalCost.text = $"x {scrapMCost}";
        _wiresCost.text = $"x {wireCost}";
        _cooperCost.text = $"x {cooperCost}";
    }
}
