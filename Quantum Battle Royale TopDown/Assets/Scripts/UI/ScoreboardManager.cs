using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Quantum;

public class ScoreboardManager : QuantumCallbacks
{
  public TextMeshProUGUI Text;

  public override void OnUpdateView(QuantumGame game)
  {
    Text.text = "";
    Frame frame = game.Frames.Verified;
    var filter = frame.Filter<PlayerCharacter, PowerUpInventoty>();
    while (filter.Next(out var entity, out var playerCharacter, out var inv))
    {
      PlayerLink link = frame.Get<PlayerLink>(playerCharacter.Owner);

      var data = frame.GetPlayerData(link.Player);
      Text.text += data.PlayerName + " ";

      int powerUpCount = 0;
      for (int i = 0; i < inv.Slots.Length; i++)
      {
        powerUpCount += inv.Slots[i].Amount;
      }
      Text.text += powerUpCount +"\n";
    }
  }
}
