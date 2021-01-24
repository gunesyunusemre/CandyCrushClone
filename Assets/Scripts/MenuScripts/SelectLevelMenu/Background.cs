using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{

    private void OnMouseDown()
    {
        if (OnTouchDownEventHandler != null)
        {
            OnTouchDownEventHandler(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    public delegate void OnTouchDown(Vector3 firstTouch);
    public static event OnTouchDown OnTouchDownEventHandler;

    private void OnMouseUp()
    {
        if (OnTouchUpEventHandler != null)
        {
            OnTouchUpEventHandler(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    public delegate void OnTouchUp(Vector3 finalTouch);
    public static event OnTouchUp OnTouchUpEventHandler;


}
