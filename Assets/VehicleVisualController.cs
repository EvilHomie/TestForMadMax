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
            wheel.Rotate(GameLogicParameters.Instance.WheelsRotateSpeedMod * RaidManager.Instance.PlayerMoveSpeed * Time.deltaTime, 0, 0, Space.Self);
        }
    }

    public void UpdateVisualEffect()
    {
        foreach (var dust in _wheelsDustPS)
        {
            if (dust == null) continue;
            var main = dust.main;
            main.startSpeed = GameLogicParameters.Instance.SpeedMod * RaidManager.Instance.PlayerMoveSpeed;

            var emmision = dust.emission;
            emmision.rateOverTime = GameLogicParameters.Instance.DustPSEmmisionRateMod * RaidManager.Instance.PlayerMoveSpeed;
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
