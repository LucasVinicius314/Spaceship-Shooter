using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipScript : MonoBehaviour
{
  Rigidbody2D rb;
  CameraScript cameraScript;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    cameraScript = Camera.main.GetComponent<CameraScript>();
  }

  void Update()
  {
    Vector3 position = transform.position;
    Camera.main.transform.position = position + cameraScript.offset;

    if (Input.GetKeyDown(KeyCode.Space))
    {
      rb.drag = 1f;
      rb.angularDrag = 1f;
    }
    if (Input.GetKeyUp(KeyCode.Space))
    {
      rb.drag = 0f;
      rb.angularDrag = 0.5f;
    }
  }

  void FixedUpdate()
  {
    float horizontal = -Input.GetAxisRaw("Horizontal");
    float vertical = Input.GetAxisRaw("Vertical");

    rb.AddForce(transform.up * vertical);
    rb.AddTorque(horizontal);

    rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -30f, 30f);
  }
}
