using System.Collections.Generic;
using UnityEngine;

public class ResourcesInVehicle : MonoBehaviour
{
    [SerializeField] List<DropedResource> _resources;




    public void DropResources()
    {
        if (_resources.Count == 0) return;

        int copperAmount = 0;
        int wiresAmount = 0;
        int scrapMetalAmount = 0;

        foreach (DropedResource resource in _resources)
        {
            for (int i = 0; i <= resource.amount; i++)
            {
                float chance = Random.Range(0, 100f);
                if (resource.dropChance < chance) continue;


                if (resource.type == ResourcesType.Ñopper)
                {
                    copperAmount++;
                }
                else if (resource.type == ResourcesType.Wires)
                {
                    wiresAmount++;
                }
                else if (resource.type == ResourcesType.ScrapMetal)
                {
                    scrapMetalAmount++;
                }
            }
        }

        PlayerResourcesManager.Instance.AddResources(copperAmount, wiresAmount, scrapMetalAmount);
    }
}
