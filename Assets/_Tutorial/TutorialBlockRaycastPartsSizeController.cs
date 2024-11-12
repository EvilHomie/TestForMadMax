using System.Collections.Generic;
using UnityEngine;

public class TutorialBlockRaycastPartsSizeController : MonoBehaviour
{
    [SerializeField] RectTransform _blockRaycastPart;
    [SerializeField] List<TutorialBlockRayCastPart> _tutorialBlockRayCastPart;

    float _scaleFactor;
    RectTransform _canvasRT;
    Vector2 _size;
    float _topBorderPos;
    float _bottomBorderPos;
    float _leftBorderPos;
    float _rightBorderPos;
    private void Awake()
    {
        _canvasRT = GetComponent<RectTransform>();
    }
    private void Update()
    {
        _scaleFactor = _canvasRT.localScale.x;
        CalculateBorderPositionsInWorld();
        AdaptBlockPartsSizes();
    }

    void CalculateBorderPositionsInWorld()
    {
        _size = _blockRaycastPart.rect.size;
        _topBorderPos = _blockRaycastPart.position.y / _scaleFactor + _size.y / 2;
        _bottomBorderPos = _blockRaycastPart.position.y / _scaleFactor - _size.y / 2;
        _leftBorderPos = _blockRaycastPart.position.x / _scaleFactor - _size.x / 2;
        _rightBorderPos = _blockRaycastPart.position.x / _scaleFactor + _size.x / 2;
    }

    void AdaptBlockPartsSizes()
    {
        foreach (var blockPart in _tutorialBlockRayCastPart)
        {
            if (blockPart.BlockPart == BlockRayCastPart.RightPart)
            {
                float posY = _blockRaycastPart.transform.position.y;
                blockPart.RectTransform.position = new(blockPart.RectTransform.position.x, posY, blockPart.RectTransform.position.z);

                float width = _canvasRT.rect.width - _rightBorderPos;
                blockPart.RectTransform.sizeDelta = new(width, _size.y);
            }
            else if (blockPart.BlockPart == BlockRayCastPart.LeftPart)
            {
                float posY = _blockRaycastPart.transform.position.y;
                blockPart.RectTransform.position = new(blockPart.RectTransform.position.x, posY, blockPart.RectTransform.position.z);
                float width = _leftBorderPos;
                blockPart.RectTransform.sizeDelta = new(width, _size.y);
            }
            else if (blockPart.BlockPart == BlockRayCastPart.TopPart)
            {
                float height = _canvasRT.rect.height - _topBorderPos;
                blockPart.RectTransform.sizeDelta = new(_canvasRT.rect.width, height);
            }
            else if (blockPart.BlockPart == BlockRayCastPart.BottomPart)
            {
                float height = _bottomBorderPos;
                blockPart.RectTransform.sizeDelta = new(_canvasRT.rect.width, height);
            }
        }
    }
}
public enum BlockRayCastPart
{
    RightPart,
    LeftPart,
    BottomPart,
    TopPart
}
