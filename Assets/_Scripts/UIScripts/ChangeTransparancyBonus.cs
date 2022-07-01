using UnityEngine;
using UnityEngine.UI;

public class ChangeTransparancyBonus : MonoBehaviour
{
    [SerializeField] private SatelliteBehavior satellite;
    private Image _buttonImage;

    private void Awake()
    {
        _buttonImage = GetComponent<Image>();
    }
    private void Update()
    {
        if (satellite.isDicharged)
        {
            gameObject.SetActive(false);
        }
        float alphaValue = satellite.currentEnergyLevel / 100f;
        _buttonImage.color = new Color(_buttonImage.color.r, _buttonImage.color.g, _buttonImage.color.b, alphaValue);
    }

}
