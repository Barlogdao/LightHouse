using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGAmeWindow : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI _messageText;
    [SerializeField] Button _button;

    private void OnEnable()
    {
        if (Player.Instance.EnergyScore >= Game.Instance.RequiredEnergyScore)
        {

            _messageText.text = "Help is coming!";

        }
        else
        {
            _messageText.text = "So sad...";
        }
    }
    private void Start()
    {
        _button.onClick.AddListener(() => SceneManager.LoadScene(GetScene()));
    }

    private int GetScene()
    {
        return Player.Instance.EnergyScore >= Game.Instance.RequiredEnergyScore ? 2:0 ;
    }
}
