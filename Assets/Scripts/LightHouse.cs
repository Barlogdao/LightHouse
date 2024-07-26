using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightHouse : MonoBehaviour
{
    [SerializeField] private Transform _lightRay;
    [SerializeField] private int _lightRaySpeed;
    [SerializeField] private Light2D _smallLight;
    private float _smallLightStep = 0.05f;

    private const float INNER_LIGHT_MIN = 0.2f;
    private const float INNER_LIGHT_MAX = 1.2f;

    private void Start()
    {
        Game.Instance.StateChanged += OnStateChanged;
    }

    private void OnStateChanged(GameState state)
    {
        StopAllCoroutines();
        switch (state)
        {
            case GameState.StartingGame:
                StopPlayLights();
                break;
            case GameState.Day:
                StopPlayLights();
                break;
            case GameState.Night:
                StartPlayLights();
                break;
            case GameState.EndGame:
                StopPlayLights();
                break;
        }
    }

    private void Update()
    {
        if(_lightRay.gameObject.activeInHierarchy && Time.timeScale != 0)
        _lightRay.Rotate(0f, 0f, _lightRaySpeed * Time.deltaTime);
    }

    private void StartPlayLights()
    {
        _lightRay.gameObject.SetActive(true);
        _smallLight.gameObject.SetActive(true);
        StartCoroutine(SmallLight());
    }

    private void StopPlayLights()
    {
        _lightRay.gameObject.SetActive(false);
        _smallLight.gameObject.SetActive(false);
    }

    IEnumerator SmallLight()
    {
        _smallLight.intensity = INNER_LIGHT_MAX;
        while (true)
        {
            while (_smallLight.intensity < INNER_LIGHT_MAX)
            {
                _smallLight.intensity += _smallLightStep;
                yield return new WaitForSeconds(_smallLightStep *2);
            }
            _smallLight.intensity = INNER_LIGHT_MAX;
            yield return new WaitForSeconds(1f);
            while (_smallLight.intensity > INNER_LIGHT_MIN)
            {
                _smallLight.intensity -= _smallLightStep;
                yield return new WaitForSeconds(_smallLightStep *2);

            }
            _smallLight.intensity = INNER_LIGHT_MIN;
            yield return new WaitForSeconds(1f);
        }
    }
    private void OnDestroy()
    {
        Game.Instance.StateChanged-= OnStateChanged;
    }


    public void UpgradeLightRaySpeed()
    {
        _lightRaySpeed += 30;
    }

    public void UpgradeLightRayInvisible()
    {

    }

    public void UpgradeInnerLight()
    {
        _smallLight.pointLightInnerRadius += 2f;
        _smallLight.pointLightOuterRadius += 2f;
    }
}
