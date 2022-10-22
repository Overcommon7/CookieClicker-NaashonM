using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CookieButton : MonoBehaviour
{
    readonly double TimeFrame = 1;
    double timer = 0;
    [SerializeField, ReadOnly] float spinRate = 0;
    [SerializeField] float spinSpeed = 0;
    [SerializeField] TextMeshProUGUI textMeshPro;
    RectTransform rectTransform;
    public static short chocolateMilkMultiplier = 1;

    public void OnClick()
    {
        PlayerManager.instance.playerValues.amountOfCookies += PlayerManager.instance.playerValues.cookiesPerClick * PlayerManager.instance.runTimeValues.cookiePerClickMultiplier * chocolateMilkMultiplier;
        PlayerManager.instance.clickVariables.numOfClicksPerTimeFrame++;
        PlayerManager.instance.runTimeValues.totalClicks++;
        var textMesh = Instantiate(textMeshPro);
        textMesh.GetComponent<RectTransform>().SetParent(transform.parent);
        textMesh.GetComponent<RectTransform>().anchoredPosition = 
            new Vector2(Random.Range((int)rectTransform.rect.x, (int)rectTransform.rect.width), Random.Range((int)rectTransform.rect.y, (int)rectTransform.rect.height));                            


        textMesh.text = '+' + string.Format("{0:#,##0}", PlayerManager.instance.playerValues.cookiesPerClick * PlayerManager.instance.runTimeValues.cookiePerClickMultiplier * chocolateMilkMultiplier);
        SoundManager.instance.PlayCookieSound();
    }

    private void Sustained(short level)
    {
        ++PlayerManager.instance.runTimeValues.timeSustained;
        if (level == 4 && PlayerManager.instance.runTimeValues.timeSustained >= 5) ++level;
            PlayerManager.instance.runTimeValues.cookiePerClickMultiplier = level;
        spinRate = level;
    }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void BonusCalculator()
    {
        timer += Time.deltaTime;
        if (timer < TimeFrame) return;
        timer = 0;
        if (PlayerManager.instance.runTimeValues.clicksPerSecond >= 6)
        {
            PlayerManager.instance.runTimeValues.cookiesPerSecondBoost = PlayerManager.instance.playerValues.cookiesPerSecond * 0.501;
            if (CookieManager.instance.SetActiveIndex(3)) Sustained(4);
            else PlayerManager.instance.runTimeValues.timeSustained = 0;

        }
        else if (PlayerManager.instance.runTimeValues.clicksPerSecond >= 4)
        {
            PlayerManager.instance.runTimeValues.cookiesPerSecondBoost = PlayerManager.instance.playerValues.cookiesPerSecond * 0.301;
            if (CookieManager.instance.SetActiveIndex(2)) Sustained(3);
            else PlayerManager.instance.runTimeValues.timeSustained = 0;
        }
        else if (PlayerManager.instance.runTimeValues.clicksPerSecond >= 2)
        {
            PlayerManager.instance.runTimeValues.cookiesPerSecondBoost = PlayerManager.instance.playerValues.cookiesPerSecond * 0.101;
            if (CookieManager.instance.SetActiveIndex(1)) Sustained(2);
            else PlayerManager.instance.runTimeValues.timeSustained = 0;
        }
        else
        {
            PlayerManager.instance.runTimeValues.cookiesPerSecondBoost = 0;
            if (CookieManager.instance.SetActiveIndex(0)) Sustained(1);
            else PlayerManager.instance.runTimeValues.timeSustained = 0;
        }
    }

    void Rotate()
    {
        rectTransform.Rotate(0, 0, spinRate * spinSpeed * Time.deltaTime);
    }

    private void Update()
    {
        BonusCalculator();
        Rotate();        
    }
}
