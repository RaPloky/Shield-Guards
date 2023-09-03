using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyAlarm
{
    None,
    UFO,
    Meteor,
    Carrier
}

public class AlarmManager : MonoBehaviour
{
    private bool _isUFO_AlarmActive = false;
    private bool _isMeteor_AlarmActive = false;
    private bool _isCarrier_AlarmActive = false;

    private int _activeUFO_Count = 0;
    private int _activeMeteor_Count = 0;
    // carrier count is not present bcz it's possible
    // to spawn only one carrier at the time

    [SerializeField] private AudioSource ufoSource;
    [SerializeField] private AudioSource meteorSource;
    [SerializeField] private AudioSource carrierSource;

    private void OnEnable()
    {
        EventManager.OnEnemyDeployed += EnableAlarm;
        EventManager.OnEnemyReset += DisableAlarm;
    }
    private void OnDisable()
    {
        EventManager.OnEnemyDeployed -= EnableAlarm;
        EventManager.OnEnemyReset -= DisableAlarm;
    }

    private void EnableAlarm(EnemyAlarm alarmType)
    {
        switch (alarmType)
        {
            case EnemyAlarm.UFO:
                if (!_isUFO_AlarmActive)
                {
                    ufoSource.Play();
                    _isUFO_AlarmActive = true;
                }
                _activeUFO_Count = Mathf.Clamp(++_activeUFO_Count, 0, 3);
                break;

            case EnemyAlarm.Meteor:
                if (!_isMeteor_AlarmActive)
                {
                    meteorSource.Play();
                    _isMeteor_AlarmActive = true;
                }
                _activeMeteor_Count = Mathf.Clamp(++_activeMeteor_Count, 0, 3);
                break;

            case EnemyAlarm.Carrier:
                if (!_isCarrier_AlarmActive)
                {
                    carrierSource.Play();
                    _isCarrier_AlarmActive = true;
                }
                break;

            case EnemyAlarm.None:
                break;
        }
    }

    private void DisableAlarm(EnemyAlarm alarmType)
    {
        switch (alarmType)
        {
            case EnemyAlarm.UFO:
                _activeUFO_Count = Mathf.Clamp(--_activeUFO_Count, 0, 3);
                if (_isUFO_AlarmActive && Mathf.Approximately(_activeUFO_Count, 0))
                {
                    ufoSource.Stop();
                    _isUFO_AlarmActive = false;
                }
                break;

            case EnemyAlarm.Meteor:
                _activeMeteor_Count = Mathf.Clamp(--_activeMeteor_Count, 0, 3);
                if (_isMeteor_AlarmActive && Mathf.Approximately(_activeMeteor_Count, 0))
                {
                    meteorSource.Stop();
                    _isMeteor_AlarmActive = false;
                }
                break;

            case EnemyAlarm.Carrier:
                if (_isCarrier_AlarmActive)
                {
                    carrierSource.Stop();
                    _isCarrier_AlarmActive = false;
                }
                break;

            case EnemyAlarm.None:
                break;
        }
    }
}
