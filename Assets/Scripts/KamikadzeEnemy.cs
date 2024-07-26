using UnityEngine;

public class KamikadzeEnemy : Damagable
{
    public override void GetDamage(int damage)
    {
        ShowDeath();
        

        Player.Instance.EnergyScore += _energyReward;
        Player.Instance.DeatailsAmount += _detailsReward;
        Player.Instance.ShowEnargyDrain(transform.position);
        Destroy(gameObject, 3f);
    }

    protected override void EnterLightRayBehaviour()
    {
        
    }

    protected override void LeaveLightRayBehaviour()
    {
       
    }
}
