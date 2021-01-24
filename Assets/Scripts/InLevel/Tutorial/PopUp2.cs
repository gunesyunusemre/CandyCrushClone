using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp2 : MonoBehaviour
{
    //**********Definitions***********
    #region Definitions
    [SerializeField]
    private Image fillImg=null;
    [SerializeField]
    Text timeText = null;
    [SerializeField]
    private GameObject UpPanel=null;
    [SerializeField]
    private Button NextButton=null;

    [SerializeField]
    float timeMaxPoint = 10;
    float time;
    #endregion
    //********************************



    //**********MonoBehavior**********
    #region MonoBehavior
    private void Start()
    {
        time = timeMaxPoint;

        UpPanel.SetActive(false);
        NextButton.gameObject.SetActive(false);

        NextButton.onClick.AddListener(OnClickNextButton);
    }

    private void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            fillImg.fillAmount = time / timeMaxPoint;
            timeText.text = ((int)time).ToString();
        }
        else
        {
            UpPanel.SetActive(true);
            NextButton.gameObject.SetActive(true);
        }
    }
    #endregion
    //*******************************

    //*******Buttons*****************
    private void OnClickNextButton()
    {
        PopUps.popUps.IncreaseIndex();
    }
    //*******************************
}
