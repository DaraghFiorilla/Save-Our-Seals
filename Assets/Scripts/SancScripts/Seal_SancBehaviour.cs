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
    [HideInInspector] public float timer;
    [HideInInspector] public bool animationInactive;
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
    [SerializeField] private GameObject releaseSealPrefab;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        gameManager = FindFirstObjectByType<Sanc_GameManager>();
        animationInactive = true;
        feedCooldownSlider = GameObject.Find("/Canvas/UI/SlidersParent/HungerSlider").GetComponent<Slider>();
        enrichmentCooldownSlider = GameObject.Find("/Canvas/UI/SlidersParent/EnrichmentSlider").GetComponent<Slider>();
        ResetAnimTimer();

        string key = "SealCollected_" + myID + "_";
        if (!PlayerPrefs.HasKey(key)) { SaveAsCollected(); }
        if (PlayerPrefs.GetInt(key + "Released_") == 1)
        {
            if (gameManager.seals.Contains(this)) { gameManager.seals.Remove(this); }
            gameObject.SetActive(false);
        }
    }

    public void SaveAsCollected()
    {
        string keyPrefix = "SealCollected_" + myID + "_";
        PlayerPrefs.SetInt(keyPrefix, 1);
        PlayerPrefs.SetString(keyPrefix + "Type_", sealType);
        PlayerPrefs.SetInt(keyPrefix + "Age_", age);
        PlayerPrefs.SetInt(keyPrefix + "Health_", health);
        PlayerPrefs.SetInt(keyPrefix + "Hunger_", hunger);
        PlayerPrefs.SetInt(keyPrefix + "Enrichment_", enrichment);
        if (PlayerPrefs.GetInt(keyPrefix + "Released_") != 1) PlayerPrefs.SetInt(keyPrefix + "Released_", 0);
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

    public void ReleaseSeal()
    {
        if (gameManager.seals.Contains(this)) { gameManager.seals.Remove(this); }
        PlayerPrefs.SetInt("SealCollected_" + myID + "_Released_", 1);
        PlayerPrefs.Save();
        gameManager.UpdateRescueNo();
        gameObject.SetActive(false);
    }

    public void CheckHealth()
    {
        if (health >= 99)
        {
            GameObject obj = Instantiate(releaseSealPrefab, GameObject.FindGameObjectWithTag("Canvas").gameObject.transform);
            obj.transform.SetAsLastSibling();
            ReleaseSeal releaseSeal = obj.GetComponent<ReleaseSeal>();
            releaseSeal.releasedSeal = this;
            if (sealType == "Grey") { releaseSeal.imageObjs[0].SetActive(true); releaseSeal.imageObjs[1].SetActive(false); }
            else { releaseSeal.imageObjs[0].SetActive(false); releaseSeal.imageObjs[1].SetActive(true); }
        }
    }

    private void ResetAnimTimer()
    {
        timer = 0;
        currentTimeBetweenAnim = Random.Range(minTimeBetweenAnim, maxTimeBetweenAnim);
    }

    public void AdjustHealth(int adjustAmount)
    {
        if (gameManager.TrashComplete()) { adjustAmount += 10; }
        health += adjustAmount;
        if (selected)
        {
            gameManager.healthDisplay.text = health + "%";
        }
        SaveAsCollected();
        CheckHealth();
    }

    public void AdjustHunger(int adjustAmount)
    {
        if (gameManager.TrashComplete()) { adjustAmount += 10; }
        hunger += adjustAmount;
        if (selected)
        {
            gameManager.hungerDisplay.text = hunger + "%";
        }
        SaveAsCollected();
    }
    public void AdjustEnrichment(int adjustAmount)
    {
        if (gameManager.TrashComplete()) { adjustAmount += 10; }
        enrichment += adjustAmount;
        if (selected)
        {
            gameManager.enrichDisplay.text = enrichment + "%";
        }
        SaveAsCollected();
    }

    public IEnumerator FeedingCoroutine(float eatCooldownTime, float happyTime, int adjustAmount)
    {
        animationInactive = false;
        animator.SetTrigger("eat");
        Debug.Log("Feeding cooldown coroutine started with cooldown time: " + eatCooldownTime + happyTime);
        canEnrich = false;
        canFeed = false;
        feedCooldownSlider.maxValue = eatCooldownTime + happyTime;
        feedCooldownSlider.value = feedCooldownSlider.maxValue;
        while (eatCooldownTime > 0)
        {
            eatCooldownTime -= Time.deltaTime;
            if (selected) { feedCooldownSlider.value = eatCooldownTime + happyTime; }
            yield return null;
        }
        animator.SetTrigger("finishedEat");
        while (happyTime > 0)
        {
            happyTime -= Time.deltaTime;
            if (selected) { feedCooldownSlider.value = happyTime; }
            yield return null;
        }
        canFeed = true;
        canEnrich = true;
        hunger += adjustAmount;
        if (selected)
        {
            gameManager.hungerDisplay.text = hunger + "%";
        }
        animationInactive = true;
        SaveAsCollected();
        Debug.Log("Feeding cooldown coroutine finished");
        yield return null;
    }


    public IEnumerator EnrichmentCoroutine(float enrichCooldownTime, float happyTime, int adjustAmount)
    {
        animationInactive = false;
        Debug.Log("Enrich cooldown coroutine started with cooldown time: " + (enrichCooldownTime + happyTime));
        animator.SetTrigger("eat");
        canFeed = false;
        canEnrich = false;
        enrichmentCooldownSlider.maxValue = enrichCooldownTime + happyTime;
        while (enrichCooldownTime > 0)
        {
            enrichCooldownTime -= Time.deltaTime;
            if (selected) { enrichmentCooldownSlider.value = enrichCooldownTime + happyTime; }
            yield return null;
        }
        animator.SetTrigger("finishedEat");
        while (happyTime > 0)
        {
            happyTime -= Time.deltaTime;
            if (selected) { enrichmentCooldownSlider.value = happyTime; }
            yield return null;
        }
        canEnrich = true;
        canFeed = true;
        enrichment += adjustAmount;
        if (selected)
        {
            gameManager.enrichDisplay.text = enrichment + "%";
        }
        animationInactive = true;
        SaveAsCollected();
        Debug.Log("Enrich cooldown coroutine finished");
        yield return null;
    }

    public void Tick()
    {
        myTickCounter++;
        if ((float)myTickCounter % 15 == 0)
        {
            if (hunger > 30 && enrichment > 30)
            {
                if ((hunger + enrichment) / 2 > 60) { AdjustHealth(15); }
                else if (hunger + enrichment / 2 > 80) { AdjustHealth(25); }
                else { AdjustHealth(10); }
            }
        }
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
