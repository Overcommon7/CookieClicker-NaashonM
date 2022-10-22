using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldenCookie : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI amountInText;
    public void OnGoldenCookieClick()
    {
        PlayerManager.instance.playerValues.amountOfCookies += (PlayerManager.instance.playerValues.cookiesPerSecond * 900) + 13;
        var text = Instantiate(amountInText);
        text.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().position;
        text.transform.SetParent(transform.parent, true);
        text.text = string.Format("{0:#,##0}", (PlayerManager.instance.playerValues.cookiesPerSecond * 900) + 13);
        SoundManager.instance.PlayInGoldenClicked();
        Destroy(gameObject);
        GoldenCookieManager.activeGoldenCookie = null;
    }
}
