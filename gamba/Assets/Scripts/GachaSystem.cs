using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class GachaSystem : MonoBehaviour
{
    public TMP_Text pointsText;
    public TMP_Text resultText;
    public Button pullButton;
    public TMP_Text historyText;

    public int startingPoints = 100;
    public int pullCost;
    public int pullsPerPress = 10;

    private int currentPoints;
    private List<string> pullHistory = new List<string>();

    private Dictionary<string, List<string>> rarityPools = new Dictionary<string, List<string>>();
    private List<(string rarity, float chance)> rarityChances = new List<(string, float)>();

    void Start()
    {
        currentPoints = startingPoints;
        SetupRarities();
        UpdateUI();
    }

    void SetupRarities()
    {
        rarityChances.Add(("Common", 0.6f));
        rarityChances.Add(("Rare", 0.25f));
        rarityChances.Add(("Epic", 0.12f));
        rarityChances.Add(("Legendary", 0.03f));

        rarityPools["Common"] = new List<string> { "Rusty Sword", "Old Boots", "Cracked Shield", "Goblin Tooth" };
        rarityPools["Rare"] = new List<string> { "Goblin Sword", "Silver Helmet", "Spiked Gauntlets" };
        rarityPools["Epic"] = new List<string> { "Fire Wand", "Shadow Cloak", "Golden Longbow" };
        rarityPools["Legendary"] = new List<string> { "Excalibur", "Phoenix Feather", "Cursed Crown" };
    }

    public void Pull()
    {
        int totalCost = pullCost * pullsPerPress;

        if (currentPoints < totalCost)
        {
            resultText.text = "Not enough points!";
            return;
        }

        currentPoints -= totalCost;
        List<string> results = new List<string>();

        for (int i = 0; i < pullsPerPress; i++)
        {
            string pull = GetRandomPull();
            results.Add(pull);
            pullHistory.Insert(0, pull);
        }

        if (pullHistory.Count > 50)
        {
            pullHistory.RemoveRange(50, pullHistory.Count - 50);
        }

        resultText.text = "You pulled:\n";
        foreach (string r in results)
        {
            resultText.text += "- " + "\n";
        }

        UpdateUI();
    }

    string GetRandomPull()
    {
        float roll = Random.value;
        float cumulative = 0f;

        foreach (var (rarity, chance) in rarityChances)
        {
            cumulative += chance;
            if (roll <= cumulative)
            {
                List<string> pool = rarityPools[rarity];
                string item = pool[Random.Range(0, pool.Count)];
                return $"{item} ({rarity})";
            }
        }

        return "Glitched Orb (???)";
    }

    void UpdateUI()
    {
        pointsText.text = "Points: " + currentPoints;
        historyText.text = "History:\n";

        int displayCount = Mathf.Min(10, pullHistory.Count);
        for (int i = 0; i < displayCount; i++)
        {
            historyText.text += "- " + pullHistory[i] + "\n";
        }
    }
}