using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp3 : MonoBehaviour
{
    //*********Definitions**********
    #region Definitions
    [SerializeField]
    private Button MoveButton = null;
    [SerializeField]
    private Button NextButton = null;
    [SerializeField]
    private Text MoveText = null;
    [SerializeField]
    private int maxCount = 0;
    private int count=0;

    [SerializeField]
    private GameObject UpPanel=null;
    #endregion
    //******************************

    //*********MonoBehaviour********
    private void Start()
    {
        MoveButton.onClick.AddListener(OnClickMove);
        NextButton.onClick.AddListener(OnClickNext);
        MoveText.text = "Move: " + count + "/" + maxCount;
    }
    //*****************************

    //*********Buttons*************
    #region Buttons
    private void OnClickMove()
    {
        count++;
        MoveText.text = "Move: " + count + "/" + maxCount;
        if (count>=maxCount)
        {
            MoveText.color = Color.red;
            UpPanel.SetActive(true);
            NextButton.gameObject.SetActive(true);
            MoveButton.gameObject.SetActive(false);
        }
    }

    private void OnClickNext()
    {
        PopUps.popUps.IncreaseIndex();
    }
    #endregion
    //****************************
}
