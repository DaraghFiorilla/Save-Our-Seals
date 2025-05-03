using UnityEngine;

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

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        //animator = GetComponent<Animator>();
        animationInactive = true;
        ResetAnimTimer();
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
    }

    public void AdjustEnrichment(int adjustAmount)
    {
        enrichment += adjustAmount;
    }

    public void AdjustHealth(int adjustAmount)
    {
        health += adjustAmount;
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
