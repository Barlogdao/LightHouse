using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSystem : MonoBehaviour
{
    UpgradeWindow[] _upgrades;
    [SerializeField] Image _detailImage;
    [SerializeField] TMPro.TextMeshProUGUI _detailAmount;
    [SerializeField] TMPro.TextMeshProUGUI _upgradeInfo;
    private void Awake()
    {
        _upgrades = GetComponentsInChildren<UpgradeWindow>();
    }
    private void Start()
    {
        _detailImage.sprite = Game.Instance.DetailImage.Image;
        UpgradeWindow.UpgradeHighlited += OnUpgradeHighlited;
        UpgradeWindow.UpgradeDehighlited += OnUpgradeDehighlited;
        _upgradeInfo.text = "";
    }

    private void OnUpgradeDehighlited()
    {
        _upgradeInfo.text = "";
    }

    private void OnUpgradeHighlited(string text)
    {
        _upgradeInfo.text = text;
    }

    private void OnEnable()
    {
        SetDetailsAmout();
    }

    public void RefreshUpgradesStatus()
    {
        foreach (var upgrade in _upgrades)
        {
            upgrade.RefreshWindow();
        }
        SetDetailsAmout();
    }

    private void SetDetailsAmout()
    {
        _detailAmount.text = Player.Instance.DeatailsAmount.ToString();
    }

    private void OnDestroy()
    {
        UpgradeWindow.UpgradeHighlited -= OnUpgradeHighlited;
        UpgradeWindow.UpgradeDehighlited -= OnUpgradeDehighlited;
    }
}
