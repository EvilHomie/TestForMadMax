using UnityEngine;

public class Manager : MonoBehaviour
{ 
    [SerializeField] float _globalMoveSpeed;

    public static float GlobalMoveSpeed;

    private void Update()
    {
        GlobalMoveSpeed = _globalMoveSpeed;
    }
}
