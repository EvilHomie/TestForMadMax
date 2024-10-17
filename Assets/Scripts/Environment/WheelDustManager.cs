using UnityEngine;

public class WheelDustManager : MonoBehaviour
{
    private void Awake()
    {
        EnemyVehicleManager enemyVehicleManager = transform.root.GetComponent<EnemyVehicleManager>();
        enemyVehicleManager.WheelsDustPS.Add(GetComponent<ParticleSystem>());
    }
}
