using System.Collections;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public Vector2 startingLocalPos, targetLocalPos;
    public float moveSpeed;
    public string[] bodyTexts;
    [SerializeField] private TextMeshProUGUI bodyContainer;

    public void UpdateBodyText(int index)
    {
        bodyContainer.text = bodyTexts[index];
    }

    public void MoveToTarget()
    {
        StopAllCoroutines();
        StartCoroutine(MoveToTargetCor());
    }

    public void MoveToStart()
    {
        StopAllCoroutines();
        StartCoroutine(MoveToStartPos());
    }

    IEnumerator MoveToTargetCor()
    {
        while ((Vector2)transform.localPosition != targetLocalPos)
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, targetLocalPos, moveSpeed);
            yield return null;
        }
        yield return null;
    }

    IEnumerator MoveToStartPos()
    {
        while ((Vector2)transform.localPosition != startingLocalPos)
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, startingLocalPos, moveSpeed);
            yield return null;
        }
        yield return null;
    }
}
