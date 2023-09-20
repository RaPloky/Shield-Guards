using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] private string gameId = "5421933";
    [SerializeField] private bool testingMode;

    private void Awake()
    {
        InitializeAds();
    }

    private void InitializeAds()
    {
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(gameId, testingMode, this);
        }
    }

    public void OnInitializationComplete()
    {
        print("Ads initialized successfully");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError("Ads NOT initialized successfully");
    }

}
