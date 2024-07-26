using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpgradeWindow : MonoBehaviour
{
    [SerializeField] private UpgradeSO _upgradeData;

    [SerializeField] private Image _upgradeImage;
    [SerializeField] private TMPro.TextMeshProUGUI _detailAmountText;
    [SerializeField] private Image _detailImage;
    [SerializeField] private Button _button;
    [SerializeField] private Color _selectedFrameColor;
    [SerializeField] private Color _lockedUpgradeIconColor;
    [SerializeField] private Image _frame;

    public static event Action<string> UpgradeHighlited;
    public static event Action UpgradeDehighlited;
    public static event Action<UpgradeSO> UpgradeSelected;

    private void Awake()
    {
        UpgradeSelected += OnUpgradeSelected;
        _button.onClick.AddListener(() => {
            UpgradeSelected?.Invoke(_upgradeData);
            ShowDescription();
        });
    }
    
    

    private void OnUpgradeSelected(UpgradeSO upgrade)
    {
        _frame.color = _upgradeData == upgrade ? _selectedFrameColor : Color.white;
    }

    public void ShowDescription()
    {
        UpgradeHighlited?.Invoke($"{_upgradeData.Name}\n\n{_upgradeData.Description}");
    }
    public void HideDescriptiom()
    {
        UpgradeDehighlited?.Invoke();
    }

    private void OnEnable()
    {
        _detailAmountText.text = _upgradeData.DetailCost.ToString();
        _detailImage.sprite = Game.Instance.DetailImage.Image;
        _upgradeImage.sprite = _upgradeData.Sprite;
        _frame.color = Color.white;
        RefreshWindow();
        HideDescriptiom();
    }

    public void RefreshWindow()
    {
        //_button.interactable = !Player.Instance.UpgradesUnlocked.Contains(_upgradeData.Type) && Player.Instance.DeatailsAmount >= _upgradeData.DetailCost && Player.Instance.UpgradesUnlocked.Contains(_upgradeData.RequiredUpgrade);
        _button.image.color = IsUpgradeAvaliable() ? Color.white : _lockedUpgradeIconColor;
        
        if (Player.Instance.UpgradesUnlocked.Contains(_upgradeData.Type))
        {
            _detailImage.gameObject.SetActive(false);
            _detailAmountText.gameObject.SetActive(false);
        }

    }

    private bool IsUpgradeAvaliable()
    {
        return !Player.Instance.UpgradesUnlocked.Contains(_upgradeData.Type) && Player.Instance.DeatailsAmount >= _upgradeData.DetailCost && Player.Instance.UpgradesUnlocked.Contains(_upgradeData.RequiredUpgrade);
    }


    private void OnDestroy()
    {
        UpgradeSelected -= OnUpgradeSelected;
    }

}
