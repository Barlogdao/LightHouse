using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRay : MonoBehaviour
{
    [SerializeField] private float _speed;

    private void Start()
    {
        Destroy(gameObject, 3f);

    }

    private void Update()
    {
        transform.position += transform.up * Time.deltaTime * _speed;
    }
}
