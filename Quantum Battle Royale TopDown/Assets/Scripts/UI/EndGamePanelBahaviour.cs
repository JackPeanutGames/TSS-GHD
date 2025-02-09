using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quantum;
using TMPro;

public class EndGamePanelBahaviour : MonoBehaviour
{
  public GameObject Content;
  public TextMeshProUGUI WinnerName;

  public void Start()
  {
    QuantumEvent.Subscribe<EventOnGameFinish>(this, OnGameFinish);
    Content.SetActive(false);
  }

  private void OnGameFinish(EventOnGameFinish e)
  {
    WinnerName.text = "Player " + GetWinnerName(e.Game.Frames.Verified)+ " Won!";
    Content.SetActive(true);
  }

  private string GetWinnerName(Frame f)
  {
    if (f.ComponentCount<PlayerCharacter>() > 1)
    {
      return "None";
    }
    var characters = f.GetComponentIterator<PlayerCharacter>();
    foreach (var character in characters)
    {
      PlayerLink link = f.Get<PlayerLink>(character.Component.Owner);
      RuntimePlayer runtimePlayer = f.GetPlayerData(link.Player);
      return runtimePlayer.PlayerName;
    }
    return "None";
  }
}
