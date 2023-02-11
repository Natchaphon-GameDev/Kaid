using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LongClickButton : MonoBehaviour , IPointerDownHandler , IPointerUpHandler
{
    private bool pointerDown;
    private float pointerDownTimer;

    [SerializeField] private float holdTime;

    public UnityEvent OnLongClick;

    [SerializeField] private Image fillImg;


    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Reset();

    }

    private void Update()
    {
        
        if (!RandomScrollManager.Instance.LongClickCheck)
        {
            if (pointerDown)
            {
                StartCoroutine(DelayClick());
            }
            else
            {
                Reset();
                StopAllCoroutines();
            }
        }
        
    }

    private IEnumerator DelayClick()
    {
        yield return new WaitForSeconds(0.25f);
        pointerDownTimer += Time.deltaTime;
        if (pointerDownTimer >= holdTime)
        {
            OnLongClick?.Invoke();
            Reset();
        }
        fillImg.fillAmount = pointerDownTimer / holdTime;
    }

    private void Reset()
    {
        pointerDown = false;
        pointerDownTimer = 0;
        fillImg.fillAmount = pointerDownTimer / holdTime;
    }

    public void DestroyCard()
    {
        switch (tag)
        {
            case "Card_F81":
                InventoryManager.Instance.Cards[0].CardAmount--;
                break;
            case "Card_F13":
                InventoryManager.Instance.Cards[1].CardAmount--;
                break;
            case "Card_W75":
                InventoryManager.Instance.Cards[2].CardAmount--;
                break;
            case "Card_W410":
                InventoryManager.Instance.Cards[3].CardAmount--;
                break;
            case "Card_TEP58":
                InventoryManager.Instance.Cards[4].CardAmount--;
                break;
            case "Card_P32":
                InventoryManager.Instance.Cards[5].CardAmount--;
                break;
            case "Card_Z1":
                InventoryManager.Instance.Cards[6].CardAmount--;
                break;
            case "Card_Z23":
                InventoryManager.Instance.Cards[7].CardAmount--;
                break;
            case "Card_Z25":
                InventoryManager.Instance.Cards[8].CardAmount--;
                break;
            case "Card_Z46":
                InventoryManager.Instance.Cards[9].CardAmount--;
                break;
            case "Card_U73":
                InventoryManager.Instance.Cards[10].CardAmount--;
                break;
            case "Card_U556":
                InventoryManager.Instance.Cards[11].CardAmount--;
                break;
            case "Card_U110":
                InventoryManager.Instance.Cards[12].CardAmount--;
                break;
            case "Card_U101":
                InventoryManager.Instance.Cards[13].CardAmount--;
                break;
            case "Card_I168":
                InventoryManager.Instance.Cards[14].CardAmount--;
                break;
            case "Card_I13":
                InventoryManager.Instance.Cards[15].CardAmount--;
                break;
            case "Card_I58":
                InventoryManager.Instance.Cards[16].CardAmount--;
                break;
            case "Card_I26":
                InventoryManager.Instance.Cards[17].CardAmount--;
                break;
        }

        if (CompareTag("Card_W75") || CompareTag("Card_W410") || CompareTag("Card_TEP58") || CompareTag("Card_P32") || CompareTag("Card_F13") || CompareTag("Card_F81")) 
        {
            MoneyManager.Instance.AddMoney(RandomScrollManager.Instance.RandomScrollPlusCost / 2);
        }
        else
        {
            MoneyManager.Instance.AddMoney(RandomScrollManager.Instance.RandomScrollCost / 2);
        }
        
        UiManager.Instance.UpdateCardInventory();

        UiManager.Instance.blockUI.gameObject.SetActive(false);
        AnimationManager.Instance.MapDown();
        InventoryManager.Instance.IsBuildMode = false;
        BuildManager.Instance.heroToBuild = InventoryManager.Instance.HeroTeam.Find(x=>x.HeroName == HeroName.Null);
        Destroy(gameObject);
    }
}
