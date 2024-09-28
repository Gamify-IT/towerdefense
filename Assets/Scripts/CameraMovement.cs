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
    private float verticalExtent;
    private float horizontalExtent;

    private void Start()
    {
        tilemap.CompressBounds();
        SetCameraBounds();
        UpdateCameraExtents();
        maxCamSize = CalculateMaxCameraSize();
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
    }

    /// <summary>
    /// Updates the camera extents based on the current camera size.
    /// </summary>
    private void UpdateCameraExtents()
    {
        verticalExtent = cam.orthographicSize;
        horizontalExtent = verticalExtent * cam.aspect;
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
        if (!UIManager.Instance.IsHoveringUI())
        {
            float scrollData = Input.GetAxis("Mouse ScrollWheel");

            if (scrollData != 0.0f)
            {
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - scrollData * zoomStep, minCamSize, maxCamSize);
                UpdateCameraExtents();
                transform.position = ClampCamera(transform.position);
            }
        }
    }

    /// <summary>
    /// Clamps the camera's position to ensure it stays within the tilemap bounds.
    /// </summary>
    /// <returns>position vector with clamped coordinates</returns>
    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        verticalExtent = cam.orthographicSize;
        horizontalExtent = cam.orthographicSize * cam.aspect;
        UpdateCameraExtents();

        float minX = minBounds.x + horizontalExtent;
        float maxX = maxBounds.x - horizontalExtent;
        float minY = minBounds.y + verticalExtent;
        float maxY = maxBounds.y - verticalExtent;

        float clampedX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3(clampedX, clampedY, targetPosition.z);
    }

    /// <summary>
    /// Calculate maximum vertical and horizontal size that fits within the tilemap bounds.
    /// </summary>
    /// <returns>the smaller of the two float values</returns>
    private float CalculateMaxCameraSize()
    {
        float maxVerticalSize = (maxBounds.y - minBounds.y) / 2f;
        float maxHorizontalSize = (maxBounds.x - minBounds.x) / (2f * cam.aspect);

        return Mathf.Min(maxVerticalSize, maxHorizontalSize);
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
