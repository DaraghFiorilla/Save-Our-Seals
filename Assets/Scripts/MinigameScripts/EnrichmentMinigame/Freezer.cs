using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Freezer : MonoBehaviour
{
    [SerializeField] private bool minigameActive;
    [SerializeField] private Slider tempSlider;
    [SerializeField] private TextMeshProUGUI sliderText;
    private bool isHeld;

    public float temp, minTemp, maxTemp;
    public float coolRate, warmRate;
    public float targetMin, targetMax;
    public float successHoldTime;
    public float timeInTargetZone;

    private void Awake()
    {
        tempSlider.minValue = minTemp;
        tempSlider.maxValue = maxTemp;
    }

    public void StartFreezer()
    {
        Debug.Log("minigame starting");
        minigameActive = true;
    }

    private void Update()
    {
        if (minigameActive)
        {
            if (isHeld) { CoolTemp(); }
            else { WarmTemp(); }
            temp = Clamp(temp, minTemp, maxTemp);
            if (temp >= targetMin && temp <= targetMax)
            {
                timeInTargetZone += Time.deltaTime;
            }
            else { timeInTargetZone = 0; }

            if (timeInTargetZone >= successHoldTime)
            {
                minigameActive = false;
            }
            tempSlider.value = temp;
            sliderText.text = temp.ToString() + "°C";
        }
    }

    private float Clamp(float temp, float minTemp, float maxTemp)
    {
        throw new NotImplementedException();
    }

    private void CoolTemp()
    {
        Debug.Log("CoolTemp run");
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
