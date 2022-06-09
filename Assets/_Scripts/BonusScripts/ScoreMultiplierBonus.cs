using UnityEngine;

public class ScoreMultiplierBonus : BonusManager
{
    public ScoreCounter scoreCounter;
    public int bonusDuration;
    public int bonusScoreMultiplier;
    public int instantiateDelay;

    [SerializeField]
    private int _bonusScoreLimit = 5000;
    private readonly int _defaultMultiplier = 1;

    private void Start()
    {
        // "1" because method call is need literally right after the game starts:
        InvokeRepeating("TryToInstantiateScoreMultiplier", 1, instantiateDelay);
    }
    public override void ActivateBonus()
    {
        if (bonusCounter == 0 || PauseMenu.isGamePaused) return;

        scoreCounter.scoreMultiplier *= bonusScoreMultiplier;
        Invoke("ResetMultiplier", bonusDuration);
        bonusCounter--;
    }
    private void ResetMultiplier()
    {
        scoreCounter.scoreMultiplier = _defaultMultiplier;
    }
    private void TryToInstantiateScoreMultiplier()
    {
        if (scoreCounter.currentScore >= _bonusScoreLimit)
        {
            InstantiateBonus();
            // Equal to multiple in 2 times:
            _bonusScoreLimit += _bonusScoreLimit;
        }
    }
}
