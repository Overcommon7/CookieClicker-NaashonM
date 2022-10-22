using MyBox;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ShopType
{ 
   Upgrade,
   CPS
}

[System.Serializable]
public class ShopValues
{
    public ShopType shopType;
    [SerializeField] private int level;
    [SerializeField] private UInt64 price;
    public UInt64 basePrice;
    public double increase;
    [System.NonSerialized] public TextMeshProUGUI levelText;
    [System.NonSerialized] public TextMeshProUGUI priceText;

    public int Level { get => level; set { level = value; levelText.text = string.Format("{0:#,##0.##}", level); } }
    public UInt64 Price { get => price; set { price = value; priceText.text = string.Format("{0:#,##0.##}", price); } }

}

public class ShopItem : MonoBehaviour
{
    [SerializeField] ShopValues shopValues;
    Button button;
    float cooldown = 0f;
 

    void Start()
    {
        button = GetComponent<Button>();
        if (button == null) enabled = false;

        button.onClick.AddListener(Buy);
        var textMeshes = GetComponentsInChildren<TextMeshProUGUI>();
        shopValues.levelText = textMeshes[0];
        shopValues.priceText = textMeshes[1];
        if (ShopManager.instance.isEmpty || !ShopManager.instance.data.ContainsKey(transform.name) )
        {
            shopValues.Level = shopValues.Level;
            shopValues.Price = shopValues.Price;
            shopValues.basePrice = shopValues.Price;
        }
        else
        {
            shopValues.Level = int.Parse(ShopManager.instance.data[transform.name][1]);
            shopValues.basePrice = ulong.Parse(ShopManager.instance.data[transform.name][2]);
            if (shopValues.shopType == ShopType.CPS) shopValues.Price = GetPrice();
            else
            {
                shopValues.Price = shopValues.basePrice;
                int level = shopValues.Level;
                while (level > 0)
                {
                    shopValues.Price = (ulong)(shopValues.Price * 2.5);
                    --level;
                }
                    
            }

        }
    }

    void Update()
    {
         if (cooldown > 0f)
            cooldown -= Time.deltaTime;
    }

    private void OnApplicationQuit()
    {
        button.onClick.RemoveAllListeners();        
    }                                                                                        
    public void Buy()
    {
        if (cooldown >= 0.01f) return;
        cooldown = 0.2f;
        if (PlayerManager.instance.playerValues.amountOfCookies < shopValues.Price)
        {
            CantBuy();
            SoundManager.instance.PlayCantBuySound();
            return;
        }

        PlayerManager.instance.playerValues.amountOfCookies -= shopValues.Price;
        shopValues.Level++;
        if (shopValues.shopType == ShopType.CPS) BuyCPS();
        else BuyUpgrade();
        SoundManager.instance.PlayBuySound();
    }

    void CantBuy()
    {
        
    }

    ulong GetPrice()
    {
        return (ulong)(Math.Pow(1.4, shopValues.Level) * shopValues.basePrice);
    }

    void BuyCPS()
    {
        PlayerManager.instance.playerValues.cookiesPerSecond += shopValues.increase;
        shopValues.Price = GetPrice();
    }

    void BuyUpgrade()
    {
        PlayerManager.instance.playerValues.cookiesPerClick *= shopValues.increase;
        shopValues.Price = (ulong)(shopValues.Price * 2.5);
    }

    public override string ToString()
    {
       return transform.name + ' ' + shopValues.Level.ToString() + ' ' + shopValues.basePrice.ToString();
    }
}
