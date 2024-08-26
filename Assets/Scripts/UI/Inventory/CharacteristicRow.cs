using TMPro;
using UnityEngine;

public class CharacteristicRow : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _charName;
    [SerializeField] TextMeshProUGUI _charValue;

    public void SetData(string charName, string charValue)
    {
        gameObject.SetActive(true);
        _charName.text = charName;
        _charValue.text = charValue;
    }
}
