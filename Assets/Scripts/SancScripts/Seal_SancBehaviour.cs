using UnityEngine;
using UnityEngine.SceneManagement;

public class Seal_SancBehaviour : MonoBehaviour
{
    [Header("Animation Variables")]
    public float minTimeBetweenAnim;
    public float maxTimeBetweenAnim;
    public float currentTimeBetweenAnim;
    public float timer;
    public bool animationInactive;
    public Sprite fullSprite;
    
    [Header("State Variables")]
    public int health;
    public int enrichment;
    public int hunger;
    public int myTickCounter;
    public bool selected;
    public int age;
    public string sealType;
    public int myID;

    private Animator animator;
    private Sanc_GameManager gameManager;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        gameManager = FindFirstObjectByType<Sanc_GameManager>();
        animationInactive = true;
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

    public void AdjustHunger(int adjustAmount)
    {
        hunger += adjustAmount;
        SaveAsCollected();
    }

    public void AdjustEnrichment(int adjustAmount)
    {
        enrichment += adjustAmount;
        SaveAsCollected();
    }

    public void AdjustHealth(int adjustAmount)
    {
        health += adjustAmount;
        SaveAsCollected();
    }

    public void Tick()
    {
        Debug.Log("Tick");
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
