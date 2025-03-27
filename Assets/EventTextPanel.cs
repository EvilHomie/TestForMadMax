using DamageNumbersPro;
using UnityEngine;

public class EventTextPanel : MonoBehaviour
{
    [SerializeField] DamageNumber _numberPrefab;
    [SerializeField] RectTransform _rectParent;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Spawn new popup with a random number between 0 and 100.
            DamageNumber damageNumber = _numberPrefab.SpawnGUI(_rectParent, Vector2.zero, Random.Range(1, 100));
        }
    }
}
