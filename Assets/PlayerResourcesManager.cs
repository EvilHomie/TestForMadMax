using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerResourcesManager : MonoBehaviour
{
    public static PlayerResourcesManager Instance;

    Dictionary<ResourcesType, int> _resources = new();

    [SerializeField] TextMeshProUGUI _copperAmountText;
    [SerializeField] TextMeshProUGUI _wiresAmountText;
    [SerializeField] TextMeshProUGUI _scrapMetalAmountText;

    [SerializeField] TextMeshProUGUI _copperAddText;
    [SerializeField] TextMeshProUGUI _wiresAddText;
    [SerializeField] TextMeshProUGUI _scrapMetalAddText;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    private void Start()
    {
        UpdateCounters();
        DisableAddTexts();
    }

    void UpdateCounters()
    {
        _copperAmountText.text = _resources.ContainsKey(ResourcesType.Ñopper) ? _resources[ResourcesType.Ñopper].ToString() : 0.ToString();
        _wiresAmountText.text = _resources.ContainsKey(ResourcesType.Wires) ? _resources[ResourcesType.Wires].ToString() : 0.ToString();
        _scrapMetalAmountText.text = _resources.ContainsKey(ResourcesType.ScrapMetal) ? _resources[ResourcesType.ScrapMetal].ToString() : 0.ToString();
    }

    void DisableAddTexts()
    {
        _copperAddText.text = string.Empty;
        _wiresAddText.text = string.Empty;
        _scrapMetalAddText.text = string.Empty;
    }

    void AnimateAddTexts(int copperAmount, int wiresAmount, int scrapMetalAmount)
    {
        StartCoroutine(AddTextsAnimation(copperAmount, wiresAmount, scrapMetalAmount));
    }

    IEnumerator AddTextsAnimation(int copperAmount, int wiresAmount, int scrapMetalAmount)
    {
        float t = 0f;

        _copperAddText.text = copperAmount.ToString();
        _wiresAddText.text = wiresAmount.ToString();
        _scrapMetalAddText.text = scrapMetalAmount.ToString();

        while (t <1) 
        {
            t += Time.deltaTime;

            _copperAddText.color = Color.Lerp(Color.clear, Color.green, t);
            _wiresAddText.color = Color.Lerp(Color.clear, Color.green, t);
            _scrapMetalAddText.color = Color.Lerp(Color.clear, Color.green, t);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        _copperAddText.color = Color.clear;
        _wiresAddText.color = Color.clear;
        _scrapMetalAddText.color = Color.clear;
    }

    public void AddResources(int copperAmount, int wiresAmount, int scrapMetalAmount)
    {
        if (copperAmount != 0)
        {
            if (!_resources.ContainsKey(ResourcesType.Ñopper))
                _resources.Add(ResourcesType.Ñopper, copperAmount);
            else
                _resources[ResourcesType.Ñopper] += copperAmount;
        }

        if (wiresAmount != 0)
        {
            if (!_resources.ContainsKey(ResourcesType.Wires))
                _resources.Add(ResourcesType.Wires, wiresAmount);
            else
                _resources[ResourcesType.Wires] += wiresAmount;
        }

        if (scrapMetalAmount != 0)
        {
            if (!_resources.ContainsKey(ResourcesType.ScrapMetal))
                _resources.Add(ResourcesType.ScrapMetal, scrapMetalAmount);
            else
                _resources[ResourcesType.ScrapMetal] += scrapMetalAmount;
        }

        UpdateCounters();
        AnimateAddTexts(copperAmount, wiresAmount, scrapMetalAmount);
    }
}
