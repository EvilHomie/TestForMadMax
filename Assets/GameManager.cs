using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Button _startRaidBtn;
    [SerializeField] Button _garageBtn;


    private void Start()
    {
        _startRaidBtn.onClick.AddListener(StartRaid);
    }



    public void StartRaid()
    {
        PlayerVehicleManager.Instance.StartVehicle();
    }
   

}
