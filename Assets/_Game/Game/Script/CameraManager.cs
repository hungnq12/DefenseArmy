using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;
    private Vector3 _lastMousePosition;
    private bool _isDragging;
    private bool _isGameStarted;

    public void StartLevel()
    {
        _isGameStarted = true;
    }

    void Update()
    {
        if (_isGameStarted)
        {
            if (Utils.IsNotPointerOverUIObject() && Input.GetMouseButtonDown(0))
            {
                _isDragging = true;
                _lastMousePosition = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                _isDragging = false;
            }

            if (_isDragging)
            {
                Vector3 deltaMouse = Input.mousePosition - _lastMousePosition;
                _lastMousePosition = Input.mousePosition;

                float rotateX = deltaMouse.x * rotationSpeed * Time.deltaTime;

                transform.RotateAround(transform.position, Vector3.up, rotateX);
            }
        }
    }
}
