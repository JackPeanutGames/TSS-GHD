using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObjectAnimation : MonoBehaviour
{
  public AnimationCurve HeightCurve;
  public float TimeScale = 1;
  public Vector3 Offset;
  public float RotateSpeed = 100f;

  private float _time;

  void Update()
  {
    UpdateFloatingPosition();
    UpdateRotation();
  }

  private void UpdateFloatingPosition()
  {
    _time += (Time.deltaTime * TimeScale);
    float height = HeightCurve.Evaluate(_time % HeightCurve.keys[HeightCurve.length - 1].time);
    transform.position = new Vector3(transform.position.x, height, transform.position.z);
    transform.position += Offset;
  }

  private void UpdateRotation()
  {
    transform.Rotate(Vector3.up * Time.deltaTime * RotateSpeed);
  }
}
