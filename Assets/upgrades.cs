using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

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
    Random rnd = new Random();
    public enum UpgradeType
    {
        Damage,
        Health,
        Speed,
        AttackSpeed,
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

    public void Test()
    {
        Debug.Log(upgradesLogics[2].name);
    }
    public void GainUpgrade(int i)
    {
        var u = upgradesLogics[i];
        u.level++;
        stats.upgraded = true;
        CalculateStats();
        DoneTimeToStart();
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

    public void StartRoll()
    {
        StartCoroutine(Roll());
    }

    IEnumerator Roll()
    {
        int num1 = rnd.Next(upgradesLogics.Length);
        int num2 = rnd.Next(upgradesLogics.Length);
        int num3 = rnd.Next(upgradesLogics.Length);
        Debug.Log("the first number is " + num1 + " the secound number is " + num2 + " the third number is " + num3);
        Debug.Log("It should work because the " + upgradesLogics[num1].name + " is num1 and " + upgradesLogics[num2].name + " is num2 and finally " + upgradesLogics[num3].name + " is num3");
        yield return new WaitForSeconds(4f);
        GameObject upgrade1 = Instantiate(upgradesLogics[num1].Button, upgradebase.transform);
        upgrade1.transform.position = spawn1.position;
        GameObject upgrade2 = Instantiate(upgradesLogics[num2].Button, upgradebase.transform);
        upgrade2.transform.position = spawn2.position;
        GameObject upgrade3 = Instantiate(upgradesLogics[num3].Button, upgradebase.transform);
        upgrade3.transform.position = spawn3.position;

    }



    public void DoneTimeToStart()
    {
        upgradebase.SetActive(false);        
        Time.timeScale = 1f;
        stats.upgraded = false;
    }


}

