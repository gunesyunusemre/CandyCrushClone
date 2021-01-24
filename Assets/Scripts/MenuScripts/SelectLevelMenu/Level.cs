using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            Ray ray = new Ray(mousePos2D, new Vector3(0, 0, 1));
            RaycastHit2D hitInfo = Physics2D.GetRayIntersection(ray);

            float distance = Vector2.Distance(mousePos2D, this.transform.position);

            if (hitInfo.collider != null)
            {
                if (distance>this.gameObject.GetComponent<CircleCollider2D>().radius)
                    return;

                SceneManager.LoadScene(this.gameObject.name);
                Debug.Log(this.gameObject.name);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, this.gameObject.GetComponent<CircleCollider2D>().radius);
    }
}
