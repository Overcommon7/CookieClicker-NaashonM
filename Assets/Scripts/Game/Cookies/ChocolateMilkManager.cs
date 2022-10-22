using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChocolateMilkManager : MonoBehaviour
{
    public delegate void ChocolateMilk();
    static public event ChocolateMilk OnChocolateMilkStart;
    static public event ChocolateMilk OnChocolateMilkEnd;

    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI time;
    [SerializeField, ReadOnly] float timer = 0;
    bool duringChocolateMilk = false;
    
    void Start()
    {
        OnChocolateMilkStart += OnChocolateStart;
        OnChocolateMilkEnd += EndChocolate;
        slider.value = 0f;
    }

    void OnChocolateStart()
    {
        timer = 0f;
        duringChocolateMilk = true;
        MilkManager.spriteIndexOffset = 5;
        CookieButton.chocolateMilkMultiplier = 10;
    }

    void EndChocolate()
    {
        duringChocolateMilk = false;
        timer = 0f;
        MilkManager.spriteIndexOffset = 0;
        CookieButton.chocolateMilkMultiplier = 1;
    }

    private void OnDestroy()
    {
        OnChocolateMilkStart -= OnChocolateStart;
        OnChocolateMilkEnd -= EndChocolate;
    }

    void Update()
    {
        if (!duringChocolateMilk)
        {

            timer += Time.deltaTime;
            if (timer >= 1f)
            {
                timer = 0f;
                ++slider.value;
                time.text = slider.value.ToString();
            }
            if (slider.value >= slider.maxValue - 0.5f)
                OnChocolateMilkStart?.Invoke();
        }
        else
        {
            timer += Time.deltaTime;
            if (timer >= 0.06666667f)
            {
                timer = 0f;
                --slider.value;
                time.text = slider.value.ToString();
            }
            
            if (slider.value <= 0.001f) 
                OnChocolateMilkEnd?.Invoke();
        }       
    }
}
