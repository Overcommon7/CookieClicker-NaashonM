using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldenCookieManager : MonoBehaviour
{
    [SerializeField] GameObject goldenCookie; 
    [SerializeField] float fallSpeed;
    [SerializeField, ReadOnly] float timeSinceLastSpawn, timeSpawnable;
    static public RectTransform activeGoldenCookie = null;
    [SerializeField] bool spawn = false;

    private void SpawnCookie()
    {
        activeGoldenCookie = Instantiate(goldenCookie).GetComponent<RectTransform>();
        activeGoldenCookie.transform.SetParent(transform);
        activeGoldenCookie.position = 
            new Vector2(Random.Range(0, 735), GetComponent<RectTransform>().position.y);        
        timeSinceLastSpawn = 0f;
        timeSpawnable = 0f;
        SoundManager.instance.PlayInGoldenSpawned();
    }
    private void Update()
    {
        if (spawn)
        {
            SpawnCookie();
            spawn = false;
        }
        float dt = Time.deltaTime;
        if (activeGoldenCookie == null)
        {
            timeSinceLastSpawn += dt;
            if (timeSinceLastSpawn >= 60f)
            {
                timeSpawnable += dt;
                if (timeSpawnable > 0.51f)
                {
                    if (Random.Range(0, 10) < 1) SpawnCookie();
                    else timeSpawnable = 0f;
                }
            }
        }
        else
        {
            activeGoldenCookie.Translate(0, -fallSpeed * Time.deltaTime, 0);
            if (activeGoldenCookie.anchoredPosition.y < -2700)
            {
                Destroy(activeGoldenCookie.gameObject);
                activeGoldenCookie = null;
            }
        }
    }
}
