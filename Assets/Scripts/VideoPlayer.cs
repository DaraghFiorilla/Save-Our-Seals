using UnityEngine;
using UnityEngine.Video;

public class VideoPlayer : MonoBehaviour
{
    [SerializeField] float timer;
    [SerializeField] GameObject rawImage;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource= GetComponent<AudioSource>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) { EndReached(); }
    }

    void EndReached()
    {
        rawImage.SetActive(false);
        audioSource.Play();
        gameObject.SetActive(false);
    }
}
