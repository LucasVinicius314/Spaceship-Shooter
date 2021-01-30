using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
  [SerializeField]
  [Range(0f, 10f)]
  float maxRange = 2f;
  [SerializeField]
  [Range(2, 8)]
  int maxZoom = 4;
  [SerializeField]
  [Range(10, 40)]
  int minZoom = 20;

  public Vector3 offset = new Vector3(0f, 0f, -5f);
  float offsetX = 0f;
  float offsetY = 0f;
  float zoom = 0;

  void Start()
  {
    Cursor.lockState = CursorLockMode.Locked;
    zoom = Mathf.Floor(((minZoom - maxZoom) / 2f) + maxZoom);
  }

  void Update()
  {
    #region Zoom
    float input = Input.GetAxisRaw("Mouse ScrollWheel");
    zoom -= input * 4f;
    zoom = Mathf.Clamp(zoom, maxZoom, minZoom);
    Camera.main.orthographicSize = zoom;
    #endregion

    #region Offset        
    offsetX += Input.GetAxisRaw("Mouse X");
    offsetY += Input.GetAxisRaw("Mouse Y");

    offsetX = Mathf.Clamp(offsetX, -maxRange, maxRange);
    offsetY = Mathf.Clamp(offsetY, -maxRange, maxRange);

    offset.x = offsetX;
    offset.y = offsetY;
    #endregion
  }
}
