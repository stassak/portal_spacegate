using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceRotation : MonoBehaviour
{
    [Header("Rotation Axes")]
    public bool rotateX = false;
    public bool rotateY = true;
    public bool rotateZ = false;

    [Header("Rotation Period (seconds per full turn)")]
    public float periodX = 10f;
    public float periodY = 20f;
    public float periodZ = 30f;

    void Update()
    {
        RotateAxis(Vector3.right, rotateX, periodX);
        RotateAxis(Vector3.up, rotateY, periodY);
        RotateAxis(Vector3.forward, rotateZ, periodZ);
    }

    void RotateAxis(Vector3 axis, bool enabled, float period)
    {
        if (!enabled || period <= 0f)
            return;

        float degreesPerSecond = 360f / period;
        transform.Rotate(axis, degreesPerSecond * Time.deltaTime, Space.Self);
    }
}
