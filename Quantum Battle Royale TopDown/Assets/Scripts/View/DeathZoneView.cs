using Quantum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZoneView : QuantumCallbacks
{
  [SerializeField] private Transform _targetCircleTransform;

  private Transform _circleTransform;
  private Transform _topTransform;
  private Transform _bottomTransform;
  private Transform _leftTransform;
  private Transform _rightTransform;

  private void Start()
  {
    _circleTransform = transform.Find("DeathZoneShrink");
    _topTransform = transform.Find("TopZone");
    _bottomTransform = transform.Find("BottomZone");
    _leftTransform = transform.Find("LeftZone");
    _rightTransform = transform.Find("RightZone");
  }

  public override void OnUpdateView(QuantumGame game)
  {
    Frame frame = game.Frames.Verified;
    DeathZone zone = frame.GetSingleton<DeathZone>();

    Vector3 position = new Vector3(zone.CurrentCenter.X.AsFloat, 0, zone.CurrentCenter.Y.AsFloat);
    Vector3 size = new Vector3(zone.CurrentRadius.AsFloat, zone.CurrentRadius.AsFloat);
    SetCircleSize(position, size*2);

    position = new Vector3(zone.TargetCenter.X.AsFloat, .1f, zone.TargetCenter.Y.AsFloat);
    size = new Vector3(zone.TargetRadius.AsFloat, zone.TargetRadius.AsFloat);
    SetTargetCircle(position, size*2);
  }



  private void SetCircleSize(Vector3 position, Vector3 size)
  {
    transform.position = position;

    _circleTransform.localScale = size;

    _topTransform.localScale = new Vector3(1000, 1000, 0);
    _topTransform.localPosition = new Vector3(0, _topTransform.localPosition.y, _topTransform.localScale.y * .5f + size.y * .5f);

    _bottomTransform.localScale = new Vector3(1000, 1000);
    _bottomTransform.localPosition = new Vector3(0, _topTransform.localPosition.y, -_topTransform.localScale.y * .5f - size.y * .5f);

    _leftTransform.localScale = new Vector3(1000, size.y);
    _leftTransform.localPosition = new Vector3(-_leftTransform.localScale.x * .5f - size.x * .5f, _topTransform.localPosition.y, 0f);

    _rightTransform.localScale = new Vector3(1000, size.y);
    _rightTransform.localPosition = new Vector3(+_leftTransform.localScale.x * .5f + size.x * .5f, _topTransform.localPosition.y, 0f);
  }

  private void SetTargetCircle(Vector3 position, Vector3 size)
  {
    _targetCircleTransform.position = position;
    _targetCircleTransform.localScale = size;
  }
}
