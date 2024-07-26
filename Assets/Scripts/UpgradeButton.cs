using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    private UpgradeSO _selectedUpgrade;
    private  Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        UpgradeWindow.UpgradeSelected += OnUpgradeSelected;
        _button.onClick.AddListener(() => {
            Player.Instance.SetUpgrade(_selectedUpgrade.Type, _selectedUpgrade.DetailCost, _selectedUpgrade.Sprite);
            GetComponentInParent<UpgradeSystem>().RefreshUpgradesStatus();
            CheckUpgrade();
        });
    }

    private void OnEnable()
    {
        _selectedUpgrade = null;
        CheckUpgrade();
    }

    private void OnUpgradeSelected(UpgradeSO upgrade)
    {
        _selectedUpgrade = upgrade;
        CheckUpgrade();
    }


    private void CheckUpgrade()
    {
        _button.interactable = _selectedUpgrade != null && _selectedUpgrade
            && !Player.Instance.UpgradesUnlocked.Contains(_selectedUpgrade.Type)
            && Player.Instance.DeatailsAmount >= _selectedUpgrade.DetailCost 
            && Player.Instance.UpgradesUnlocked.Contains(_selectedUpgrade.RequiredUpgrade);
    }
    private void OnDestroy()
    {
        UpgradeWindow.UpgradeSelected -= OnUpgradeSelected;
    }
}
