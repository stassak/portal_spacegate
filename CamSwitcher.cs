using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwitcher : MonoBehaviour
{
    [Header("Assign Your View Models")]
    public GameObject cockpitModel;   // inside view (visible from cockpit)
    public GameObject outsideModel;   // external ship model

    [Header("Settings")]
    public KeyCode switchKey = KeyCode.V;

    private bool cockpitView = true;

    void Start()
    {
        SetView(cockpitView);
    }

    void Update()
    {
        if (Input.GetKeyDown(switchKey))
        {
            cockpitView = !cockpitView;
            SetView(cockpitView);
        }
    }

    void SetView(bool cockpit)
    {
        if (cockpitModel != null)
            cockpitModel.SetActive(cockpit);

        if (outsideModel != null)
            outsideModel.SetActive(!cockpit);

       // Debug.Log($" Switched view {(cockpit ? "Cockpit" : "External")}");
    }
}
