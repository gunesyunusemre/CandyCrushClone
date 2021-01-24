using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp1 : MonoBehaviour
{
    //******Definitions************
    #region Definitions
    [SerializeField]
    private GameObject Ball5=null;
    [SerializeField]
    private GameObject Ball6=null;
    [SerializeField]
    private GameObject[] PinkBalls=null;
    [SerializeField]
    private Button NextButton=null;
    [SerializeField]
    private Text ScoreText=null;
    [SerializeField]
    private Text MoveText=null;
    #endregion
    //*****************************

    //********MonoBehaviour********
    void Start()
    {
        Ball5.GetComponent<Animator>().enabled = true;
        Ball6.GetComponent<Animator>().enabled = false;
        Ball5.GetComponent<Button>().enabled = true;
        Ball6.GetComponent<Button>().enabled = false;
        NextButton.gameObject.SetActive(false);

        Ball5.GetComponent<Button>().onClick.AddListener(OnClickBall5);
        Ball6.GetComponent<Button>().onClick.AddListener(OnClickBall6);
        NextButton.onClick.AddListener(OnClickNextPopUp);
    }
    //******************************

    //*******Balls******************
    #region Balls
    private void OnClickBall5()
    {
        Ball5.GetComponent<Animator>().enabled = false;
        Image image = Ball5.GetComponent<Image>();
        var color = image.color;
        color.a = 1f;
        image.color = color;

        Ball6.GetComponent<Animator>().enabled = true;


        Ball5.GetComponent<Button>().enabled = false;
        Ball6.GetComponent<Button>().enabled = true;
    }

    private void OnClickBall6()
    {
        Ball6.GetComponent<Animator>().enabled = false;
        Image image = Ball6.GetComponent<Image>();
        var color = image.color;
        color.a = 1f;
        image.color = color;

        Ball6.GetComponent<Button>().enabled = false;
        StartCoroutine(Swap(Ball5, Ball6));
        StartCoroutine(DestroyItems());

        ScoreText.text = "Score: 15";
        MoveText.text = "Move: 1";
        NextButton.gameObject.SetActive(true);
    }
    #endregion
    //*********************************

    //**********Move And Destroy Work*******
    #region Helper Func.
    IEnumerator Swap(GameObject a, GameObject b)
    {
        float movDuration = 0.1f;
        Vector3 aPosition = a.transform.position;
        Vector3 bPosition = b.transform.position;
        StartCoroutine(a.gameObject.transform.Move(bPosition, movDuration));
        StartCoroutine(b.gameObject.transform.Move(aPosition, movDuration));
        yield return new WaitForSeconds(movDuration);
    }

    IEnumerator DestroyItems()
    {
        foreach (GameObject i in PinkBalls)
        {
            yield return StartCoroutine(i.transform.Scale(Vector3.zero, 0.1f));
            Destroy(i.gameObject);
        }
    }
    #endregion
    //**************************************

    //**********Next Button*****************
    private void OnClickNextPopUp()
    {
        PopUps.popUps.IncreaseIndex();
    }
    //*************************************


}
