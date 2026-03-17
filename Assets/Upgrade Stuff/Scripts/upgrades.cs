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
    public GameObject button;
    public GameObject ButtonContainer;
    [SerializeField] PlayerStats stats;
    [SerializeField] PlayerAttack gunlogic;
    [SerializeField] UpgradeOpen logic;
    public Transform spawn1, spawn2, spawn3;
    public Animator animator;

    [Tooltip("Text element showing how many picks remain. Place anywhere in your Canvas.")]
    public TMPro.TMP_Text picksRemainingText;

    public int num1;
    public int num2;
    public int num3;
    public Upgrade[] upgradesLogics;
    public bool rolled;

    private int picksRemaining = 0;
    private int currentWaveRef = 0;

    public enum UpgradeType
    {
        Damage, //✅
        Health, //✅
        Speed, //✅
        AttackSpeed, //✅
        DashCooldown, //✅
        DashChargers, //✅
        SideAttacks, //✅
        Shotgun, //✅
        BackAttack, //✅
        Pierce, //✅
        BulletExplosion, //✅
        Doubleshot //✅
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
        if (i < 0 || i >= upgradesLogics.Length) return;

        upgradesLogics[i].level++;
        CalculateStats();

        picksRemaining--;
        UpdatePicksText();

        Runit(); // Clear existing buttons

        if (picksRemaining > 0)
        {
            // Show how many picks are left then re-roll
            StartCoroutine(RollAfterFrame());
        }
        else
        {
            StartCoroutine(CloseAfterFrame());
        }
    }

    IEnumerator RollAfterFrame()
    {
        yield return null;
        rolled = false;   // Allow re-roll
        StartRoll();
    }

    IEnumerator CloseAfterFrame()
    {
        yield return null;
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
    private void Start()
    {
        foreach (var u in upgradesLogics)
        {
            u.level = 0;
        }
    }
    public void CalculateStats()
    {
        if (stats == null)
        {
            Debug.LogError("Stats är null! Dra in PlayerStats i Inspector.");
            return;
        }

        stats.ResetStats();

        foreach (var u in upgradesLogics)
        {
            if (u.level <= 0) continue;

            float value = u.level * u.effect * GetRarityMultiplier(u.rarityType);

            switch (u.type)
            {
                case UpgradeType.Damage:
                    stats.damage += value; break;
                case UpgradeType.Health:
                    stats.hp += value; break;
                case UpgradeType.Speed:
                    stats.speed += value; break;
                case UpgradeType.AttackSpeed:
                    stats.attack_speed += value; break;
                case UpgradeType.DashChargers:
                    stats.dash_chargers += value; break;
                case UpgradeType.DashCooldown:
                    stats.dash_coldown_reduction += value; break;
                case UpgradeType.BackAttack:
                    gunlogic.BackAttack = true; break;
                case UpgradeType.SideAttacks:
                    gunlogic.SideAttacks = true; break;
                case UpgradeType.Shotgun:
                    gunlogic.Shotgun = true; break;
                case UpgradeType.Pierce:
                    stats.pierce = true;
                    stats.pierceAmount += value;
                    break;
                case UpgradeType.Doubleshot:
                    gunlogic.doubleshoot = true; break;
                case UpgradeType.BulletExplosion:
                    gunlogic.bulletExplosion = true; break;
            }
        }


        stats.ApplyStats();
    }

    public void DoneTimeToStart()
    {
        if (picksRemainingText != null)
            picksRemainingText.text = "";
        button.SetActive(false);
        logic.hasOpened = false;
        rolled = false;
        if (upgradebase != null)
            upgradebase.SetActive(false);
    }

    public void Runit()
    {
        foreach (Transform child in ButtonContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void StartSafeZone(int wave)
    {
        currentWaveRef = wave;
        picksRemaining = PicksForWave(wave);
        UpdatePicksText();
    }

    int PicksForWave(int wave)
    {
        if (wave >= 20) return 4;
        if (wave >= 15) return 3;
        if (wave >= 10) return 2;
        return 1;
    }

    public void StartRoll()
    {
        if (!rolled)
        {
            StartCoroutine(Roll());
        }        
    }

    IEnumerator Roll()
    {
        rolled = true;
        num1 = UnityEngine.Random.Range(0, upgradesLogics.Length);
        num2 = UnityEngine.Random.Range(0, upgradesLogics.Length);
        num3 = UnityEngine.Random.Range(0, upgradesLogics.Length);

        int index1 = num1;
        int index2 = num2;
        int index3 = num3;
        Debug.Log("the first number is " + num1 + " the secound number is " + num2 + " the third number is " + num3);
        Debug.Log("It should work because the " + upgradesLogics[num1].name + " is num1 and " + upgradesLogics[num2].name + " is num2 and finally " + upgradesLogics[num3].name + " is num3");
        Time.timeScale = 1f;
        animator.SetBool("done", false);
        animator.SetTrigger("Roll");
        yield return new WaitForSeconds(3f);
        animator.SetBool("done", true); 

        GameObject upgrade1 = Instantiate(upgradesLogics[num1].Button, ButtonContainer.transform);
        upgrade1.transform.position = spawn1.position;
        upgrade1.GetComponent<Button>().onClick.AddListener(delegate { GainUpgrade(index1); });
        GameObject upgrade2 = Instantiate(upgradesLogics[num2].Button, ButtonContainer.transform);
        upgrade2.transform.position = spawn2.position;
        upgrade2.GetComponent<Button>().onClick.AddListener(delegate { GainUpgrade(index2); });
        GameObject upgrade3 = Instantiate(upgradesLogics[num3].Button, ButtonContainer.transform);
        upgrade3.transform.position = spawn3.position;
        upgrade3.GetComponent<Button>().onClick.AddListener(delegate { GainUpgrade(index3); });
    }

    void UpdatePicksText()
    {
        if (picksRemainingText != null)
            picksRemainingText.text = $"Picks remaining: {picksRemaining}";
    }
}

