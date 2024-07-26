using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : WeaponBase
{
    public bool IsUpgraded = false;
    [SerializeField] private ParticleSystem _particles;
    [SerializeField] private GameObject _signalPref;
    public override void Shoot(Vector2 placePoint)
    {
        AudioManager.Instance.SoundManager.PlayRifle();
        var target = Physics2D.OverlapPoint(placePoint, _layerMask);
        Instantiate(_particles, placePoint, Quaternion.identity);
        if (IsUpgraded)
        {
            var signal = Instantiate(_signalPref,placePoint, Quaternion.identity);
            Destroy(signal, 4.5f);
        }
        //shoot
        if (target != null && target.TryGetComponent<Damagable>(out var damagable))
        {
            damagable.GetDamage(_damage);
        }
    }
}
