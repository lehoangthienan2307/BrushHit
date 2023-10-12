using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public State GameState {get; private set;}
    public int DisplayedScore {get; private set;}
    public bool IsGamePause {get; private set;}
    public bool IsNewLevel {get; set;}

    private int actualScore;
    private int roundStartScore;
    private int previousRubberColored;
    private int combo;
    private bool isCombo;
    private int currentRubberColored;

    public Action<int> OnScoreUpdate;
    public Action OnGameOver;
    public Action OnGamePlaying;
    public Action OnNewLevel;
    public Action<int> OnCombo;
    private int rubberColoredTotal;
    private int rubberTotal;
    private int count = 0;
    
    public ParticleSystem[] colorEffect;
    ParticleSystem testaa;

    [Header("Score Increase Speed")]
    [SerializeField] private int constantTerm = 1;
    [SerializeField] private int proportionalFactor = 5;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        IsNewLevel = false;
        InitRound();
        StartCoroutine(RunningScoreUpdater());
    }
    private void Update()
    {
        if (IsNewLevel)
        {
            IsGamePause = true;
            if(Input.GetMouseButtonDown(0))
            {
                OnNewLevel?.Invoke();
                IsNewLevel = false;  
                Invoke(nameof(UnPauseGame), 0.5f);
            }
        }
        if (rubberColoredTotal == rubberTotal)
        {
            LevelManager.Instance.GoNextRound();
            InitRound();
        }
    }
    private IEnumerator WaitFor(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }
    private void UnPauseGame()
    {
        IsGamePause = false;
    }
    public void StartPlaying()
    {
        OnGamePlaying?.Invoke();
    }
    
    public void GameOver()
    {
        IsGamePause = true;
        OnGameOver?.Invoke();
    }
    private void InitRound()
    {
        combo = 0;
        isCombo = false;
        currentRubberColored = 0;
        currentRubberColored = 0;

        if (IsNewLevel)
        {
            DisplayedScore = 0;
            actualScore = 0;
            roundStartScore = 0;
            PlayerPrefs.SetInt("score", roundStartScore);
            OnScoreUpdate?.Invoke(DisplayedScore);
        }
        else
        {
            DisplayedScore = PlayerPrefs.GetInt("score", 0);
            OnScoreUpdate?.Invoke(DisplayedScore);
            roundStartScore = actualScore;
            PlayerPrefs.SetInt("score", roundStartScore);
        }
        rubberColoredTotal = 0;
        IsGamePause = false;
        
    }
    public void SetRubberTotal(int total)
    {
        rubberTotal = total;
    }
    public void Retry()
    {
        IsGamePause = false;
        DisplayedScore = roundStartScore;
        actualScore = roundStartScore;
        OnScoreUpdate?.Invoke(DisplayedScore);
        LevelManager.Instance.RetryRound();
        InitRound();
    }
    public void Brush(Collider other)
    {
        SoundManager.Instance.Play(SoundType.SFX);
        if (other.gameObject.tag == "Colored")
        {
            other.GetComponentInChildren<MeshRenderer>().material.color = LevelManager.Instance.GetColored();
        }
        else
        {
            other.GetComponentInChildren<MeshRenderer>().material.color = LevelManager.Instance.GetUnColored();
        }

        colorEffect[count].transform.position = other.transform.position;
        var main = colorEffect[count].main;
        main.startColor = LevelManager.Instance.GetColored();
        
        colorEffect[count].Play();
        
        if (count < colorEffect.Length-1)
        {
            count++;
        }
        else
        {
            count = 0;
        }
    }
    public void IncreaseRubberColored()
    {
        if (actualScore == 0)
        {
            actualScore++;
            
        }
        else
        {
            if (isCombo)
            {
                actualScore += 10;
            }
            else
            {
                actualScore += 20;
            }
        }
        
        rubberColoredTotal++;
        currentRubberColored++;
        
    }
    public void DecreaseRubberColored()
    {
        actualScore -= 10;
        rubberColoredTotal--;
        
    }
    public void CheckCombo()
    {
        if (currentRubberColored >= previousRubberColored && currentRubberColored!= 0)
        {
            combo++;
            if (combo >= 3)
            {
                //UI
                isCombo = true;
                OnCombo?.Invoke(combo);
            }
        }
        else
        {
            isCombo = false;
            combo = 0;
            OnCombo?.Invoke(combo);
        }
        previousRubberColored = currentRubberColored;
        currentRubberColored = 0;
    }
    private IEnumerator RunningScoreUpdater()
    {
        while(true)
        {
            yield return new WaitForSeconds( 0.1f);
            int difference = actualScore - DisplayedScore;

            if (difference != 0)
            {        
                int proportionalTerm = difference / proportionalFactor;
            
                int moveStep = Mathf.Abs( proportionalTerm) + constantTerm;
            
                DisplayedScore = (int)Mathf.MoveTowards( DisplayedScore, actualScore, moveStep);

                OnScoreUpdate?.Invoke(DisplayedScore);
            }
        
    }
}
}
