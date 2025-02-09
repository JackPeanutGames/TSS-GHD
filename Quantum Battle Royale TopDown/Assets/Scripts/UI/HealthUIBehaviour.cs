using Quantum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIBehaviour : QuantumCallbacks
{
  [SerializeField]
  private Image _healthUI;
  private GameManager _gameManager;

  private void Start()
  {
    _gameManager = FindObjectOfType<GameManager>();
  }

  public override void OnUpdateView(QuantumGame game)
  {
    if (_gameManager.LocalView == null)
    {
      return;
    }

    Frame frame = game.Frames.Predicted;
    if(frame.Exists(_gameManager.LocalView.EntityRef) == false)
    {
      return;
    }

    Health health = frame.Get<Health>(_gameManager.LocalView.EntityRef);
    _healthUI.fillAmount = (health.CurrentValue / health.MaxValue).AsFloat;
  }
}
