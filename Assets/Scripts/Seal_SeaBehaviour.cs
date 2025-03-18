using UnityEngine;

public class Seal_SeaBehaviour : MonoBehaviour
{
    public float maxTimeBetweenMoves;
    public float minTimeBetweenMoves;
    public float currentTimeBetweenMoves;
    public float timer;
    public float speed;

    private void Awake()
    {
        ResetMoveTime();
    }

    private void Update()
    {
        if (timer < currentTimeBetweenMoves) { timer += Time.deltaTime; }
        else { MoveSeal(); }
    }

    void ResetMoveTime()
    {
        currentTimeBetweenMoves = Random.Range(minTimeBetweenMoves, maxTimeBetweenMoves);
    }

    private void MoveSeal()
    {
        
    }
}
