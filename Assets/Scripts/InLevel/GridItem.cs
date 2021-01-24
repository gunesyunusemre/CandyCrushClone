using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridItem : MonoBehaviour
{

    //***********ItemPositionIndex****************
    #region PositionIndex
    public int x
    {
        get;
        private set;
    }

    public int y
    {
        get;
        private set;
    }
    #endregion
    //********************************************

    /// <summary>
    /// Tag/ID/TYPE
    /// </summary>
    [HideInInspector]
    public int id;

    //***********BallsChangeposition**************
    public void OnItemPositionChanged(int newX, int newY)
    {
        x = newX;
        y = newY;
        gameObject.name = string.Format("Ball [{0}] [{1}]", x, y);
    }
    //********************************************

    //***********Ball's OnClick Event*************
    private void OnMouseDown()
    {
        if (OnMouseOverItemEventHandler!=null)
        {
            OnMouseOverItemEventHandler(this);
        }
    }

    public delegate void OnMouseOverItem(GridItem item);
    public static event OnMouseOverItem OnMouseOverItemEventHandler;
    //********************************************

}
