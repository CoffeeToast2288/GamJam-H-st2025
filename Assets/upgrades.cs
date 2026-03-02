using System;
using TMPro;
using Unity.Burst;
using UnityEngine;
using UnityEngine.UI;


public class upgrades : MonoBehaviour
{
    public GameObject upgradebase;
    PlayerStats stats;
    public Transform spawn1, spawn2, spawn3;

    public Upgrade[] upgradesLogics;

    float baseDamage = 1;
    float baseHealth = 4;
    float baseSpeed = 5;
    float baseAttackSpeed = 1;

    public enum UpgradeType
    {
        Damage,
        Health,
        Speed,
        AttackSpeed,
        Light,
        DashCharge,
        DashCooldown
    }
    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    [System.Serializable]
    public class Upgrade
    {
        public GameObject Button;
        public string name;
        public float effect;
        public UpgradeType type;
        public Rarity rarityType;

        [HideInInspector] public int level;

        
    }

    public void GainUpgrade(int i)
    {
        var u = upgradesLogics[i];
        u.level++;
        CalculateStats();
    }
    float GetRarityMultiplier(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common: return 1.0f;
            case Rarity.Uncommon: return 1.25f;
            case Rarity.Rare: return 1.5f;
            case Rarity.Epic: return 2.0f;
            case Rarity.Legendary: return 3.0f;
            default: return 1.0f;
        }
    }
    public void CalculateStats()
    {
        // Reset stats first
        stats.damage = baseDamage;
        stats.hp = baseHealth;
        stats.speed = baseSpeed;
        stats.attack_speed = baseAttackSpeed;

        foreach (var u in upgradesLogics)
        {
            if (u.level <= 0) continue;

            float rarityMultiplier = GetRarityMultiplier(u.rarityType);
            float value = u.level * u.effect * rarityMultiplier;

            switch (u.type)
            {
                case UpgradeType.Damage:
                    stats.damage += value;
                    break;

                case UpgradeType.Health:
                    stats.hp += value;
                    break;

                case UpgradeType.Speed:
                    stats.speed += value;
                    break;

                case UpgradeType.AttackSpeed:
                    stats.attack_speed += value;
                    break;
            }
        }
    }


    public void DoneTimeToStart()
    {
        upgradebase.SetActive(false);
        Time.timeScale = 1f;
    }


}

