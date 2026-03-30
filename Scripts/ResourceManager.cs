using UnityEngine;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour
{
    private Dictionary<string, float> resources = new Dictionary<string, float>();

    public Dictionary<string, float> Resources => resources;

    private void Awake()
    {
        resources["Revenue"] = 0f;
        resources["Reputation"] = 0f;
        resources["RevenuePerSecond"] = 0f;
    }

    public float GetResource(string resourceName)
    {
        if (resources.ContainsKey(resourceName))
        {
            return resources[resourceName];
        }

        return 0f;
    }

    public void AddResource(string resourceName, float amount)
    {
        if (resources.ContainsKey(resourceName))
        {
            resources[resourceName] += amount;
        }
        else
        {
            resources[resourceName] = amount;
        }
    }

    public bool SpendResource(string resourceName, float amount)
    {
        if (resources.ContainsKey(resourceName) && resources[resourceName] >= amount)
        {
            resources[resourceName] -= amount;
            return true;
        }

        return false;
    }

    public void SetResource(string resourceName, float value)
    {
        if (resources.ContainsKey(resourceName))
        {
            resources[resourceName] = value;
        }
        else
        {
            resources.Add(resourceName, value);
        }
    }
}
