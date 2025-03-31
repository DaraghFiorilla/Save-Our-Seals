using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    public float timer;
    public float tickFrequency;
    public UnityAction OnTick;
    //public float tickCounter;
    private Sanc_GameManager gameManager;
    private void Awake()
    {
        gameManager = GetComponent<Sanc_GameManager>();
        OnTick += ResetTimer;
        OnTick += CallTickEvents;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= tickFrequency)
        {
            OnTick?.Invoke();
        }
    }

    public void ResetTimer()
    {
        timer = 0;
    }

    public void CallTickEvents()
    {
        foreach (Seal_SancBehaviour seal in gameManager.seals)
        {
            seal.Tick();
        }
    }
}
