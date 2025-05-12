using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LandSealScript : MonoBehaviour
{
    public float minAnimTime, maxAnimTime, timeForSprite;
    private float targetTime, timer;
    [SerializeField] private Sprite[] sprites;
    private SpriteRenderer spriteRenderer;
    private GameObject heartObj;

    private void Awake()
    {
        heartObj = transform.GetChild(0).gameObject;
        spriteRenderer = GetComponent<SpriteRenderer>();
        RandomiseTime();
        spriteRenderer.sprite = sprites[0];
    }

    private void Update()
    {
        if (timer < targetTime)
        {
            timer += Time.deltaTime;
        }
        else { StartCoroutine(PlayAnim(timeForSprite)); }
    }

    void RandomiseTime()
    {
        timer = 0;
        targetTime = Random.Range(minAnimTime, maxAnimTime);
    }

    IEnumerator PlayAnim(float time)
    {
        spriteRenderer.sprite = sprites[1];
        heartObj.SetActive(true);

        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        spriteRenderer.sprite = sprites[0];
        heartObj.SetActive(false);
        RandomiseTime();
        yield return null;
    }
}
