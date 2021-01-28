using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
  [SerializeField]
  bool targetLead = true;
  [SerializeField]
  [Range(1f, 10f)]
  float rotateSpeed = 5f;
  [SerializeField]
  [Range(1f, 100f)]
  float viewRange = 30f;
  [SerializeField]
  [Range(1f, 100f)]
  float targetRange = 10f;
  [SerializeField]
  [Range(60f, 900f)]
  float fireRate = 300f;
  [SerializeField]
  [Range(1f, 40f)]
  float projectileSpeed = 1f;
  [SerializeField]
  [Range(0f, 2f)]
  float inaccuracy = 0f;
  [SerializeField]
  GameObject projectileObject;

  GameObject target;
  GameObject head;
  Vector3 lastTargetPos = Vector3.zero;
  Vector3 newDirection = Vector3.zero;
  bool isShooting = false;
  float leadIndex = 1f;

  void Start()
  {
    SetTarget();
    SetHead();

    StartCoroutine(Shoot());
  }

  void FixedUpdate()
  {
    SetTarget();

    if (target != null)
    {
      Calculate();
    }
    else
    {
      isShooting = false;
    }
  }

  void SetTarget()
  {
    target = null;
    float closest = viewRange;
    GameObject[] targets = GameObject.FindGameObjectsWithTag("target");
    foreach (GameObject targ in targets)
    {
      float targDistance = (targ.transform.position - transform.position).magnitude;
      if (targDistance < closest)
      {
        target = targ;
        closest = targDistance;
      }
    }
  }

  void SetHead()
  {
    head = transform.Find("Head").gameObject;
  }

  void Calculate()
  {
    float singleStep = rotateSpeed * Time.deltaTime;
    Vector3 targetDirection = target.transform.position - transform.position;

    #region Lead
    Vector3 targetVelocity = (target.transform.position - lastTargetPos) * (1 / Time.fixedDeltaTime);
    float a = Mathf.Pow(targetVelocity.x, 2) + Mathf.Pow(targetVelocity.y, 2) - Mathf.Pow(projectileSpeed, 2);
    float b = 2 * (targetVelocity.x * targetDirection.x + targetVelocity.y * targetDirection.y);
    float c = Mathf.Pow(targetDirection.x, 2) + Mathf.Pow(targetDirection.y, 2);

    float disc = Mathf.Pow(b, 2) - 4 * a * c;

    float t1 = (-b + Mathf.Sqrt(disc)) / (2 * a);
    float t2 = (-b - Mathf.Sqrt(disc)) / (2 * a);

    float t = 0f;
    if (t1 <= 0f) t = t2;
    if (t2 <= 0f) t = t1;
    if (t == 0f)
    {
      t = t1 < t2 ? t1 : t2;
    }
    #endregion

    Vector3 targetDirectionBiased = new Vector3(t * targetVelocity.x + targetDirection.x, t * targetVelocity.y + targetDirection.y, 0f);

    newDirection = Vector3.RotateTowards(transform.forward, targetLead ? targetDirectionBiased : targetDirection, singleStep, 0.0f);
    newDirection.z = 0f;

    float targetAngle = targetLead
    ? Mathf.Atan2(targetDirectionBiased.y, targetDirectionBiased.x)
    : Mathf.Atan2(targetDirection.y, targetDirection.x);
    float newAngle = Mathf.Atan2(newDirection.y, newDirection.x);
    float difference = newAngle - targetAngle;

    float targDistance = (target.transform.position - transform.position).magnitude;
    if (Mathf.Abs(difference) < singleStep && targDistance < targetRange)
    {
      isShooting = true;
    }
    else
    {
      isShooting = false;
    }

    Debug.DrawRay(transform.position, targetDirection, Color.blue);
    Debug.DrawRay(transform.position, newDirection, Color.red);
    Debug.DrawRay(transform.position, targetDirectionBiased, Color.green);

    transform.LookAt(transform.position + newDirection);

    lastTargetPos = target.transform.position;

    #region Old
    // leadIndex = (target.transform.position - transform.position).magnitude;

    // float singleStep = rotateSpeed * Time.deltaTime;

    // Vector3 lead = (target.transform.position - lastTargetPos) * (1 / Time.fixedDeltaTime);

    // Vector3 targetDirection = target.transform.position + lead - transform.position;
    // Vector3 targetDirectionBiased = target.transform.position + (lead * leadIndex) - transform.position;

    // newDirection = Vector3.RotateTowards(transform.forward, biased ? targetDirection : targetDirectionBiased, singleStep, 0.0f);
    // newDirection.z = 0f;

    // float targetAngle = Mathf.Atan2(biased ? targetDirectionBiased.y : targetDirection.y, biased ? targetDirectionBiased.x : targetDirection.x);
    // float newAngle = Mathf.Atan2(newDirection.y, newDirection.x);
    // float difference = newAngle - targetAngle;

    // if (true || Mathf.Abs(difference) < singleStep)
    // {
    //   isShooting = true;
    // }
    // else
    // {
    //   isShooting = false;
    // }

    // Debug.DrawRay(transform.position, targetDirection, Color.blue);
    // Debug.DrawRay(transform.position, newDirection, Color.red);
    // Debug.DrawRay(transform.position, targetDirectionBiased, Color.green);

    // transform.LookAt(transform.position + newDirection);

    // lastTargetPos = target.transform.position;
    #endregion
  }

  IEnumerator Shoot()
  {
    while (true)
    {
      if (isShooting)
      {
        GameObject projectile = GameObject.Instantiate(projectileObject, head.transform.position, head.transform.rotation);
        Vector2 inaccuracyVector = new Vector2(projectile.transform.forward.x * Random.Range(-inaccuracy, inaccuracy), 0f);
        projectile.GetComponent<Rigidbody2D>().AddForce(Vector2.ClampMagnitude(new Vector2(newDirection.x, newDirection.y) + inaccuracyVector, 1f) * 50f * projectileSpeed);
      }

      yield return new WaitForSeconds(1f / (fireRate / 60f));
    }
  }
}
