using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance {get; private set;}
    [SerializeField] private RectTransform roundClearedLeft;
    [SerializeField] private RectTransform roundClearedRight;
    [SerializeField] private RectTransform roundClearedText;

    [SerializeField] private RectTransform colorWheel;
    [SerializeField] private RectTransform levelClearedText;
    [SerializeField] private RectTransform tapToContinue;
    private Vector3 roundClearedLeftPosition;
    private Vector3 roundClearedRightPosition;
    private Vector3 originRoundClearedLeftPos;
    private Vector3 originRoundClearedRightPos;
    private int rotateColorWheelId;
    private int scaleColorWheelId;
    private int levelClearedScaleId;
    public bool check = false;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        
    }
    private void Update()
    {
        if (check)
        {
            LoadLevel();
            check = false;
        }
    }
    private void Start()
    {
        GameManager.Instance.OnNewLevel += OnNewLevel;

        //#if UNITY_STANDALONE
        roundClearedLeftPosition = new Vector2(-740f,0);
        roundClearedRightPosition = new Vector2(240f,0);
        roundClearedLeft.anchoredPosition = new Vector2(-2200f, 0);
        roundClearedRight.anchoredPosition = new Vector2(2100f, 0);
        //#endif

        /*#if UNITY_ANDROID || UNITY_IOS
        roundClearedLeft.anchoredPosition = new Vector2(-Screen.width, 0);
        roundClearedRight.anchoredPosition = new Vector2(Screen.width, 0);
        #endif*/

        originRoundClearedLeftPos = roundClearedLeft.anchoredPosition;
        originRoundClearedRightPos = roundClearedRight.anchoredPosition;
        roundClearedText.localScale = Vector3.zero;

        colorWheel.localScale = Vector3.zero;
        levelClearedText.localScale = new Vector3(5,5,5);
        levelClearedText.gameObject.SetActive(false);
        tapToContinue.gameObject.SetActive(false);
    }
    private void OnNewLevel()
    {
        LeanTween.cancel(rotateColorWheelId);
        LeanTween.cancel(scaleColorWheelId);
        LeanTween.cancel(levelClearedScaleId);
        colorWheel.localScale = Vector3.zero;
        tapToContinue.gameObject.SetActive(false);
        levelClearedText.gameObject.SetActive(false);
        levelClearedText.localScale = new Vector3(5,5,5);
    }
    public void LoadRound()
    {
        LeanTween.move(roundClearedLeft, roundClearedLeftPosition, 0.4f).setEaseOutExpo();
        LeanTween.move(roundClearedRight, roundClearedRightPosition, 0.4f).setEaseOutExpo();

        LeanTween.scale(roundClearedText, new Vector3(1.5f, 1.5f, 1.5f), 0.6f).setEaseOutElastic().setDelay(0.3f).setOnComplete( () =>
        {
            LeanTween.scale(roundClearedText, Vector3.zero, 0.5f).setEaseOutExpo();
            LeanTween.move(roundClearedLeft, originRoundClearedLeftPos, 0.4f).setEaseOutExpo().setDelay(0.5f);
            LeanTween.move(roundClearedRight, originRoundClearedRightPos, 0.4f).setEaseOutExpo().setDelay(0.5f);
            
        });

    }
    public void LoadLevel()
    {
        rotateColorWheelId = LeanTween.rotateAround(colorWheel, Vector3.forward, -360, 5f).setLoopClamp().id;
        scaleColorWheelId = LeanTween.scale(colorWheel, new Vector3(26f, 26f, 26f), 0.7f).setEaseOutElastic().setOnComplete( () =>
        {
            levelClearedText.gameObject.SetActive(true);
            levelClearedScaleId = LeanTween.scale(levelClearedText, Vector3.one, 0.5f).setEaseOutElastic().id;

            tapToContinue.gameObject.SetActive(true);
        }).id;
        
    }
    public void ResetValue()
    {
        roundClearedText.localScale = Vector3.zero;

        colorWheel.localScale = Vector3.zero;
        levelClearedText.localScale = new Vector3(5,5,5);
        levelClearedText.gameObject.SetActive(false);
        tapToContinue.gameObject.SetActive(false);
    }
}
