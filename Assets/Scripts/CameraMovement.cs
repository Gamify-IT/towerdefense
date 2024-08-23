using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Handles camera movement and zooming within the bounds of a tilemap.
/// </summary>
public class CameraMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera cam;
    [SerializeField] private Tilemap tilemap;

    [Header("Attributes")]
    [SerializeField] private float zoomStep = 0.5f;
    [SerializeField] private float minCamSize = 2f;
    [SerializeField] private float maxCamSize = 10f;
    [SerializeField] private float speed = 5f;

    private Vector3 minBounds;
    private Vector3 maxBounds;

    private void Start()
    {
        tilemap.CompressBounds();

        SetCameraBounds();
        UpdateCameraExtents();
    }

    private void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    /// <summary>
    /// Sets the camera bounds based on the tilemap bounds.
    /// </summary>
    private void SetCameraBounds()
    {
        Bounds tilemapBounds = tilemap.localBounds;
        minBounds = tilemapBounds.min + (Vector3)tilemap.transform.position;
        maxBounds = tilemapBounds.max + (Vector3)tilemap.transform.position;

        Debug.Log("Min Bounds: " + minBounds);
        Debug.Log("Max Bounds: " + maxBounds);
    }

    /// <summary>
    /// Updates the camera extents based on the current camera size.
    /// </summary>
    private void UpdateCameraExtents()
    {
        float vertExtent = cam.orthographicSize;
        float horzExtent = vertExtent * cam.aspect;

        Debug.Log("Camera vertExtent: " + vertExtent);
        Debug.Log("Camera horzExtent: " + horzExtent);
    }

    /// <summary>
    /// Handles camera movement using the WASD-Keys.
    /// </summary>
    private void HandleMovement()
    {
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
        {
            moveDirection += Vector3.left;
        }

        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += Vector3.up;
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveDirection += Vector3.down;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += Vector3.right;
        }

        if (moveDirection != Vector3.zero)
        {
            moveDirection.Normalize();
            Vector3 newPosition = transform.position + moveDirection * Time.deltaTime * speed;
            transform.position = ClampCamera(newPosition);
        }
    }

    /// <summary>
    /// Handles camera zooming using the mouse scroll.
    /// </summary>
    private void HandleZoom()
    {
        float scrollData = Input.GetAxis("Mouse ScrollWheel");

        if (scrollData != 0.0f)
        {
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - scrollData * zoomStep, minCamSize, maxCamSize);
            UpdateCameraExtents();
            transform.position = ClampCamera(transform.position);
        }
    }

    /// <summary>
    /// Clamps the camera's position to ensure it stays within the tilemap bounds.
    /// </summary>
    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float vertExtent = cam.orthographicSize;
        float horzExtent = cam.orthographicSize * cam.aspect;
        UpdateCameraExtents();

        float minX = minBounds.x + horzExtent;
        float maxX = maxBounds.x - horzExtent;
        float minY = minBounds.y + vertExtent;
        float maxY = maxBounds.y - vertExtent;

        Debug.Log("Clamp Values - minX: " + minX + ", maxX: " + maxX + ", minY: " + minY + ", maxY: " + maxY);

        float clampedX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3(clampedX, clampedY, targetPosition.z);
    }

    /// <summary>
    /// Zooms the camera in, clamping the size to the minimum value.
    /// </summary>
    public void ZoomIn()
    {
        float newSize = cam.orthographicSize - zoomStep;
        cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);
        UpdateCameraExtents();
        transform.position = ClampCamera(transform.position);
    }

    /// <summary>
    /// Zooms the camera out, clamping the size to the maximum value.
    /// </summary>
    public void ZoomOut()
    {
        float newSize = cam.orthographicSize + zoomStep;
        cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);
        UpdateCameraExtents();
        transform.position = ClampCamera(transform.position);
    }
}
