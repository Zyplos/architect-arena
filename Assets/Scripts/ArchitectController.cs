// from https://www.youtube.com/watch?v=z7eojB_1wKg

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchitectController : MonoBehaviour
{
    // public Transform target;
    public Vector3 targetPosition;

    public float horizontalMove = 0.5f;
    public float verticalMove = 0.25f;

    public float zoomSpeed = 1000.0f;
    public float minDistance = 40.0f;
    public float maxDistance = 900.0f;

    public float minVerticalAngle = 10.0f;
    public float maxVerticalAngle = 70.0f;

    private float distance = 80.0f;


    public void moveHorizontal(bool left)
    {
        float direction = left ? -1 : 1;
        transform.RotateAround(targetPosition, Vector3.up, direction * horizontalMove);
    }

    public void moveVertical(bool up)
    {
        float direction = up ? -1 : 1;
        transform.RotateAround(targetPosition, transform.TransformDirection(Vector3.right), direction * verticalMove);
    }

    public void ManualUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            moveHorizontal(true);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveHorizontal(false);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            moveVertical(true);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveVertical(false);
        }

        // Scroll wheel input
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.0f)
        {
            distance -= scroll * zoomSpeed;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
        }

        // Update camera position
        Vector3 dirToTarget = transform.position - targetPosition;
        Vector3 newPos = targetPosition + dirToTarget.normalized * distance;
        transform.position = newPos;

        // Make the camera always look at the target
        transform.LookAt(targetPosition);
    }
}