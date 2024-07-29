using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIResourcesManager : MonoBehaviour
{
    public static UIResourcesManager Instance;
    [SerializeField] TextMeshProUGUI _copperAmountText;
    [SerializeField] TextMeshProUGUI _wiresAmountText;
    [SerializeField] TextMeshProUGUI _scrapMetalAmountText;

    [SerializeField] TextMeshProUGUI _copperAddText;
    [SerializeField] TextMeshProUGUI _wiresAddText;
    [SerializeField] TextMeshProUGUI _scrapMetalAddText;

    [Header("If Not Enough")]
    [SerializeField] float _shakeDuration = 0.5f;
    [SerializeField] float _shakeIntensity = 0.2f;

    Vector3 _deffPos;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void Init()
    {
        _deffPos = transform.position;
        UpdateCounters();
        DisableAddTexts();
    }

    public void UpdateCounters()
    {
        _copperAmountText.text = PlayerData.Instance.AvailableResources.ContainsKey(ResourcesType.Ñopper) ? PlayerData.Instance.AvailableResources[ResourcesType.Ñopper].ToString() : 0.ToString();
        _wiresAmountText.text = PlayerData.Instance.AvailableResources.ContainsKey(ResourcesType.Wires) ? PlayerData.Instance.AvailableResources[ResourcesType.Wires].ToString() : 0.ToString();
        _scrapMetalAmountText.text = PlayerData.Instance.AvailableResources.ContainsKey(ResourcesType.ScrapMetal) ? PlayerData.Instance.AvailableResources[ResourcesType.ScrapMetal].ToString() : 0.ToString();
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

        _copperAddText.text = $"+ {copperAmount}";
        _wiresAddText.text = $"+ {wiresAmount}";
        _scrapMetalAddText.text = $"+ {scrapMetalAmount}";

        while (t < 1)
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

    public void RemoveAllResources()
    {
        PlayerData.Instance.AvailableResources = new Dictionary<ResourcesType, int>()
            {
                { ResourcesType.ScrapMetal,0},
                { ResourcesType.Wires,0},
                { ResourcesType.Ñopper,0}
            };
        UpdateCounters();
    }

    public void AddResources(int copperAmount, int wiresAmount, int scrapMetalAmount)
    {
        if (copperAmount != 0)
        {
            if (!PlayerData.Instance.AvailableResources.ContainsKey(ResourcesType.Ñopper))
                PlayerData.Instance.AvailableResources.Add(ResourcesType.Ñopper, copperAmount);
            else
                PlayerData.Instance.AvailableResources[ResourcesType.Ñopper] += copperAmount;
        }

        if (wiresAmount != 0)
        {
            if (!PlayerData.Instance.AvailableResources.ContainsKey(ResourcesType.Wires))
                PlayerData.Instance.AvailableResources.Add(ResourcesType.Wires, wiresAmount);
            else
                PlayerData.Instance.AvailableResources[ResourcesType.Wires] += wiresAmount;
        }

        if (scrapMetalAmount != 0)
        {
            if (!PlayerData.Instance.AvailableResources.ContainsKey(ResourcesType.ScrapMetal))
                PlayerData.Instance.AvailableResources.Add(ResourcesType.ScrapMetal, scrapMetalAmount);
            else
                PlayerData.Instance.AvailableResources[ResourcesType.ScrapMetal] += scrapMetalAmount;
        }

        UpdateCounters();
        AnimateAddTexts(copperAmount, wiresAmount, scrapMetalAmount);
        UILevelStatistic.Instance.OnCollectResources(scrapMetalAmount, wiresAmount, copperAmount);
    }

    public bool TrySpendResources(int scrapMetalAmount, int wiresAmount, int copperAmount)
    {
        if (!CheckResources(scrapMetalAmount, wiresAmount, copperAmount))
        {
            StartCoroutine(Shake(_shakeDuration));
            return false;
        }

        PlayerData.Instance.AvailableResources[ResourcesType.Ñopper] -= copperAmount;
        PlayerData.Instance.AvailableResources[ResourcesType.Wires] -= wiresAmount;
        PlayerData.Instance.AvailableResources[ResourcesType.ScrapMetal] -= scrapMetalAmount;
        UpdateCounters();
        return true;
    }

    IEnumerator Shake(float duration)
    {
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            Vector3 randomOffset = Random.insideUnitSphere * _shakeIntensity;
            transform.position = _deffPos + randomOffset;
            yield return null;
        }
        transform.position = _deffPos;
    }

    bool CheckResources(int scrapMetalAmount, int wiresAmount, int copperAmount)
    {
        if (copperAmount != 0)
        {
            if (!PlayerData.Instance.AvailableResources.ContainsKey(ResourcesType.Ñopper)) return false;
            if (PlayerData.Instance.AvailableResources[ResourcesType.Ñopper] < copperAmount) return false;
        }

        if (wiresAmount != 0)
        {
            if (!PlayerData.Instance.AvailableResources.ContainsKey(ResourcesType.Wires)) return false;
            if (PlayerData.Instance.AvailableResources[ResourcesType.Wires] < wiresAmount) return false;
        }

        if (scrapMetalAmount != 0)
        {
            if (!PlayerData.Instance.AvailableResources.ContainsKey(ResourcesType.ScrapMetal)) return false;
            if (PlayerData.Instance.AvailableResources[ResourcesType.ScrapMetal] < scrapMetalAmount) return false;
        }
        return true;
    }
}
