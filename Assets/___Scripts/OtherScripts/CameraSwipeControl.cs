using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraSwipeControl : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    public GameObject PivotPoint;
    public GameObject CameraPos;
    public GameObject PauseMenuUI;
    [Range(0, 0.5f)] public float moveTime;

    private readonly float _swipeAngle = 120;
    private void Start()
    {
        CameraPos.transform.LookAt(PivotPoint.transform);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (PauseMenu.isGamePaused) return;
        if ((Mathf.Abs(eventData.delta.x)) > (Mathf.Abs(eventData.delta.y)))
        {
            if (eventData.delta.x > 0)
            {
                CameraPos.transform.RotateAround(PivotPoint.transform.position, Vector3.up, _swipeAngle);
            }
            else
            {
                CameraPos.transform.RotateAround(PivotPoint.transform.position, Vector3.down, _swipeAngle);
            }
        }
    }
    public void OnDrag(PointerEventData eventData) { }
}

