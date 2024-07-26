using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    private void Update()
    {
        //transform.Translate(GetMousePos() /** Time.deltaTime * _speed*/);
        transform.position += (GetMousePos() - transform.position).normalized * Time.deltaTime * _speed ;
    }

    public static Vector3 GetMousePos()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }
}
