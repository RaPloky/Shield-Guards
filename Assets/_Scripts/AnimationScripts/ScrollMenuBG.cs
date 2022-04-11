﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollMenuBG : MonoBehaviour
{
    [SerializeField] RectTransform BG;
    [SerializeField] float scrollSpeedX;
    [SerializeField] float scrollLimitX;
    private float _scrollStartPosX;

    private void Awake()
    {
        // Start position is x-position at first frame:
        _scrollStartPosX = BG.anchoredPosition.x;
    }

    private void Update()
    {
        BG.anchoredPosition = new Vector2(BG.anchoredPosition.x + scrollSpeedX, BG.anchoredPosition.y);

        if (BG.anchoredPosition.x <= scrollLimitX)
        {
            BG.anchoredPosition = new Vector2(_scrollStartPosX, BG.anchoredPosition.y);
        }
    }
}