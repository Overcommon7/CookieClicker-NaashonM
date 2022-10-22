using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PurchaseButton : MonoBehaviour
{
    double price;
    [SerializeField] int timeWarps;
    [SerializeField] int CPS; 
    void Start()
    {
        UpdatePrice(); 
    }

    void UpdatePrice()
    {
        price = double.Parse(GetComponentInChildren<TextMeshProUGUI>().text.Replace('$', ' '));
    }

    public void OnClick()
    {
        PlayerManager.instance.playerValues.amountOfCookies += PlayerManager.instance.playerValues.cookiesPerSecond * 86400 * timeWarps;
        PlayerManager.instance.playerValues.cookiesPerSecond *= 1 + (CPS * 0.01);
        PurchasesManager.instance.UpdateText();
        SoundManager.instance.PlayInAppPurcahse();
    }
}
