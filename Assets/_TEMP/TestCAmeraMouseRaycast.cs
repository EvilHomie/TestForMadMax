using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class TestCAmeraMouseRaycast : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                DetachLogic reboundLogic = hit.collider.gameObject.GetComponent<DetachLogic>();
                reboundLogic?.Detach();
            }
        }
        
    }
}
