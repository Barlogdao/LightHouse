using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Game : SingletonPersistent<Game>
{

    [SerializeField] private Light2D _globalLight;
    [SerializeField] private Texture2D _normalCursor, _battleCursor;
    private Vector2 _normalHotspot, _battleHotSpot;
   
    
    [SerializeField] private int _dayAmount;
    public int TotalDays => _dayAmount;
    private int _currentday = 1;
    public int CurrentDay => _currentday;
    [SerializeField] private float _nightTimer;
    public float NightTimer => _nightTimer;

    [field:SerializeField] public int RequiredEnergyScore { get; private set; }

    private GameState _state;
    public GameState State => _state;
    public event Action <GameState> StateChanged;

    [field: SerializeField] public ResSO EnergyImage, DetailImage;



    private void Start()
    {
        _normalHotspot = Vector2.zero;
        _battleHotSpot = new Vector2(_battleCursor.width/2, _battleCursor.height/2);

        FindObjectOfType<UiCanvas>().Init();
        
        ChangeState(GameState.Day);
        
    }

    public void ChangeState(GameState state)
    {
        _state = state;
        OnStateChanged(_state);
        StateChanged?.Invoke(_state);
    }
    public void NextStage()
    {
        if (_state == GameState.Night && _currentday == _dayAmount)
        {
            ChangeState(GameState.EndGame);
        }
        else if (_state == GameState.Night)
        {
            _currentday++;
            ChangeState(GameState.Day);
        }
        else if (_state == GameState.Day)
        {
            ChangeState(GameState.Night);
        }
    }

    private void OnStateChanged(GameState state)
    {
        Cursor.SetCursor(_normalCursor, _normalHotspot, CursorMode.Auto);
        StopAllCoroutines();
        switch (state)
        {
            case GameState.StartingGame:
                break;
            case GameState.Day:
                StartCoroutine(GlobalLightOn());
                AudioManager.Instance.PlayOcean();
                AudioManager.Instance.SoundManager.PlayNewDay();
                break;
            case GameState.Night:
                Cursor.SetCursor(_battleCursor, _battleHotSpot, CursorMode.Auto);
                StartCoroutine(GlobalLightOFF());
                AudioManager.Instance.PlayBattleMusic();
                break;
            case GameState.EndGame:
                StartCoroutine(GlobalLightOn());
                AudioManager.Instance.PlayOcean();
                AudioManager.Instance.SoundManager.PlayNewDay();
                break;
        }
    }

    IEnumerator GlobalLightOn()
    {
        _globalLight.intensity = 0;
        float endIntensity = 1f;
        float incrementIntensity = 0.05f;
        if (_state == GameState.Day && _currentday == 1)
        {
            _globalLight.intensity = 0.8f;
        }
        while (_globalLight.intensity < endIntensity)
        {
            _globalLight.intensity += incrementIntensity;

            yield return new WaitForSeconds(incrementIntensity);
        }
        _globalLight.intensity = endIntensity;
    }

    IEnumerator GlobalLightOFF()
    {
        _globalLight.intensity = 1;
        float endIntensity = 0f;
        float incrementIntensity = 0.05f;
        if (_state == GameState.Night && _currentday == 1)
        {
            endIntensity = 0.1f;
        }
        while (_globalLight.intensity > endIntensity)
        {
            _globalLight.intensity -= incrementIntensity;

            yield return new WaitForSeconds(incrementIntensity);
        }
        _globalLight.intensity = endIntensity;
    }

    public void SetNormalCursor()
    {
        Cursor.SetCursor(_normalCursor, _normalHotspot, CursorMode.Auto);
    }

}



public enum GameState
{
    StartingGame,
    Day,
    Night,
    EndGame
}
