using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Manager
    public ResourceManager resourceManager;

    // UI Text
    public TextMeshProUGUI revenueText;
    public TextMeshProUGUI reputationText;
    public TextMeshProUGUI revenuePerSecondText;
    public TextMeshProUGUI juniorDevCountText;
    public TextMeshProUGUI releasedGamesCountText;

    // Manual Click Values
    public float developGameClickValue = 10f;

    // Passive Income
    public int juniorDeveloperCount = 0;
    public int releasedGameCount = 0;
    public float juniorDeveloperIncome = 2f;
    public float releasedGameIncome = 5f;

    // Costs
    public float juniorDeveloperCost = 50f;
    public float releasedGameCost = 100f;

    private List<Upgrade> upgrades = new List<Upgrade>();

    private float globalRevenueMultiplier = 1f;

    private void Start()
    {
        if (resourceManager == null)
        {
            resourceManager = GetComponent<ResourceManager>();
        }

        upgrades.Add(new Upgrade("Better Engine", 75f, 1.25f));
        upgrades.Add(new Upgrade("Marketing Push", 150f, 1.5f));
        upgrades.Add(new Upgrade("Asset Library", 250f, 2f));

        UpdateRevenuePerSecond();
        UpdateUI();
    }

    private void Update()
    {
        RunPassiveIncome();
        UpdateUI();
    }

    private void RunPassiveIncome()
    {
        float totalPassiveIncome = 0f;

        totalPassiveIncome += juniorDeveloperCount * juniorDeveloperIncome;
        totalPassiveIncome += releasedGameCount * releasedGameIncome;

        totalPassiveIncome *= globalRevenueMultiplier;

        resourceManager.AddResource("Revenue", totalPassiveIncome * Time.deltaTime);
    }

    private void UpdateRevenuePerSecond()
    {
        float totalPassiveIncome = 0f;

        totalPassiveIncome += juniorDeveloperCount * juniorDeveloperIncome;
        totalPassiveIncome += releasedGameCount * releasedGameIncome;

        totalPassiveIncome *= globalRevenueMultiplier;

        resourceManager.SetResource("RevenuePerSecond", totalPassiveIncome);
    }

    private void UpdateUI()
    {
        revenueText.text = "Revenue: $" + resourceManager.GetResource("Revenue").ToString("F1");
        reputationText.text = "Reputation: " + resourceManager.GetResource("Reputation").ToString("F1");
        revenuePerSecondText.text = "Revenue / Sec: $" + resourceManager.GetResource("RevenuePerSecond").ToString("F1");
        juniorDevCountText.text = "Junior Developers: " + juniorDeveloperCount;
        releasedGamesCountText.text = "Released Games: " + releasedGameCount;
    }

    public void DevelopGame()
    {
        float clickAmount = developGameClickValue * globalRevenueMultiplier;
        resourceManager.AddResource("Revenue", clickAmount);
        resourceManager.AddResource("Reputation", 1f);
        Debug.Log("Develop Game clicked. Revenue added: " + clickAmount);
    }

    public void HireJuniorDeveloper()
    {
        if (resourceManager.SpendResource("Revenue", juniorDeveloperCost))
        {
            juniorDeveloperCount++;
            juniorDeveloperCost *= 1.15f;
            UpdateRevenuePerSecond();
            Debug.Log("Hired Junior Developer.");
        }
        else
        {
            Debug.Log("Not enough Revenue to hire Junior Developer.");
        }
    }

    public void ReleaseSmallGame()
    {
        if (resourceManager.SpendResource("Revenue", releasedGameCost))
        {
            releasedGameCount++;
            releasedGameCost *= 1.15f;
            resourceManager.AddResource("Reputation", 5f);
            UpdateRevenuePerSecond();
            Debug.Log("Released Small Game.");
        }
        else
        {
            Debug.Log("Not enough Revenue to release Small Game.");
        }
    }

    public void BuyBetterEngine()
    {
        BuyUpgrade("Better Engine");
    }

    public void BuyMarketingPush()
    {
        BuyUpgrade("Marketing Push");
    }

    public void BuyAssetLibrary()
    {
        BuyUpgrade("Asset Library");
    }

    private void BuyUpgrade(string upgradeName)
    {
        for (int i = 0; i < upgrades.Count; i++)
        {
            if (upgrades[i].upgradeName == upgradeName && !upgrades[i].purchased)
            {
                if (resourceManager.SpendResource("Revenue", upgrades[i].cost))
                {
                    upgrades[i].purchased = true;
                    globalRevenueMultiplier *= upgrades[i].revenueMultiplier;
                    UpdateRevenuePerSecond();
                    Debug.Log("Purchased upgrade: " + upgrades[i].upgradeName);
                }
                else
                {
                    Debug.Log("Not enough Revenue to buy " + upgrades[i].upgradeName);
                }

                return;
            }
        }
    }
}
