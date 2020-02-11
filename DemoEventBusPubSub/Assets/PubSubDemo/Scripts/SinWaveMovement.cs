using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinWaveMovement : MonoBehaviour
{
    void Update()
    {
        transform.position = new Vector3(Mathf.Sin(Time.time) * 10, 0, 0);
    }
}
