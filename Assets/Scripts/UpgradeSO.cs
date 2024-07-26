using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Upgrade", menuName = "FolderName/Upgrade", order = 1)]
public class UpgradeSO : ScriptableObject
{
    public Sprite Sprite;
    public string Name;
    public string Description;
    public int DetailCost;
    public UpgradeType Type;
    public UpgradeType RequiredUpgrade;

}

public enum UpgradeType
{
    None,
    Rifle,
    Laser,
    Babaha,
    SignalRifle,
    LaserCD,
    BabahaAvoid,
    LightRaySpeed,
    LightRayInvisible,
    InnerLightRange,
    Radar,
    RadarDuration,
    RadarCD,
}
