using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerCountDown : MonoBehaviour
{
    //*******Definitions*********
    #region Definitions
    private Image fillImg;
    private float timeMaxPoint=10;
    private float time;

    [SerializeField]
    private Text timeText = null;
    #endregion
    //***************************

    //*******MonoBehaviour*******
    #region MonoBehaviour
    void Start()
    {
        timeMaxPoint = WaveManager.waveManager.Time;

        fillImg = this.GetComponent<Image>();
        time = timeMaxPoint;

        fillImg.gameObject.SetActive(WaveManager.waveManager.IsOpenTimeTask);
        timeText.gameObject.SetActive(WaveManager.waveManager.IsOpenTimeTask);
    }

    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            fillImg.fillAmount = time / timeMaxPoint;
            timeText.text = ((int)time).ToString();
            WaveManager.waveManager.Time = (int)time;
        }
        else
        {
            WaveManager.waveManager.TimeUp();
        }
    }
    #endregion
    //***************************
}
