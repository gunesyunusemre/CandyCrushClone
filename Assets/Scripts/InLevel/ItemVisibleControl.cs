using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemVisibleControl : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }
}
