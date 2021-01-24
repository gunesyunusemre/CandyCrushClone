using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    //*********Panels definition***********
    #region Definitions
    [SerializeField]
    private GameObject MainMenuPanel=null;

    [SerializeField]
    private GameObject OptionsPanel = null;
    #endregion
    //*************************************

    //************Main Menu****************
    #region MainMenu
    public void OnClickPlayButton()
    {
        SceneManager.LoadScene("SelectLevelMenu");
    }

    int ClickCount = 0;
    public void OnClickClearPrefs()
    {
        ClickCount++;
        Debug.Log(ClickCount);
        if (ClickCount==5)
        {
            PlayerPrefs.DeleteAll();
            ClickCount = 0;
        }
        
    }

    public void OnClickCaycomButton()
    {
        Application.OpenURL("http://www.caycomtech.com/Games");
    }

    public void OnClickOptionButton()
    {
        MainMenuPanel.SetActive(!MainMenuPanel.activeSelf);
        OptionsPanel.SetActive(!OptionsPanel.activeSelf);
    }
    #endregion
    //****************************************


    //***************Option Menu**************
    #region OptionMenu
    public void OnClickPolicyButton()
    {
        Application.OpenURL("http://www.caycomtech.com/Privacy-Policy");
    }

    public void OnClickContactUsButton()
    {
        Application.OpenURL("http://www.caycomtech.com/Contact-Us");
    }

    public void OnClickTermsButton()
    {
        Application.OpenURL("http://www.caycomtech.com/Terms");
    }
    #endregion
    //**************************************

}
