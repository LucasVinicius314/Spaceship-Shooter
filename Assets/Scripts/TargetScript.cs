using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
  void Start()
  {
    GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f)) * 50f);
    StartCoroutine(Despawn());
  }

  void OnCollisionEnter2D(Collision2D collision)
  {
    Destroy(gameObject);
  }

  IEnumerator Despawn()
  {
    yield return new WaitForSeconds(100f);
    Destroy(gameObject);
  }
}
