using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance {get; private set;}
    [SerializeField] private LevelDatabase levelDatabase;
    public Action<int, int> OnLevelChange;
    private int currentLevel;
    private int currentRound;
    private Level level;
    [SerializeField] private Button powerUpButton;
    private GameObject currentRoundObject;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        currentLevel = PlayerPrefs.GetInt("currentLevel", 0);
        currentRound = PlayerPrefs.GetInt("currentRound", 0);
       
        InitRound();
    }
    private void InitRound()
    {
        powerUpButton.interactable = true;

        GameObject roundObject = levelDatabase.GetRound(currentLevel, currentRound);
        currentRoundObject = Instantiate(roundObject, transform.position, Quaternion.identity);

        level = currentRoundObject.GetComponent<Level>();
        powerUpButton.onClick.AddListener(OnButtonClick);

        GameManager.Instance.SetRubberTotal(GetRoundRubbersNumber());
        OnLevelChange?.Invoke(currentLevel, currentRound);
    }
    private void OnButtonClick()
    {
        level.PowerUpPlayer();
        powerUpButton.interactable = false;
    }
    public int GetRoundRubbersNumber()
    {
        return currentRoundObject.GetComponent<Level>().GetRubbersNumber();
    }
    public void GoNextRound()
    {
        int maxRoundLevel = levelDatabase.GetRoundCount(currentLevel) - 1;
        if (currentRound == maxRoundLevel)
        {
            
            if (levelDatabase.HasNextLevel(currentLevel))
            {
                currentLevel++;
                currentRound = 0;
            }
            else
            {
                currentLevel = 0;
                currentRound = 0;
            }
            
            GameManager.Instance.IsNewLevel = true;
            LevelLoader.Instance.LoadLevel();
            
        }
        else
        {
            currentRound++;
            LevelLoader.Instance.LoadRound();
        }

        PlayerPrefs.SetInt("currentLevel", currentLevel);
        PlayerPrefs.SetInt("currentRound", currentRound);

        if (level != null)
        {
            powerUpButton.onClick.RemoveListener(OnButtonClick);
        }
        
        Destroy(currentRoundObject);
        InitRound();

    }
    public void RetryRound()
    {
        if (level != null)
        {
            powerUpButton.onClick.RemoveListener(OnButtonClick);
        }
        Destroy(currentRoundObject);
        InitRound();
    }
    public Color GetColored()
    {
        return levelDatabase.GetColored(currentLevel, currentRound);
    }
    public Color GetUnColored()
    {
        return levelDatabase.GetUnColored(currentLevel, currentRound);
    }
}
