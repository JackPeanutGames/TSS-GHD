using Photon.Deterministic;
using Quantum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeathZoneInformationView : QuantumCallbacks
{
  public GameObject ArrowParen;
  public TextMeshProUGUI ClockText;
  public Image DangerIndicator;
  public Color DangerIndicatorColor;
  public float RunMessageTime = 2;
  public GameObject RunMessage;

  private GameManager _gameManager;

  private void Start()
  {
    QuantumEvent.Subscribe<EventStartZoneShrinking>(this, OnStartZoneShrinking);
    _gameManager = FindObjectOfType<GameManager>();
    RunMessage.SetActive(false);
  }

  private void OnStartZoneShrinking(EventStartZoneShrinking e)
  {
    ChangeEnableRunMessage();
    Invoke("ChangeEnableRunMessage", RunMessageTime);
  }

  private void ChangeEnableRunMessage()
  {
    RunMessage.SetActive(!RunMessage.activeSelf);
  }

  public override void OnUpdateView(QuantumGame game)
  {
    Frame frame = game.Frames.Verified;

    if (_gameManager.LocalView == null || frame.Exists(_gameManager.LocalView.EntityRef) == false)
    {
      return;
    }

    UpdateArrow(frame);
    UpdateClock(frame);
    UpdateDangerIndicator(frame);
  }

  private void UpdateDangerIndicator(Frame frame)
  {
    DeathZone zone = frame.GetSingleton<DeathZone>();
    if(zone.State == DeathZoneState.Waiting)
    {
      DangerIndicator.color = Color.white;
    }
    else
    {
      DangerIndicator.color = DangerIndicatorColor;
    }

  }

  private void UpdateClock(Frame frame)
  {
    DeathZone zone = frame.GetSingleton<DeathZone>();
    int time = zone.ChangeStateDelay.AsInt;
    ClockText.text = time.ToString();
  }

  private void UpdateArrow(Frame frame)
  {
    Transform2D characterTransform = frame.Get<Transform2D>(_gameManager.LocalView.EntityRef);
    DeathZone zone = frame.GetSingleton<DeathZone>();
    ArrowParen.SetActive(zone.IsOnTargetSafeArea(frame, characterTransform.Position) == false);

    FPVector2 direction = zone.TargetCenter - characterTransform.Position;

    FP angle = FPMath.Atan2(direction.Y, direction.X) * FP.Rad2Deg - 90;
    Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle.AsFloat));
    ArrowParen.transform.rotation = targetRotation;
  }

}
