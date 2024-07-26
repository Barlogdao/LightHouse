using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UiCanvas : MonoBehaviour
{
    [SerializeField] private HealthBar _keeperHealthBar;

    [SerializeField] private Button _nextStageButton, _upgradeButton;
    [SerializeField] RectTransform _timerbar;
    [SerializeField] private Image _timer;
    [SerializeField] private TMPro.TextMeshProUGUI _dayCounter;
    private CanvasGroup _timerBarCG;

    [SerializeField] private TMPro.TextMeshProUGUI _energyScoreText;
    [SerializeField] private TMPro.TextMeshProUGUI _detailsScoreText;
    [SerializeField] private TMPro.TextMeshProUGUI _dayText;
    [SerializeField] private Image _energyImage, _detailsImage;
    [SerializeField] private Image _weaponImage;
    [SerializeField] private Image _nextWeapon, _previousWeapon;
    [SerializeField] EndGAmeWindow _endGameWindow;


    [Header("ToopTip")]
    [SerializeField] private RectTransform _tooltipPanel;
    [SerializeField] private TMPro.TextMeshProUGUI _infoText;
    [SerializeField] private Image _tooltipImage;

    [TextArea][SerializeField] private string _firstMessage;


    [SerializeField] private RectTransform _pauseMenu;
    public HealthBar KeeperHealthBar => _keeperHealthBar;

    private void Awake()
    {
        _endGameWindow.gameObject.SetActive(false);
    }

    public void ShowToolTip(string info, Sprite image = null)
    {
        _tooltipPanel.gameObject.SetActive(true);
        _infoText.text = info;
        _tooltipImage.gameObject.SetActive(image != null);
        if (image != null)
        {
            _tooltipImage.sprite = image;
        }
        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Init()
    {
        _energyImage.sprite = Game.Instance.EnergyImage.Image;
        _detailsImage.sprite = Game.Instance.DetailImage.Image;
        _timerBarCG = _timerbar.GetComponent<CanvasGroup>();
        _nextStageButton.onClick.AddListener(() => Game.Instance.NextStage());
        _timerbar.gameObject.SetActive(false);
        _nextStageButton.gameObject.SetActive(false);
        Game.Instance.StateChanged += OnStageChanged;
        Player.Instance.EnergyAmountChanged += OnEnergyAmountChanged;
        Player.Instance.DetailsAmountChanged += OnDetailsAmountChanged;
        Player.Instance.WeaponChanged += OnWeaponChanged;
        _detailsScoreText.text = "0";
        _energyScoreText.text = "0";
        SetDayCounter();
        _dayText.gameObject.SetActive(false);
        _tooltipPanel.gameObject.SetActive(false);
        EventBus.ShowTooltip += ShowToolTip;
        _pauseMenu.gameObject.SetActive(false);
        _keeperHealthBar.gameObject.SetActive(false);
        _nextWeapon.gameObject.SetActive(false);
        _previousWeapon.gameObject.SetActive(false);
        _weaponImage.gameObject.SetActive(false);

    }

    private void OnWeaponChanged(WeaponBase weapon)
    {
        _weaponImage.sprite = weapon.Image;
        if(Player.Instance.WeaponList.Count > 1) 
        {
            _nextWeapon.sprite = Player.Instance.NextWeapon.Image;
            _previousWeapon.sprite = Player.Instance.PreviousWeapon.Image;
        }
    }

    public void MultipleWeaponsHandler()
    {
        _nextWeapon.gameObject.SetActive(true);
        _previousWeapon.gameObject.SetActive(true);
    }

    private void OnDetailsAmountChanged(int amount)
    {
        _detailsScoreText.text = amount.ToString();
    }

    private void OnEnergyAmountChanged(int amount)
    {
        _energyScoreText.text = $"{amount} / {Game.Instance.RequiredEnergyScore}";
        _energyScoreText.transform.DOScale(1.2f, 0.2f).OnComplete(() => { _energyScoreText.transform.DOScale(1f, 0.2f); }).SetUpdate(true);
    }

    private void OnStageChanged(GameState state)
    {
        _timerbar.gameObject.SetActive(false);
        _nextStageButton.gameObject.SetActive(false);
        _upgradeButton.gameObject.SetActive(false);

        switch (state)
        {
            case GameState.StartingGame:
                break;
            case GameState.Day:
                SetDayCounter();
                DaySetter();
                break;
            case GameState.Night:
                if(Game.Instance.CurrentDay == 1)
                {
                   _weaponImage.gameObject.SetActive(true);
                }
                SetDayCounter();
                _timerbar.gameObject.SetActive(true);
                TimerBarVisual();
                StartCoroutine(PlayNightTimer());
                break;
            case GameState.EndGame:
                _dayCounter.gameObject.SetActive(false);
                _detailsImage.gameObject.SetActive(false);
                _detailsScoreText.gameObject.SetActive(false);
                StartCoroutine(EndRoutine());
                break;
        }
    }

    private void DaySetter()
    {
        _dayText.gameObject.SetActive(true);
        _dayText.text = $"Day {Game.Instance.CurrentDay}";
        _dayText.color = new Color(_dayText.color.r, _dayText.color.g, _dayText.color.b, 0f);
        _dayText.DOFade(1f, 2f).OnComplete(() => _dayText.DOFade(0f, 1f).OnComplete(() =>
        {
            _upgradeButton.gameObject.SetActive(Game.Instance.CurrentDay != 1);
            _nextStageButton.gameObject.SetActive(true);
            _dayText.gameObject.SetActive(false);
            if (Game.Instance.CurrentDay == 1)
            {
                SetFirstMessage();
            }
        }));
    }

    private void SetFirstMessage()
    {
        ShowToolTip(_firstMessage);
    }

    private void OnEndGame()
    {
        _endGameWindow.gameObject.SetActive(true);
    }

    private IEnumerator EndRoutine()
    {
        int score = Player.Instance.EnergyScore;
        yield return new WaitForSeconds(1.5f);
        while(Player.Instance.EnergyScore > 0)
        {
            Player.Instance.EnergyScore -= 2;
            yield return new WaitForSeconds(0.05f);
        }
        Player.Instance.EnergyScore = 0;
        yield return new WaitForSeconds(1f);
        if (score >= Game.Instance.RequiredEnergyScore)
        {
            Instantiate(Player.Instance.Salut, Player.Instance.ShootPont.position, Quaternion.identity);
            AudioManager.Instance.SoundManager.PlayGoodSalut();

        }
        else
        {
            Instantiate(Player.Instance.BadSalut, Player.Instance.ShootPont.position, Quaternion.identity);
            AudioManager.Instance.SoundManager.PlayBadSalut();
        }
        yield return new WaitForSeconds(4f);
        if (score >= Game.Instance.RequiredEnergyScore) AudioManager.Instance.SoundManager.PlaeBoatSound();
        Player.Instance.EnergyScore = score;
        _energyImage.enabled = false;
        _energyScoreText.enabled = false;
        yield return new WaitForSeconds(2f);
        OnEndGame();
    }

    private void TimerBarVisual()
    {
        _timerBarCG.alpha = 0f;
        _timerBarCG.DOFade(1f, 0.5f);
    }
    IEnumerator PlayNightTimer()
    {
        _timer.fillAmount = 1f;
        float lastTime = Game.Instance.NightTimer;
        yield return new WaitForSeconds(0.5f);
        while (lastTime > 0f)
        {
            lastTime -= 0.5f;
            _timer.fillAmount = lastTime / Game.Instance.NightTimer;
            yield return new WaitForSeconds(0.5f);
        }
        Game.Instance.NextStage();
    }
    private void SetDayCounter()
    {
        _dayCounter.text = $"Day: {Game.Instance.CurrentDay} / {Game.Instance.TotalDays}";
    }
    private void OnDestroy()
    {
        EventBus.ShowTooltip -= ShowToolTip;
        Game.Instance.StateChanged -= OnStageChanged;
        Player.Instance.EnergyAmountChanged -= OnEnergyAmountChanged;
        Player.Instance.DetailsAmountChanged -= OnDetailsAmountChanged;
        Player.Instance.WeaponChanged -= OnWeaponChanged;

    }


    public void Pause()
    {
        if (!_pauseMenu.gameObject.activeInHierarchy)
        {
            _pauseMenu.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
        else if (_pauseMenu.gameObject.activeInHierarchy)
        {
            _pauseMenu.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }
    public void TimeScaleBack()
    {
        Time.timeScale = 1f;
    }
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        Game.Instance.SetNormalCursor();
        SceneManager.LoadScene(0);
    }
}