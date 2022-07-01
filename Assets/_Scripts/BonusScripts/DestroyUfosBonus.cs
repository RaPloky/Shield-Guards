using System.Collections;
using UnityEngine;

public class DestroyUfosBonus : BonusManager
{
    public SatelliteBehavior satelDestroyer;
    public UFOBehavior ufoPrefab;

    public int destroyedUfosCurrCount;
    public int destroyedUfosCountGoal;
    public float spawnFreezeTime;
    public int damageDebuff;

    private bool _isDebuffed = false;

    private void Update()
    {
        if (satelDestroyer.isDicharged && !_isDebuffed)
        {
            _isDebuffed = true;
            DebuffDamageToUfos();
            return;
        }
    }
    private void DebuffDamageToUfos()
    {
        ufoPrefab.damageToEnemy -= damageDebuff;
    }
    public override void ActivateBonus()
    {
        if (bonusCounter == 0 || PauseMenu.isGamePaused || satelDestroyer.isDicharged) return;

        GameObject[] ufos = GameObject.FindGameObjectsWithTag("UFO");

        // If there's no UFOs, bonus can't be used:
        if (ufos.Length == 0) return;

        for (byte i = 0; i < ufos.Length; i++)
        {
            ufos[i].GetComponentInParent<EnemyCommonValues>().DestroyAndStartSpawn(ufos[i].transform.parent.transform);
            StartCoroutine(FreezeSpawnTime(spawnFreezeTime, ufos[i].transform.parent.transform.gameObject));
        }
        bonusCounter--;
    }
    private IEnumerator FreezeSpawnTime(float freezeTime, GameObject spawner)
    {
        // Freezes UFO spawn:
        spawner.GetComponent<EnemySpawnManager>().spawnFreezed = true;
        yield return new WaitForSeconds(freezeTime);
        // Unfreezes UFO spawn:
        spawner.GetComponent<EnemySpawnManager>().spawnFreezed = false;
    }
}
