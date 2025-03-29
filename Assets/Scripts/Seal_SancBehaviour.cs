using UnityEngine;

public class Seal_SancBehaviour : MonoBehaviour
{
    public float minTimeBetweenAnim;
    public float maxTimeBetweenAnim;
    public float currentTimeBetweenAnim;
    public float timer;
    public bool inactive;
    //private Animator animator;
    private Animator animator;
   

    private void Awake()
    {
        animator = GetComponent<Animator>();
        //animator = GetComponent<Animator>();
        inactive = true;
        ResetAnimTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (inactive) 
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
}
