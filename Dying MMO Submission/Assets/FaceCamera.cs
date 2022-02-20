using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    Canvas canvas;
    Camera camera;

    private void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
        camera = Camera.main;
        canvas.worldCamera = camera;
    }

    public void Update()
    {
        // rotates canvas to camera, but it doesn't look good
        if (camera == null)
            camera = Camera.main;

        canvas.transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
    }

}
