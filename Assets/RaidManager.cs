using UnityEngine;

public class RaidManager : MonoBehaviour
{
    public static RaidManager Instance;

    [SerializeField] float _playerMoveSpeed = 5f;   

    public float PlayerMoveSpeed => _playerMoveSpeed;
   

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
}
