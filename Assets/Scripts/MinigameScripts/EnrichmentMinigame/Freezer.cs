using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Freezer : MonoBehaviour
{
    [SerializeField] private bool minigameActive;
    [SerializeField] private Slider tempSlider;
    [SerializeField] private TextMeshProUGUI sliderText;
    private EnrichmentMinigame em;
    private bool isHeld;

    public float temp, minTemp, maxTemp;
    public float coolRate, warmRate;
    public float targetMin, targetMax;
    public float successHoldTime;
    public float timeInTargetZone;

    private void Awake()
    {
        em = transform.parent.GetComponent<EnrichmentMinigame>();
        tempSlider.minValue = minTemp;
        tempSlider.maxValue = maxTemp;
    }

    public void StartFreezer()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        minigameActive = true;
    }

    private void Update()
    {
        if (minigameActive)
        {
            if (isHeld) { CoolTemp(); }
            else { WarmTemp(); }
            temp = Mathf.Clamp(temp, minTemp, maxTemp);
            if (temp >= targetMin && temp <= targetMax)
            {
                timeInTargetZone += Time.deltaTime;
            }
            else { timeInTargetZone = 0; }

            if (timeInTargetZone >= successHoldTime)
            {
                minigameActive = false;
                em.SpawnEndWindow();
            }
            tempSlider.value = temp;
            sliderText.text = temp.ToString("F1") + "°C";
        }
    }

    private void CoolTemp()
    {
        temp -= coolRate * Time.deltaTime;
    }

    private void WarmTemp()
    {
        temp += warmRate * Time.deltaTime;
    }

    public void FreezerButtonHeld() // referenced by button
    {
        isHeld = true;
    }

    public void FreezerButtonReleased() // referenced by button
    {
        isHeld = false;
    }
}
