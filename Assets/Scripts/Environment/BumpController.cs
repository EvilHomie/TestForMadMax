using UnityEngine;

public class BumpController : MonoBehaviour
{
    [SerializeField] float _moveSpeed;

    [SerializeField] Transform[] _bumpContainers;

    void FixedUpdate()
    {
        foreach (var bumps in _bumpContainers)
        {
            if (bumps.position.x < -1900)
            {
                bumps.position = new Vector3(1400, bumps.position.y, bumps.position.z);
            }
            else bumps.Translate(_moveSpeed * InRaidManager.Instance.PlayerMoveSpeed * Time.fixedDeltaTime * Vector3.left);
        }
    }
}
