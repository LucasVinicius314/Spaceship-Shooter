using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
  [SerializeField]
  [Range(0f, 5f)]
  float moveSpeed = 1f;

  void Start()
  {

  }

  void FixedUpdate()
  {
    float horizontal = Input.GetAxisRaw("Horizontal");
    float vertical = Input.GetAxisRaw("Vertical");

    transform.Translate(Vector3.ClampMagnitude(new Vector3(horizontal, vertical, 0f), 1f) * moveSpeed * Time.fixedDeltaTime);
  }
}
