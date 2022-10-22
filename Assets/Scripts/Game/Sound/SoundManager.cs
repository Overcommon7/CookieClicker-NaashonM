using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource audioSource2;
    [SerializeField] AudioClip buy;
    [SerializeField] AudioClip click;
    [SerializeField] AudioClip click2;
    [SerializeField] AudioClip cantBuy;
    [SerializeField] AudioClip goldenCookieSpawned;
    [SerializeField] AudioClip goldenCookieClicked;
    [SerializeField] AudioClip inAppPurchaseBought;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void PlayCookieSound()
    {
        if (Random.Range(0, 10) < 2) audioSource.PlayOneShot(click2);
        else audioSource2.PlayOneShot(click);
    }

    public void PlayBuySound()
    {
        audioSource.PlayOneShot(buy);
    }

    public void PlayCantBuySound()
    {
        audioSource.PlayOneShot(cantBuy);
    }

    public void PlayInAppPurcahse()
    {
        audioSource.PlayOneShot(inAppPurchaseBought);
    }

    public void PlayInGoldenSpawned()
    {
        audioSource.PlayOneShot(goldenCookieSpawned);
    }

    public void PlayInGoldenClicked()
    {
        audioSource.PlayOneShot(goldenCookieClicked);
    }
}
