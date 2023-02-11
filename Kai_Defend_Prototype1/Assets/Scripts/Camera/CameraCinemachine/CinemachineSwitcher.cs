using System.Collections;
using Cinemachine;
using Manager;
using UnityEngine;

public class CinemachineSwitcher : MonoBehaviour
{
    public static CinemachineSwitcher Instance { get; private set; }
    private Animator animator;
    
    [SerializeField] private Canvas canvas;
    private bool rightState;
    private bool leftState;
    private bool topState;
    private bool startState;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        GameManager.Instance.GameOver += GameOverCam;
        // AdsManager.Instance.OnAdsReward += AdsReward;
        RandomScrollManager.Instance.OnFirstRandomCard += HideCanvas;

        StartCoroutine(WaitToStart());
    }

    public void AdsReward()
    {
        //Debug if Game over at Top cam
        if (Camera.main.orthographic)
        {
            Camera.main.orthographic = false;
        }
        animator.Play("Font");
    }
    
    private void GameOverCam()
    {
        canvas.gameObject.SetActive(false);
        animator.Play("Start");
    }

    private IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(0.5f);
        animator.Play("Font");
        yield return new WaitForSeconds(0.7f);
        canvas.gameObject.SetActive(true);
    }

    private void HideCanvas()
    {
        canvas.gameObject.SetActive(!true);
    }

    public void SwitchLeftCamera()
    {
        if (TimeManager.Instance.DisableControler)
        {
            return;
        }
        if (topState)
        {
            return;
        }
        if (rightState)
        {
            rightState = false;
            animator.Play("Font");
            return;
        }
        animator.Play("Left");
        rightState = false;
        topState = false;
        leftState = true;
    }

    public void SwitchFontCamera()
    {
        if (TimeManager.Instance.DisableControler)
        {
            return;
        }
        
        BuildManager.Instance.EnableSelectHero = false;

        Camera.main.orthographic = false;

        BuildManager.Instance.DeselectHero();
        RandomScrollManager.Instance.LongClickCheck = false;
        StopAllCoroutines();
        animator.Play("Font");
        canvas.gameObject.SetActive(!true);
        rightState = false;
        leftState = false;
        topState = false;
    }
    
    private IEnumerator WaitForCam()
    {
        yield return new WaitForSeconds(0.5f);
        Camera.main.orthographic = true;
    }
    
    public void SwitchTopCamera()
    {
        if (TimeManager.Instance.DisableControler)
        {
            return;
        }

        BuildManager.Instance.EnableSelectHero = true;

        StartCoroutine(WaitForCam());
        
        UiManager.Instance.UpdateCardInventory();
        animator.Play("Top");
        StartCoroutine(WaitForShowCanvas());
        leftState = false;
        rightState = false;
        topState = true;
    }
    
    public void SwitchRightCamera()
    {
        if (TimeManager.Instance.DisableControler)
        {
            return;
        }
        if (topState)
        {
            return;
        }
        if (leftState)
        {
            leftState = false;
            animator.Play("Font");
            return;
        }
        animator.Play("Right");
        leftState = false;
        topState = false;
        rightState = true;
    }

    private IEnumerator WaitForShowCanvas()
    {
        yield return new WaitForSeconds(0.5f);
        canvas.gameObject.SetActive(true);
        RandomScrollManager.Instance.LongClickCheck = true;

        if (InventoryManager.Instance.InventoryField.gameObject.activeInHierarchy)
        {
            Debug.Log("inventory");
            RandomScrollManager.Instance.LongClickCheck = false;
        }
    }
}
