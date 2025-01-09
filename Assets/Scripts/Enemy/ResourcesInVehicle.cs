using System.Collections.Generic;
using UnityEngine;

public class ResourcesInVehicle : MonoBehaviour
{
    [SerializeField] List<DropedResource> _resources;

    public void DropResources()
    {
        if (InRaidManager.Instance.InSurviveMod) return;
        if (_resources.Count == 0) return;

        int copperAmount = 0;
        int wiresAmount = 0;
        int scrapMetalAmount = 0;

        foreach (DropedResource resource in _resources)
        {
            float chance = Random.Range(resource.DropChance, 100f);
            int amount = (int)(resource.Amount * chance / 100);

            if (resource.ResourcesType == ResourcesType.Ñopper)
            {
                copperAmount = amount;
            }
            else if (resource.ResourcesType == ResourcesType.Wires)
            {
                wiresAmount = amount;
            }
            else if (resource.ResourcesType == ResourcesType.ScrapMetal)
            {
                scrapMetalAmount = amount;
            }
        }

        UIResourcesManager.Instance.AddResources(copperAmount, wiresAmount, scrapMetalAmount);
    }
}
