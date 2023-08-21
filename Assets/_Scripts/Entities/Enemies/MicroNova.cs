public class MicroNova : Meteor
{
    private void Awake()
    {
        SetData();
    }

    protected override void DisableDangerNotifications() { }
    protected override void UpdateHealthBar() { }
}
