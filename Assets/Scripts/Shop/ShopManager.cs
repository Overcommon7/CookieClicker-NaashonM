using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance = null;
    public Dictionary<string, string[]> data = null;
    private List<ShopItem> stores = new List<ShopItem>();
    [SerializeField] CanvasGroup navigationBar;
    [SerializeField] Button boostersButton;
    readonly string filepath = "Shops.txt";
    [NonSerialized] public bool isEmpty = false;
    Animator animator;

    private void Awake()
    {
        if (instance != null)
        {
            this.enabled = false;
            return;
        }
        
        instance = this;
        stores = GetComponentsInChildren<ShopItem>(true).ToList();
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(true);
        data = Utils.GetFileAsDictionary(filepath);
        if (data == null || data.Count == 0) isEmpty = true;
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        TurnOffChildren();
        boostersButton.interactable = PlayerManager.instance.playerValues.cookiesPerSecond != 0;
    }
    private void OnDestroy()
    {
        List<string> saveData = new List<string>();
        foreach (var store in stores)
            saveData.Add(store.ToString());

        Utils.WriteToFile(ref saveData, filepath);
    }
    public void TurnOffChildren()
    {
        for (short i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
    }
    public void Close()
    {
        animator.SetBool("Close", true);
        navigationBar.interactable = true;
        boostersButton.interactable = PlayerManager.instance.playerValues.cookiesPerSecond != 0;
    }
    public void Open()
    {
        animator.SetBool("Close", false);
        for (short i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(true);
        navigationBar.interactable = false;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) OnDestroy();
        else
        {
            instance = null;
            Awake();
            Start();
        }
    }
}
