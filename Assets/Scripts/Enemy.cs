//using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Damagable
{

    
    [SerializeField] private LayerMask _lightRayLayer;

    public override void GetDamage(int damage)
    {
        ShowDeath();

        Player.Instance.EnergyScore += _energyReward;
        Player.Instance.DeatailsAmount += _detailsReward;
        Destroy(gameObject,3f);
    }

    protected override void EnterLightRayBehaviour()
    {
       
    }

    protected override void LeaveLightRayBehaviour()
    {
        
    }
}
