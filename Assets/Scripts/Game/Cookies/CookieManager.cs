using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct FallingVariables
{
    public float fallSpeed;
    public float minYValue;
    public float maxYValue;
}

public class CookieManager : MonoBehaviour
{
    List<GameObject> cookies = new List<GameObject>();
    [SerializeField] FallingVariables fallingVariables;
    int index = 0;
    public static CookieManager instance = null;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        for (short i = 0; i < transform.childCount; i++)
            cookies.Add(transform.GetChild(i).gameObject);
    }

    void Update()
    {       
        FallingCookies.Update(cookies[index].transform, ref fallingVariables);
    }

    private void FixedUpdate()
    {

    }

    public bool SetActiveIndex(int idx)
    {
        if (idx == index) return true;
        index = idx;
        foreach (var obj in cookies)
            obj.SetActive(false);
        cookies[index].SetActive(true);
        return false;
    }
}
