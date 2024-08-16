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
            for (int i = 0; i <= resource.Amount; i++)
            {
                float chance = Random.Range(0, 100f);
                if (resource.DropChance < chance) continue;


                if (resource.ResourcesType == ResourcesType.Ñopper)
                {
                    copperAmount++;
                }
                else if (resource.ResourcesType == ResourcesType.Wires)
                {
                    wiresAmount++;
                }
                else if (resource.ResourcesType == ResourcesType.ScrapMetal)
                {
                    scrapMetalAmount++;
                }
            }
        }

        ResourcesManager.Instance.AddResources(copperAmount, wiresAmount, scrapMetalAmount);
    }
}
