using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PurchasesManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI cookiesInADay;
    Animator animator;
    [SerializeField] CanvasGroup navigationBar;
    public static PurchasesManager instance = null;

    private void Awake()
    {
        if (instance) Destroy(gameObject);
        else instance = this;
    }
    void Start()
    {
        DisableAllChildren();
        animator = GetComponent<Animator>();
    }

    public void DisableAllChildren()
    {
        for (short i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
    }

    public void Close()
    {
        animator.SetBool("Close", true);
        navigationBar.interactable = true;
    }

    public void Open()
    {
        for (short i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(true);
        UpdateText();
        animator.SetBool("Close", false);
        navigationBar.interactable = false;

    }

    public void UpdateText()
    {
        cookiesInADay.text = string.Format("{0:#,##0}", PlayerManager.instance.playerValues.cookiesPerSecond * 86400);
    }
}
