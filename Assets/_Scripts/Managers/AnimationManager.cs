using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] private Animator globalAnimator;
    [SerializeField] private float menuOutDelay;

    private GlitchAnimationController _glitchController;

    private void Start()
    {
        _glitchController = GlitchAnimationController.Instance;
        MenuIn();
    }

    public void MenuIn()
    {
        _glitchController.SingleDriftAndDigital(0.5f, 0.5f);
        globalAnimator.SetTrigger("MenuIn");
    }

    public void MenuOut()
    {
        StartCoroutine(_glitchController.DigitalFadeInAndOut(0.8f, 0.2f));
        globalAnimator.SetTrigger("MenuOut");
        Invoke(nameof(StartGame), menuOutDelay);
    }

    private void StartGame()
    {
        GameManager.Instance.StartGame();
    }
}
