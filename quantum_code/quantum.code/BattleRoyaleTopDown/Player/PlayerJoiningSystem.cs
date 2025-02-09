using Photon.Deterministic;

namespace Quantum
{
  public unsafe class PlayerJoiningSystem : SystemSignalsOnly, ISignalOnPlayerDataSet
  {
    public void OnPlayerDataSet(Frame frame, PlayerRef player)
    {
      CreatePlayerLink(frame, player);

    }

    private void CreatePlayerLink(Frame frame, PlayerRef player)
    {
      EntityRef entity = frame.Create();
      frame.Add<PlayerLink>(entity, out var playerLink);
      playerLink->Player = (byte)player;

      frame.Add<SpawnCharacterTimer>(entity, out var spawnTimer);
      spawnTimer->Timer = 1;
      spawnTimer->SpawnIndex = player;
    }
  }
}
