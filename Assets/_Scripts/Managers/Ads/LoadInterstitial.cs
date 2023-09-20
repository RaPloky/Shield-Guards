using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class LoadInterstitial : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private string unitID;

    public void LoadAd()
    {
        Advertisement.Load(unitID, this);
    }

    public void ShowAd()
    {
        Advertisement.Show(unitID, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        print("Interstitial loaded");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogError("Interstitial NOT loaded");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        print("Interstitial clicked");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        print("Interstitial show completed");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogError("Interstitial show failure");
    }

    public void OnUnityAdsShowStart(string placementId)
    {        
        print("Interstitial started"); 
    }
}
