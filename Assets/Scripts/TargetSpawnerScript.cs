using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawnerScript : MonoBehaviour
{
  [SerializeField]
  [Range(10f, 100f)]
  float range = 20f;
  [SerializeField]
  [Range(0.5f, 10f)]
  float frequency = 1f;
  [SerializeField]
  GameObject target;

  CameraScript cameraScript;

  void Start()
  {
    StartCoroutine(SpawnTarget());
    cameraScript = GetComponent<CameraScript>();
  }

  IEnumerator SpawnTarget()
  {
    while (true)
    {
      yield return new WaitForSeconds(1f / frequency);
      float rnd = Random.Range(0f, 360f) * Mathf.Deg2Rad;
      Vector3 position = new Vector3(Mathf.Cos(rnd), Mathf.Sin(rnd), 0f) * range;
      position.x = -2f;
      GameObject.Instantiate(target, transform.position - cameraScript.offset + position, Quaternion.identity);
    }
  }
}
