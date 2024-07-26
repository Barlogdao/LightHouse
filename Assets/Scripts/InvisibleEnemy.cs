public class InvisibleEnemy : Damagable
{

    private const int VISIBLE_STATUS = 5;
    private const int INVISIBLE_STATUS = -1;
    public override void GetDamage(int damage)
    {
        ShowDeath();
        Player.Instance.EnergyScore += _energyReward;
        Player.Instance.DeatailsAmount += _detailsReward;
        Destroy(gameObject, 3f);
    }

    protected override void EnterLightRayBehaviour()
    {
        if (Player.Instance.UpgradesUnlocked.Contains(UpgradeType.LightRayInvisible))
        {
            _sr.sortingOrder = VISIBLE_STATUS;
        }
        
    }

    protected override void LeaveLightRayBehaviour()
    {
        _sr.sortingOrder = INVISIBLE_STATUS;
    }
}
