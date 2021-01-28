using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
  void Start()
  {
    StartCoroutine(Despawn());
  }

  IEnumerator Despawn()
  {
    yield return new WaitForSeconds(10f);
    Destroy(gameObject);
  }

  void OnCollisionEnter2D(Collision2D collision)
  {
    Destroy(gameObject);
  }
}
