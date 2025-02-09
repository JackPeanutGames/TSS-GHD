using Quantum;
using UnityEngine;
using Quantum.Demo;

public class CustomCallbacks : QuantumCallbacks
{
  public string PlayerName = "testName";
  public AssetRefEntityPrototype CharacterPrototype;

  public override void OnGameStart(Quantum.QuantumGame game)
  {
    // paused on Start means waiting for Snapshot
    if (game.Session.IsPaused) return;

    if(UIMain.Client != null)
    {
      PlayerName = UIMain.Client.NickName;
    }

    foreach (var lp in game.GetLocalPlayers())
    {
      Debug.Log("CustomCallbacks - sending player: " + lp);
      game.SendPlayerData(lp, new Quantum.RuntimePlayer { PlayerName = PlayerName, SelectedCharacter = CharacterPrototype});
    }
  }

  public override void OnGameResync(Quantum.QuantumGame game)
  {
    Debug.Log("Detected Resync. Verified tick: " + game.Frames.Verified.Number);
  }
}

