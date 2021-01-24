using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager waveManager;

    //***********Definitions******************
    #region Definitions
    //*********Level Task********************************
    #region LevelTask
    [SerializeField]
    private int levelNumber;
    public int LevelNumber { get { return levelNumber; } set { levelNumber = value; } }

    [SerializeField]
    private bool isOpenTimeTask = false;
    public bool IsOpenTimeTask { get { return isOpenTimeTask; } }

    [SerializeField]
    private int time;
    public int Time { get { return time; } set { time = value; } }

    [SerializeField]
    private bool isOpenMoveTask = false;
    public bool IsOpenMoveTask { get { return isOpenMoveTask; }}

    [SerializeField]
    private int maxMove = 0;
    public int MaxMove { get { return maxMove; } }

    [SerializeField]
    private bool isOpenPointTask = false;
    public bool IsOpenPointTask { get { return isOpenPointTask; } }

    [SerializeField]
    private int maxPoint = 0;
    public int MaxPoint { get { return maxPoint; } }

    [SerializeField]
    private GameObject[] balls;
    public GameObject[] Balls { get { return balls; } set { balls = value; } }
    #endregion
    //***************************************************

    //*********Point Gain********************************
    #region PointGain
    [SerializeField]
    private int pointMultiple = 0;
    public int PointMultiple { get { return pointMultiple; } set { pointMultiple = value; } }
    #endregion
    //***************************************************

    //*********Definitions in level for Client***********
    #region ClientDefinition
    private int point = 0;
    public int Point { get { return point; } set { point = value; } }

    private int moveCount=0;
    public int MoveCount { get { return moveCount; } set { moveCount = value; } }

    private int wrongMoveCount=0;
    public int WrongMoveCount { get { return wrongMoveCount; } set { wrongMoveCount = value; } }

    private bool endLevel;
    #endregion
    //***************************************************
    #endregion
    //****************************************

    //***********MonoBehaviour****************
    private void OnEnable()
    {
        WaveManager.waveManager = this;
        endLevel = false;
    }
    //****************************************

    //***********Time Task********************
    #region TimeTask
    public void TimeUp()
    {
        if (!isOpenTimeTask)
            return;

        if (!endLevel)
        {
            Debug.Log("Time Up");
            EndLevel();
            endLevel = true;
        }
    }
    #endregion
    //****************************************

    //***********Move Task********************
    #region MoveTask
    public void OverMove()
    {
        if (!isOpenMoveTask)
            return;

        if (!endLevel)
        {
            Debug.Log("Your right to move is over!");
            Invoke("EndGame", 0.5f);
            endLevel = true;
        }
    }
    #endregion
    //****************************************

    //***********Level End Screen**************
    #region Level End
    private void EndLevel()
    {
        Debug.Log("Level End!");
        CheckTask();
        InGameUI.gameUI.EndGame();
    }

    private void CheckTask()
    {
        if (isOpenPointTask & IsOpenTimeTask & isOpenMoveTask)
        {
            if (moveCount < maxMove && point >= maxPoint)
            {
                InGameUI.gameUI.WinOrLoseText.text = "Congratulation!";
                Debug.Log("Congratulation!");
                if (PlayerPrefs.GetInt("level") >= levelNumber)
                    return;

                PlayerPrefs.SetInt("level", levelNumber);
            }
            else
            {
                InGameUI.gameUI.NextLevelButton.gameObject.SetActive(false);
                InGameUI.gameUI.WinOrLoseText.text = "Try Again!";
                Debug.Log("Try Again!");
            }
        }
        else if (isOpenPointTask & IsOpenTimeTask)
        {
            if (point>=maxPoint)
            {
                InGameUI.gameUI.WinOrLoseText.text = "Congratulation!";
                Debug.Log("Congratulation!");
                if (PlayerPrefs.GetInt("level") >= levelNumber)
                    return;

                PlayerPrefs.SetInt("level", levelNumber);
            }
            else
            {
                InGameUI.gameUI.NextLevelButton.gameObject.SetActive(false);
                InGameUI.gameUI.WinOrLoseText.text = "Try Again!";
                Debug.Log("Try Again!");
            }
        }
        else if (isOpenMoveTask && isOpenPointTask)
        {
            if (point >= maxPoint)
            {
                InGameUI.gameUI.WinOrLoseText.text = "Congratulation!";
                Debug.Log("Congratulation!");
                if (PlayerPrefs.GetInt("level") >= levelNumber)
                    return;

                PlayerPrefs.SetInt("level", levelNumber);
            }
            else
            {
                InGameUI.gameUI.NextLevelButton.gameObject.SetActive(false);
                InGameUI.gameUI.WinOrLoseText.text = "Try Again!";
                Debug.Log("Try Again!");
            }
        }
    }
    #endregion
    //****************************************

}
