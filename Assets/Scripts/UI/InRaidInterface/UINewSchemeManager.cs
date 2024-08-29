using System.Collections;
using TMPro;
using UnityEngine;

public class UINewSchemeManager : MonoBehaviour
{
    public static UINewSchemeManager Instance;

    [SerializeField] TextMeshProUGUI _newSchemeText;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void Init()
    {
        _newSchemeText.text = TextConstants.NEWSCHEME;
        _newSchemeText.color = Color.clear;
    }

    public void OnAddNewScheme()
    {
        StartCoroutine(AddTextsAnimation());
    }

    IEnumerator AddTextsAnimation()
    {
        float t = 0f;

        while (t < 1)
        {
            t += Time.deltaTime;

            _newSchemeText.color = Color.Lerp(Color.clear, Color.magenta, t);
            yield return null;
        }

        yield return new WaitForSeconds(2f);
        _newSchemeText.color = Color.clear;
    }
}
