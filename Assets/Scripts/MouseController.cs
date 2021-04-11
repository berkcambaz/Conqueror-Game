using System.Collections;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public float zoomMin;
    public float zoomMax;
    public float scrollSpeed;
    private float scroll;

    private Vector3 dragStartPos;
    private bool mouseMoved = false;

    void Start()
    {
        scroll = Game.Instance.cam.orthographicSize;
    }

    void Update()
    {
        Vector2 targetTile = Game.Instance.cam.ScreenToWorldPoint(Input.mousePosition);
        UIManager.Instance.tileSelect.transform.position = new Vector3(Mathf.RoundToInt(targetTile.x - 0.5f), Mathf.RoundToInt(targetTile.y - 0.5f), 0f);

        Game.Instance.cam.orthographicSize = Mathf.Clamp(Mathf.Lerp(Game.Instance.cam.orthographicSize, scroll, Time.deltaTime * 5f), zoomMin, zoomMax);
    }

    public void OnPointerDown()
    {
        dragStartPos = Input.mousePosition;
        StartCoroutine("OnPointerMove");
    }

    private IEnumerator OnPointerMove()
    {
        while (true)
        {
            Vector3 deltaPos = dragStartPos - Input.mousePosition;

            // If mouse has moved after clicking
            if (deltaPos != Vector3.zero)
            {
                mouseMoved = true;
                deltaPos = Game.Instance.cam.ScreenToWorldPoint(dragStartPos) - Game.Instance.cam.ScreenToWorldPoint(Input.mousePosition);
                Game.Instance.cam.transform.position += deltaPos;
                dragStartPos = Input.mousePosition;
            }
            yield return 0;
        }
    }

    public void OnPointerUp()
    {
        if (!mouseMoved)
        {
            // Select the target tile
            Vector2 targetTile = Game.Instance.cam.ScreenToWorldPoint(Input.mousePosition);
            Game.Instance.map.SelectTile(Mathf.RoundToInt(targetTile.x - 0.5f), Mathf.RoundToInt(targetTile.y - 0.5f));
        }
        mouseMoved = false;
        StopCoroutine("OnPointerMove");
    }

    public void OnScroll()
    {
        scroll = Mathf.Clamp(scroll + -Input.GetAxis("Mouse ScrollWheel") * scrollSpeed, zoomMin, zoomMax);
    }
}
