using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    public static InGameUI gameUI;

    //**********Definitions*****************
    #region Definitions
    public Button PauseButton;
    public Button ReturnGameButton;
    public Button MainMenuButtonPausePanel;
    public GameObject PausePanel;

    public GameObject EndGamePanel;
    public Text WinOrLoseText;
    public Button MainMenuButtonEndGamePanel;
    public Button NextLevelButton;
    public Button SelectLevelMenu;

    public Text MoveText;
    public Text PointText;
    #endregion
    //**************************************

    //**********MonoBehaviour***************
    #region MonoBehaviour
    private void OnEnable()
    {
        InGameUI.gameUI = this;
    }

    void Start()
    {
        PauseButton.onClick.AddListener(PauseGame);
        ReturnGameButton.onClick.AddListener(PauseGame);
        MainMenuButtonPausePanel.onClick.AddListener(ReturnMainMenu);
        MainMenuButtonEndGamePanel.onClick.AddListener(ReturnMainMenu);
        NextLevelButton.onClick.AddListener(NextLevel);
        SelectLevelMenu.onClick.AddListener(LoadSelectLevelMenu);

        MoveText.gameObject.SetActive(WaveManager.waveManager.IsOpenMoveTask);
        WriteMoveText();

        PointText.gameObject.SetActive(WaveManager.waveManager.IsOpenPointTask);
        WritePointText();
    }
    #endregion
    //**************************************

    //**********Pause Game******************
    #region PauseGame
    private void PauseGame()
    {
        PausePanel.SetActive(!PausePanel.activeSelf);
        GameGrid.gameGrid.canPlay = !PausePanel.activeSelf;
        
        if (PausePanel.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private void ReturnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
    #endregion
    //**************************************

    //**********End Game********************
    #region EndGame
    public void EndGame()
    {
        EndGamePanel.SetActive(true);
        Time.timeScale = 0.1f;
    }

    private void LoadSelectLevelMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level" + (WaveManager.waveManager.LevelNumber + 1));
    }

    private void NextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level" + (WaveManager.waveManager.LevelNumber+1));
    }
    #endregion
    //**************************************

    //**********Move Text*******************
    #region MoveText
    public void WriteMoveText()
    {
        MoveText.text = string.Format("{0} / {1}", WaveManager.waveManager.MoveCount , WaveManager.waveManager.MaxMove);

        if (WaveManager.waveManager.MoveCount >= WaveManager.waveManager.MaxMove)
        {
            ChangeColorMoveText(Color.red);
            WaveManager.waveManager.OverMove();
        }
        else
        {
            ChangeColorMoveText(Color.green);
        }
    }

    private void ChangeColorMoveText(Color color)
    {
        MoveText.color = color;
    }
    #endregion
    //**************************************

    //**********Point Text******************
    #region Point Text
    public void WritePointText()
    {
        PointText.text= string.Format("{0} / {1}", WaveManager.waveManager.Point, WaveManager.waveManager.MaxPoint);

        if (WaveManager.waveManager.Point >= WaveManager.waveManager.MaxPoint)
        {
            ChangeColorPointText(Color.green);
            //Point Task Complete
        }
        else
        {
            ChangeColorPointText(Color.red);
        }
    }

    public void ChangeColorPointText(Color color)
    {
        PointText.color = color;
    }
    #endregion
    //**************************************


}
