using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ResizeGLGCells : MonoBehaviour
{
    void Start()
    {
        GridLayoutGroup GLG = GetComponent<GridLayoutGroup>();
        Canvas.ForceUpdateCanvases();

        float widthRT = GetComponent<RectTransform>().rect.width;
        float paddingSize = GLG.padding.left;
        float cellSpaceSize = GLG.spacing.x;
        int columnCount = GLG.constraintCount;

        float newCellSize = (widthRT - paddingSize * 2 - cellSpaceSize * (columnCount - 1)) / columnCount;
        GLG.cellSize = new Vector2 (newCellSize, newCellSize);
    }

}
