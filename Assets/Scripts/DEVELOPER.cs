using UnityEngine;

public class DEVELOPER : MonoBehaviour
{
    [SerializeField] TestingLogic testingLogic;
    int _Mcount = 0;
    int _Ncount = 0;




    private void Update()
    {
        if (!Input.GetKey(KeyCode.LeftShift) && Input.anyKey)
        {
            _Mcount = 0;
            _Ncount = 0;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.M)) _Mcount++;
            if (Input.GetKeyDown(KeyCode.N)) _Ncount++;

            if (_Mcount == 5 && _Ncount == 5)
            {
                testingLogic.gameObject.SetActive(true);
            }
        }

        
    }
}
