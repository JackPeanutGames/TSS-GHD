using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Quantum;

public class MatchStatsBehaviour : QuantumCallbacks
{
  public TextMeshProUGUI PlayersCount;
  public TextMeshProUGUI LocalPlayerKillsCount;

  private GameManager _gameManager;

  private void Start()
  {
    _gameManager = FindObjectOfType<GameManager>();
  }

  public override void OnUpdateView(QuantumGame game)
  {
    if (QuantumRunner.Default == null || _gameManager == null)
    {
      return;
    }

    if (_gameManager.LocalView == null)
    {
      return;
    }

    Frame frame = game.Frames.Verified;

    PlayersCount.text = frame.ComponentCount<PlayerCharacter>().ToString();

    if (frame.Exists(_gameManager.LocalView.EntityRef))
    {
      PlayerScore score = frame.Get<PlayerScore>(_gameManager.LocalView.EntityRef);
      LocalPlayerKillsCount.text = score.Kills.ToString();
    }
  }
}
