using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameGrid : MonoBehaviour
{
    public static GameGrid gameGrid;

    //***********Definitions****************
    #region Definitions
    public int xSize, ySize;
    public float ballWidth=1;
    private GameObject[] balls;
    private GridItem[,] items;
    private GridItem currentlySelectedItem;
    public static int minItemsForMatch = 3;
    public float delayBetweenMatches = 0.5f;
    public bool canPlay;
    #endregion
    //**************************************

    //***********MonoBehaviour**************
    #region MonoBehaviour
    private void OnEnable()
    {
        GameGrid.gameGrid = this;
    }

    private void Start()
    {
        canPlay = true;
        GetBall();
        FillGrid();
        ClearGrid();
        GridItem.OnMouseOverItemEventHandler += OnMouseOverItem;
    }

    private void OnDisable()
    {
        GridItem.OnMouseOverItemEventHandler -= OnMouseOverItem;
    }
    #endregion
    //**************************************

    //***********Level Mechanics************
    #region StartMechanics
    private void GetBall()
    {
        balls = WaveManager.waveManager.Balls;
        //balls = Resources.LoadAll<GameObject>("Prefabs");
        for (int i = 0; i < balls.Length; i++)
        {
            balls[i].GetComponent<GridItem>().id = i;
        }
    }

    private void FillGrid()
    {
        items = new GridItem[xSize, ySize];
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                items[x, y] = InstantiateBall(x, y);
            }
        }
    }

    private void ClearGrid()
    {
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                MatchInfo matchInfo = GetMatchInformation(items[x, y]);
                if (matchInfo.validMatch)
                {
                    Destroy(items[x, y].gameObject);
                    items[x, y] = InstantiateBall(x, y);
                    y--;
                }
            }
        }
    }
    #endregion

    #region CreateDestroyMechanics
    private GridItem InstantiateBall(int x, int y)
    {
        GameObject randomBall = balls[Random.Range(0, balls.Length)];
        GridItem newBall = ((GameObject)Instantiate(randomBall, new Vector3(x*ballWidth, y + (10+x)), Quaternion.identity))
            .GetComponent<GridItem>();
        newBall.OnItemPositionChanged(x, y);
        return newBall;
    }

    IEnumerator DestroyItems(List<GridItem> items)
    {
        foreach (GridItem i in items)
        {
            yield return StartCoroutine(i.transform.Scale(Vector3.zero, 0.1f));
            Destroy(i.gameObject);
        }
    }
    #endregion

    #region MoveMechanics
    private void OnMouseOverItem(GridItem item)
    {
        if (currentlySelectedItem==item || !canPlay)
        {
            return;
        }

        if (currentlySelectedItem ==null)
        {
            currentlySelectedItem = item;
        }
        else
        {
            float xDiff = Mathf.Abs(item.x - currentlySelectedItem.x);
            float yDiff = Mathf.Abs(item.y - currentlySelectedItem.y);

            if (xDiff+yDiff==1)
            {
                StartCoroutine(TryMatch(currentlySelectedItem, item));
            }
            else
            {
                Debug.LogError("This move is invalid");
            }
            currentlySelectedItem = null;
        }
    }
    #endregion
    //**************************************

    //***********MoveWork*******************
    #region IsMoveSuccess
    IEnumerator TryMatch(GridItem a, GridItem b)
    {
        //Move counter++
        WaveManager.waveManager.MoveCount++;
        InGameUI.gameUI.WriteMoveText();


        canPlay = false;
        yield return StartCoroutine(Swap(a, b));
        MatchInfo matchA = GetMatchInformation(a);
        MatchInfo matchB = GetMatchInformation(b);
        if (!matchA.validMatch && !matchB.validMatch)
        {
            //wrong move counter ++
            WaveManager.waveManager.WrongMoveCount++;
            yield return StartCoroutine(Swap(a, b));
            canPlay = true;
            yield break;
        }
        if (matchA.validMatch)
        {
            //point gain
            WaveManager.waveManager.Point += (matchA.match.Count * WaveManager.waveManager.PointMultiple);
            InGameUI.gameUI.WritePointText();

            yield return StartCoroutine(DestroyItems(matchA.match));
            yield return new WaitForSeconds(delayBetweenMatches);
            yield return new WaitForSeconds(delayBetweenMatches);
            yield return StartCoroutine(UpdateGridAfterMatch(matchA));
        }
        else if (matchB.validMatch)
        {
            //point gain
            WaveManager.waveManager.Point += (matchB.match.Count * WaveManager.waveManager.PointMultiple);
            InGameUI.gameUI.WritePointText();

            yield return StartCoroutine(DestroyItems(matchB.match));
            yield return new WaitForSeconds(delayBetweenMatches);
            yield return new WaitForSeconds(delayBetweenMatches);
            yield return StartCoroutine(UpdateGridAfterMatch(matchB));
        }

        yield return new WaitForSeconds(delayBetweenMatches);
        canPlay = true;
    }
    #endregion

    #region Move
    IEnumerator Swap(GridItem a, GridItem b)
    {
        ChangeRigidBodyStatus(false);
        float movDuration = 0.1f;
        Vector3 aPosition = a.transform.position;
        Vector3 bPosition = b.transform.position;
        StartCoroutine(a.gameObject.transform.Move(bPosition, movDuration));
        StartCoroutine(b.gameObject.transform.Move(aPosition, movDuration));
        yield return new WaitForSeconds(movDuration);
        SwapIndices(a, b);
        ChangeRigidBodyStatus(true);
    }

    private void SwapIndices(GridItem a, GridItem b)
    {
        GridItem tempA = items[a.x, a.y];
        items[a.x, a.y] = b;
        items[b.x, b.y] = tempA;
        int bOldX = b.x; int bOldY = b.y;
        b.OnItemPositionChanged(a.x, a.y);
        a.OnItemPositionChanged(bOldX, bOldY);
    }

    private void ChangeRigidBodyStatus(bool status)
    {
        foreach (GridItem g in items)
        {
            g.gameObject.GetComponent<Rigidbody2D>().isKinematic = !status;
        }
    }
    #endregion
    //**************************************

    //***********Create New Balls***********
    #region CreateNewBalls
    IEnumerator UpdateGridAfterMatch(MatchInfo match)
    {
        if (match.matchStartingY==match.matchEndingY)
        {
            //match horizontal
            for (int x = match.matchStartingX; x <= match.matchEndingX; x++)
            {
                for (int y = match.matchStartingY; y < ySize-1; y++)
                {
                    GridItem upperIndex = items[x, y + 1];
                    GridItem current = items[x, y];
                    items[x, y] = upperIndex;
                    items[x, y + 1] = current;
                    items[x, y].OnItemPositionChanged(items[x, y].x, items[x, y].y - 1);
                }
                items[x, ySize-1] = InstantiateBall(x, ySize - 1);
            }
        }
        else if (match.matchStartingX==match.matchEndingX)
        {
            //match vertical
            int matchHeight = 1 + (match.matchEndingY - match.matchStartingY);
            for (int y = match.matchStartingY+matchHeight; y <= ySize-1; y++)
            {
                GridItem lowerIndex = items[match.matchEndingX, y - matchHeight];
                GridItem current = items[match.matchEndingX, y];
                items[match.matchStartingX, y - matchHeight] = current;
                items[match.matchStartingX, y] = lowerIndex;
            }
            for (int y = 0; y < ySize-matchHeight; y++)
            {
                items[match.matchEndingX, y].OnItemPositionChanged(match.matchStartingX, y);
            }
            for (int i = 0; i < match.match.Count; i++)
            {
                items[match.matchStartingX, (ySize - 1) - i] = InstantiateBall(match.matchStartingX, (ySize - 1) - i);
            }
        }

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                MatchInfo matchInfo = GetMatchInformation(items[x, y]);
                if (matchInfo.validMatch)
                {
                    //point gain
                    WaveManager.waveManager.Point += (matchInfo.match.Count * WaveManager.waveManager.PointMultiple);
                    InGameUI.gameUI.WritePointText();

                    yield return StartCoroutine(DestroyItems(matchInfo.match));
                    yield return new WaitForSeconds(delayBetweenMatches);
                    yield return StartCoroutine(UpdateGridAfterMatch(matchInfo));
                }
            }
        }
    }
    #endregion
    //**************************************

    //***********MatchWorks*****************
    #region Match
    private MatchInfo GetMatchInformation(GridItem item)
    {
        MatchInfo m = new MatchInfo();
        m.match = null;
        List<GridItem> hMatch = SearchHorizontally(item);
        List<GridItem> vMatch = SearchVertically(item);
        if (hMatch.Count >= minItemsForMatch && hMatch.Count > vMatch.Count)
        {
            //horizontal match information
            m.matchStartingX = GetMinimumX(hMatch);
            m.matchEndingX = GetMaximumX(hMatch);
            m.matchStartingY = m.matchEndingY = hMatch[0].y;
            m.match = hMatch;
        }
        else if (vMatch.Count >= minItemsForMatch)
        {
            //vertical match information
            m.matchStartingY = GetMinimumY(vMatch);
            m.matchEndingY = GetMaximumY(vMatch);
            m.matchStartingX = m.matchEndingX = vMatch[0].x;
            m.match = vMatch;
        }

        return m;
    }
    #endregion

    #region Search
    List<GridItem> SearchHorizontally(GridItem item)
    {
        List<GridItem> hItems = new List<GridItem> { item };
        int left = item.x - 1;
        int right = item.x + 1;
        while (left >= 0 && items[left, item.y].id==item.id)
        {
            hItems.Add(items[left, item.y]);
            left--;
        }
        while (right<xSize && items[right, item.y].id==item.id)
        {
            hItems.Add(items[right, item.y]);
            right++;
        }
        return hItems;
    }

    List<GridItem> SearchVertically(GridItem item)
    {
        List<GridItem> vItems = new List<GridItem> { item };
        int lower = item.y - 1;
        int upper = item.y + 1;
        while (lower >= 0 && items[item.x, lower].id == item.id)
        {
            vItems.Add(items[item.x, lower]);
            lower--;
        }
        while (upper < ySize && items[item.x, upper].id == item.id)
        {
            vItems.Add(items[item.x, upper]);
            upper++;
        }
        return vItems;
    }
    #endregion

    #region GetMatchMinMaxIndex
    private int GetMinimumX(List<GridItem> items)
    {
        float[] indices = new float[items.Count];
        for (int i = 0; i < indices.Length; i++)
        {
            indices[i] = items[i].x;
        }
        return (int)Mathf.Min(indices);
    }

    private int GetMaximumX(List<GridItem> items)
    {
        float[] indices = new float[items.Count];
        for (int i = 0; i < indices.Length; i++)
        {
            indices[i] = items[i].x;
        }
        return (int)Mathf.Max(indices);
    }

    private int GetMinimumY(List<GridItem> items)
    {
        float[] indices = new float[items.Count];
        for (int i = 0; i < indices.Length; i++)
        {
            indices[i] = items[i].y;
        }
        return (int)Mathf.Min(indices);
    }

    private int GetMaximumY(List<GridItem> items)
    {
        float[] indices = new float[items.Count];
        for (int i = 0; i < indices.Length; i++)
        {
            indices[i] = items[i].y;
        }
        return (int)Mathf.Max(indices);
    }
    #endregion
    //**************************************

}
