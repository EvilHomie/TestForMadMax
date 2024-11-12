using UnityEngine;

public class TutorialBlockRayCastPart : MonoBehaviour
{
    [SerializeField] BlockRayCastPart _blockPart;
    RectTransform _rectTransform;
    public BlockRayCastPart BlockPart => _blockPart;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }
    public RectTransform RectTransform => _rectTransform;
}
