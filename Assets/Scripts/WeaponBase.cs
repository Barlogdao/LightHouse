using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] protected LayerMask _layerMask;
    [SerializeField] protected int _damage;
    public Sprite Image;
    public string Name;
    public string Description;
    public float LastFireTime = 0;

    [field: SerializeField] public float ReloadCD { get; set; }
    public abstract void Shoot(Vector2 placePoint);



    public void UpgradeLayerMask(LayerMask layerMask)
    {
        _layerMask = layerMask;
    }
}
