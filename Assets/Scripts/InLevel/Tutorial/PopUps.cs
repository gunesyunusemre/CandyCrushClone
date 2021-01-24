using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUps : MonoBehaviour
{
    public static PopUps popUps;

    //******Definitions***************
    #region Definitions
    [SerializeField]
    private GameObject[] PopUpsGO=null;
    [SerializeField]
    private GameObject GameGrid=null;

    private int index = 0;
    #endregion
    //********************************

    //******MonoBehaviour*************
    #region MonoBehavior
    private void OnEnable()
    {
        PopUps.popUps = this;
    }

    void Start()
    {
        index = 0;
        if (PlayerPrefs.GetInt("firstPlay")==0)
        {
            SwichPopUp();
            GameGrid.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
        
    }
    #endregion
    //********************************

    //****Swich Tutorial workspace****
    #region Tutorials
    public void IncreaseIndex()
    {
        index++;
        if (index>=PopUpsGO.Length)
        {
            Debug.Log("End Tutorial!");
            gameObject.SetActive(false);
            GameGrid.SetActive(true);
            PlayerPrefs.SetInt("firstPlay", 1);
        }
        SwichPopUp();
    }

    private void SwichPopUp()
    {
        for (int i = 0; i < PopUpsGO.Length; i++)
        {
            if (i==index)
            {
                PopUpsGO[i].SetActive(true);
            }
            else
            {
                PopUpsGO[i].SetActive(false);
            }
        }
    }
    #endregion
    //********************************

}
