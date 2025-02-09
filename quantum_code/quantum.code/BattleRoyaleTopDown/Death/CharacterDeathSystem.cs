using Photon.Deterministic;

namespace Quantum
{
  public unsafe class CharacterDeathSystem : SystemSignalsOnly, ISignalOnCharacterDie
  {
    public void OnCharacterDie(Frame frame, EntityRef targetCharacter, EntityRef sourceCharacter)
    {
      PlayerCharacter targetPlayer = frame.Get<PlayerCharacter>(targetCharacter);

      //frame.Add<SpawnCharacterTimer>(targetPlayer.Owner, out var spawnCharacterTimer);
      //spawnCharacterTimer->Timer = 3;

      PlayerLink* targetLink = frame.Unsafe.GetPointer<PlayerLink>(targetPlayer.Owner);
      var targetData = frame.GetPlayerData(targetLink->Player);

      string sourceName = "";
      if (frame.TryGet<PlayerCharacter>(sourceCharacter, out var sourcePlayer))
      {
        PlayerLink* sourceLink = frame.Unsafe.GetPointer<PlayerLink>(sourcePlayer.Owner);
        var sourceData = frame.GetPlayerData(sourceLink->Player);
        sourceName = sourceData.PlayerName;
      }
      frame.Events.Message(sourceName, targetData.PlayerName);
      frame.Destroy(targetCharacter);
    }
  }
}
