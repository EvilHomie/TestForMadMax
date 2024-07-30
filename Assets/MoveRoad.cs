using UnityEngine;

public class MoveRoad : MonoBehaviour
{
    [SerializeField] MeshRenderer _mainRoadRenderer;
    //[SerializeField] MeshRenderer _sideRoadRenderer;

    [SerializeField] float _changeOffsetSpeed = 1;
    void FixedUpdate()
    {
        ChangeRendererMaterialOffset();
    }

    void ChangeRendererMaterialOffset()
    {
        _mainRoadRenderer.material.mainTextureOffset += _changeOffsetSpeed * Manager.GlobalMoveSpeed * Time.fixedDeltaTime * Vector2.left;
        //_sideRoadRenderer.material.mainTextureOffset += _changeOffsetSpeed * Manager.GlobalMoveSpeed /2 * Time.fixedDeltaTime * Vector2.left;
    }
}
