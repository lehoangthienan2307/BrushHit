using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject levelSelect;
    [SerializeField] private Button startGame;
    public void OnPlay()
    {
        playButton.gameObject.SetActive(false);
        settingButton.gameObject.SetActive(false);
        levelSelect.SetActive(true);
    }
    public void OnBack()
    {
        levelSelect.SetActive(false);
        playButton.gameObject.SetActive(true);
        settingButton.gameObject.SetActive(true);
    }
    public void OnLevelSelect()
    {
        SceneManager.LoadScene("MainGame");
    }
}
