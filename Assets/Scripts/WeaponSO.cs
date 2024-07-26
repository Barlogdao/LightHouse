using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "weapon", menuName = "FolderName/WeaponsO", order = 1)]
public class WeaponSO : ScriptableObject
{
    public Sprite Image;
    public string Name;
    public string Description;
    public WeaponBase Weapon;
}
