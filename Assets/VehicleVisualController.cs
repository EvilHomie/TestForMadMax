using UnityEngine;

public class VehicleVisualController : MonoBehaviour
{
    [SerializeField] Transform[] _wheels;
    [SerializeField] ParticleSystem[] _wheelsDustPS;


    public void RotateWheels()
    {
        foreach (var wheel in _wheels)
        {
            if (wheel == null) continue;
            wheel.Rotate(GameConfig.Instance.WheelsRotateSpeedMod * RaidObjectsManager.Instance.PlayerMoveSpeed * Time.deltaTime, 0, 0, Space.Self);
        }
    }

    public void UpdateVisualEffect()
    {
        foreach (var dust in _wheelsDustPS)
        {
            if (dust == null) continue;
            var main = dust.main;
            main.startSpeed = GameConfig.Instance.SpeedMod * RaidObjectsManager.Instance.PlayerMoveSpeed;

            var emmision = dust.emission;
            emmision.rateOverTime = GameConfig.Instance.DustPSEmmisionRateMod * RaidObjectsManager.Instance.PlayerMoveSpeed;
        }
    }

    public void StopVisualEffects()
    {
        foreach (var dust in _wheelsDustPS)
        {
            if (dust == null) continue;
            var emmision = dust.emission;
            emmision.enabled = false;
        }
    }
}
