using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{

    [SerializeField] private TMPro.TextMeshProUGUI _bandName, _authorName;

    private void Awake()
    {
        if (Player.Instance != null)
        {
            Destroy(Player.Instance.gameObject);
        }
        if (Game.Instance != null)
        {
            Destroy(Game.Instance.gameObject);
        }
        if (MapProvider.Instance != null)
        {
            Destroy(MapProvider.Instance.gameObject);
        }
    }

    IEnumerator Start()
    {
        _bandName.alpha = 0f;
        _authorName.alpha = 0f;
        _bandName.DOFade(1f, 1f).OnComplete(() => _authorName.DOFade(1f,0.5f));
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(0);
    }
}
