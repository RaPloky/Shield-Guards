using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSatellite : MonoBehaviour
{
    public static void PlayAnimation(Animator anim)
    {
        anim.Play("EngineWarp");
    }
}
