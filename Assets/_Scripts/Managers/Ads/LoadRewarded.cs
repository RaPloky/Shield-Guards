using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class LoadRewarded : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
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
        if (placementId.Equals(unitID))
            print("Rewarded loaded");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogError("Rewarded NOT loaded");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        print("Rewarded clicked");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId.Equals(unitID) && showCompletionState.Equals(UnityAdsCompletionState.COMPLETED))
            print("Rewarded show completed! Take rewards");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogError("Rewarded show failure");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        print("Rewarded started");
    }
}
