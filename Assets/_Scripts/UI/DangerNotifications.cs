using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DangerNotifications : MonoBehaviour
{
    [SerializeField] private Spawner leftUfo;
    [SerializeField] private Spawner rightUfo;
    [SerializeField] private Spawner leftMeteor;
    [SerializeField] private Spawner rightMeteor;
    [Header("-----")]
    [SerializeField] private Animator leftUfoDangerAnim;
    [SerializeField] private Animator rightUfoDangerAnim;
    [SerializeField] private Animator leftMeteorDangerAnim;
    [SerializeField] private Animator rightMeteorDangerAnim;
}
