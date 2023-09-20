using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class LoadBanner : MonoBehaviour
{
    public static LoadBanner Instance;

    [SerializeField] private string unitID;

    private readonly BannerPosition bannerPos = BannerPosition.BOTTOM_CENTER;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Advertisement.Banner.SetPosition(bannerPos);
    }

    public void LoadTheBanner()
    {
        BannerLoadOptions bannerLoadOptions = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerLoadedError
        };

        Advertisement.Banner.Load(unitID, bannerLoadOptions);
    }

    private void OnBannerLoaded()
    {
        print("Banner loaded");
        ShowBannerAd();
    }

    private void OnBannerLoadedError(string error)
    {
        Debug.LogError("Banner failed to load: " + error);
    }

    public void ShowBannerAd()
    {
        BannerOptions bannerLoadOptions = new BannerOptions
        {
            showCallback = OnBannerShow,
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden
        };

        Advertisement.Banner.Show(unitID, bannerLoadOptions);
    }

    public void HideBanner()
    {
        Advertisement.Banner.Hide();
    }

    private void OnBannerShow()
    {

    }
    private void OnBannerClicked()
    {

    }
    private void OnBannerHidden()
    {

    }
}
