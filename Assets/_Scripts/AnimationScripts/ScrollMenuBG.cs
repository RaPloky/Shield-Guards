using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollMenuBG : MonoBehaviour
{
    [SerializeField] private RectTransform BG;
    [SerializeField] private float scrollSpeedX;
    [SerializeField] private float scrollLimitX;
    private float scrollStartPosX;

    private void Awake()
    {
        // Start position is x-position at first frame:
        scrollStartPosX = BG.anchoredPosition.x;
    }

    private void Update()
    {
        BG.anchoredPosition = new Vector2(BG.anchoredPosition.x + scrollSpeedX, BG.anchoredPosition.y);

        if (BG.anchoredPosition.x <= scrollLimitX)
        {
            BG.anchoredPosition = new Vector2(scrollStartPosX, BG.anchoredPosition.y);
        }
    }
}
