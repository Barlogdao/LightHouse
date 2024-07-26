using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private Button _exitButton;
    private void Awake()
    {
        if (Player.Instance != null)
        {
            Destroy(Player.Instance.gameObject);
        }
        if(Game.Instance != null)
        {
            Destroy(Game.Instance.gameObject);
        }
        if(MapProvider.Instance != null)
        {
            Destroy(MapProvider.Instance.gameObject);
        }
    }

    public void GoCredits()
    {
        SceneManager.LoadScene(2);
    }

    public void QuitGAme()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    private IEnumerator Start()
    {
#if UNITY_WEBGL
        _exitButton.gameObject.SetActive(false);
#endif
        yield return new WaitForSeconds(0.3f);

        AudioManager.Instance.
        PlayMusic();
    }
}
