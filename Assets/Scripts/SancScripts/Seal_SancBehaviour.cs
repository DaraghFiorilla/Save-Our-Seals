using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Seal_SancBehaviour : MonoBehaviour
{
    [Header("Animation Variables")]
    public float minTimeBetweenAnim;
    public float maxTimeBetweenAnim;
    public float currentTimeBetweenAnim;
    public float timer;
    public bool animationInactive;
    public Sprite fullSprite;
    public GameObject selectionOutline;
    private Slider feedCooldownSlider, enrichmentCooldownSlider;

    [Header("State Variables")]
    public int health;
    public int enrichment;
    public int hunger;
    [HideInInspector] public int myTickCounter;
    public bool selected;
    public int age;
    public string sealType;
    public int myID;
    public float feedingCooldown, enrichCooldown;
    public bool canFeed, canEnrich;

    private Animator animator;
    private Sanc_GameManager gameManager;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        gameManager = FindFirstObjectByType<Sanc_GameManager>();
        animationInactive = true;
        feedCooldownSlider = GameObject.Find("/Canvas/UI/SlidersParent/HungerSlider").GetComponent<Slider>();
        enrichmentCooldownSlider = GameObject.Find("/Canvas/UI/SlidersParent/EnrichmentSlider").GetComponent<Slider>();
        ResetAnimTimer();

        if (!gameManager.seals.Contains(this))
        {
            SaveAsCollected();
        }
    }

    public void SaveAsCollected()
    {
        string keyPrefix = "SealCollected_" + myID;
        PlayerPrefs.SetInt(keyPrefix, 1);
        PlayerPrefs.SetString(keyPrefix + "_Type", sealType);
        PlayerPrefs.SetInt(keyPrefix + "_Age", age);
        PlayerPrefs.SetInt(keyPrefix + "_Health", health);
        PlayerPrefs.SetInt(keyPrefix + "_Hunger", hunger);
        PlayerPrefs.SetInt(keyPrefix + "_Enrichment", enrichment);
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        if (animationInactive) 
        { 
            timer += Time.deltaTime;
            if (timer >= currentTimeBetweenAnim)
            {
                ResetAnimTimer();
                animator.SetTrigger("playIdle");
            }
        }
    }

    private void ResetAnimTimer()
    {
        timer = 0;
        currentTimeBetweenAnim = Random.Range(minTimeBetweenAnim, maxTimeBetweenAnim);
    }

    public void AdjustHealth(int adjustAmount)
    {
        health += adjustAmount;
        if (selected)
        {
            gameManager.sealDisplayParent.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "Health: " + health + "%";
        }
        SaveAsCollected();
    }

    public void AdjustHunger(int adjustAmount)
    {
        hunger += adjustAmount;
        if (selected)
        {
            gameManager.sealDisplayParent.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = "Hunger: " + hunger + "%";
        }
        StartCoroutine(FeedingCooldown(feedingCooldown));
        SaveAsCollected();
    }
    public void AdjustEnrichment(int adjustAmount)
    {
        enrichment += adjustAmount;
        if (selected)
        {
            gameManager.sealDisplayParent.GetChild(3).GetComponentInChildren<TextMeshProUGUI>().text = "Enrichment: " + enrichment + "%";
        }
        StartCoroutine(EnrichmentCooldown(enrichCooldown));
        SaveAsCollected();
    }

    IEnumerator FeedingCooldown(float cooldownTime)
    {
        Debug.Log("Feeding cooldown coroutine started with cooldown time: " + cooldownTime);
        canEnrich = false;
        canFeed = false;
        feedCooldownSlider.maxValue = cooldownTime;
        feedCooldownSlider.value = feedCooldownSlider.maxValue;
        while (cooldownTime > 0)
        {
            cooldownTime -= Time.deltaTime;
            if (selected) { feedCooldownSlider.value = cooldownTime; }
            yield return null;
        }
        canFeed = true;
        canEnrich = true;
        Debug.Log("Feeding cooldown coroutine finished");
        yield return null;
    }


    IEnumerator EnrichmentCooldown(float cooldownTime)
    {
        canFeed = false;
        canEnrich = false;
        if (selected)
        {
            enrichmentCooldownSlider.maxValue = cooldownTime;
            enrichmentCooldownSlider.value = enrichmentCooldownSlider.maxValue;
        }
        while (cooldownTime > 0)
        {
            cooldownTime -= Time.deltaTime;
            if (selected) { enrichmentCooldownSlider.value = cooldownTime; }
            yield return null;
        }
        canEnrich = true;
        canFeed = true;
        yield return null;
    }

    public void Tick()
    {
        //Debug.Log("Tick");
        myTickCounter++;
        if ((float)myTickCounter % 20 == 0)
        {
            if (hunger > 0) { AdjustHunger(-1); }
        }
        if ((float)myTickCounter % 25 == 0)
        {
            if (enrichment > 0) { AdjustEnrichment(-1); }
        }
    }
}
