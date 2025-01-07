using System.Collections;
using UnityEngine;

public class GarageMainPanleAnimation : MonoBehaviour
{
    [SerializeField] AnimationCurve _animationCurve;
    [SerializeField] Vector3 _hideOffset;
    [SerializeField] float _hideDuration;

    Vector3 _deffPos;

    private void Start()
    {
        _deffPos = transform.localPosition;
    }
    public void ResetPosition()
    {
        transform.localPosition = _deffPos;
        gameObject.SetActive(true);
    }

    public void HidePanel()
    {
        //transform.localPosition = Vector3.zero;
        StartCoroutine(HidePanelCoroutine());
    }


    IEnumerator HidePanelCoroutine()
    {
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / _hideDuration;

            transform.localPosition = _hideOffset * _animationCurve.Evaluate(t);
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
