using UnityEngine;

public class GainEnergyOnTouch : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnMouseDown()
    {
        // Gain energy!
        _animator.Play("Touch", -1, 0f);
    }
}
