using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthFiller;

    public void SetHealth(int value, int maxHealth)
    {
        _healthFiller.fillAmount = (float)value/maxHealth;
    }

}
