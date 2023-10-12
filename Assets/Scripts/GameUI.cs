using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text tapToStart;
    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text comboText;
    [SerializeField] private Image currentLevelImage;
    [SerializeField] private Image nextLevelImage;
    [SerializeField] private Image[] roundsOutImage;

    [SerializeField] private GameObject gameOverPanel;

    [Header("Color")]
    
    [SerializeField] private Color levelSelectedIn;
    [SerializeField] private Color levelDefaultIn;

    [SerializeField] private Color roundBackgroundSelected;
    [SerializeField] private Color roundBackgroundDefault;
    [SerializeField] private Color roundSelected;
    [SerializeField] private Color roundDefault;
    [SerializeField] private Color roundCleared;
    
    private void Awake()
    {
        LevelManager.Instance.OnLevelChange += SetLevel;
    }
    private void Start()
    {
        GameManager.Instance.OnScoreUpdate += UpdateScore;
        GameManager.Instance.OnGameOver += OnGameOver;
        GameManager.Instance.OnGamePlaying += OnGamePlaying;
        GameManager.Instance.OnNewLevel += OnNewLevel;
        GameManager.Instance.OnCombo += SetCombo;

        SoundManager.Instance.Play(SoundType.BGM);
    }
    private void OnNewLevel()
    {
        tapToStart.gameObject.SetActive(true);
    }
    private void OnGameOver()
    {
        gameOverPanel.SetActive(true);
    }
    private void OnGamePlaying()
    {
        tapToStart.gameObject.SetActive(false);
    }
    private void UpdateScore(int newScore)
    {
        //score.text = GameManager.Instance.DisplayedScore.ToString();
        score.text = newScore.ToString();
    }
    private void SetCombo(int combo)
    {
        if (combo == 0)
        {
            comboText.gameObject.SetActive(false);
        }
        else
        {
            comboText.gameObject.SetActive(true);
            comboText.text = "COMBO x" + combo;
        }
        
    }
    
    private void SetLevel(int currentLevel, int currentRound)
    {
        comboText.gameObject.SetActive(false);
        gameOverPanel.SetActive(false);
        

        TMP_Text currentLevelText = currentLevelImage.GetComponentInChildren<TMP_Text>();
        currentLevelText.text = (currentLevel + 1).ToString();
        TMP_Text nextLevelText = nextLevelImage.GetComponentInChildren<TMP_Text>();
        nextLevelText.text = (currentLevel + 2).ToString();

        currentLevelImage.color = levelSelectedIn;
        nextLevelImage.color = levelDefaultIn;

        for (int i=0; i<roundsOutImage.Length; i++)
        {
            if (i==currentRound)
            {
                //selected
                roundsOutImage[currentRound].color = LevelManager.Instance.GetColored();
                roundsOutImage[currentRound].GetComponentsInChildren<Image>()[1].color = roundSelected;
            }
            else if (i<currentRound)
            {
                //Cleared rounds
                roundsOutImage[i].color = roundBackgroundDefault;
                roundsOutImage[i].GetComponentsInChildren<Image>()[1].color = roundCleared;
            }
            else
            {
                //default
                roundsOutImage[i].color = roundBackgroundDefault;
                roundsOutImage[i].GetComponentsInChildren<Image>()[1].color = roundDefault;
            }
        }
       
    }
}
