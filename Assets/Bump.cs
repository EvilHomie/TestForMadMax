using UnityEngine;

public class Bump : MonoBehaviour
{
    [SerializeField] float _moveSpeed;


    Vector3 _startPos;

    private void Start()
    {
        _startPos = transform.position;
    }

    void FixedUpdate()
    {
        if (transform.position.x < -1900)
        {
            transform.position = new Vector3(1200, _startPos.y, _startPos.z);
        }


        transform.Translate(_moveSpeed  * Manager.GlobalMoveSpeed * Time.fixedDeltaTime * Vector3.left);
    }
}
