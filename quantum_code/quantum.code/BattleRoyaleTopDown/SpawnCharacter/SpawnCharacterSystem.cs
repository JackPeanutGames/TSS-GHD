using Photon.Deterministic;

namespace Quantum
{
  public unsafe class SpawnCharacterSystem : SystemMainThreadFilter<SpawnCharacterSystem.Filter>, ISignalOnCharacterSpawn
  {
    public struct Filter
    {
      public EntityRef Entity;
      public PlayerLink* PlayerLink;
      public SpawnCharacterTimer* SpawnTimer;
    }


    public override void Update(Frame frame, ref Filter filter)
    {
      EntityRef linkEntity = filter.Entity;
      PlayerLink* link = filter.PlayerLink;
      SpawnCharacterTimer* spawnTimer = filter.SpawnTimer;

      spawnTimer->Timer -= frame.DeltaTime;

      if (spawnTimer->Timer <= 0)
      {
        frame.Signals.OnCharacterSpawn(linkEntity, filter.SpawnTimer->SpawnIndex);
        frame.Remove<SpawnCharacterTimer>(linkEntity);
      }
    }

    public void OnCharacterSpawn(Frame frame, EntityRef playerLink, int spawnIndex)
    {
      PlayerLink* link = frame.Unsafe.GetPointer<PlayerLink>(playerLink);

      RuntimePlayer playerData = frame.GetPlayerData(link->Player);
      EntityRef character = frame.Create(playerData.SelectedCharacter);

      link->Character = character;

      Transform2D* transform = frame.Unsafe.GetPointer<Transform2D>(character);
      transform->Position = GetSpawnPointPosition(frame, spawnIndex);

      PlayerCharacter* playerCharacter = frame.Unsafe.GetPointer<PlayerCharacter>(character);
      playerCharacter->Owner = playerLink;
    }

    private FPVector2 GetSpawnPointPosition(Frame frame, int spawnIndex)
    {
      FPVector2 position = FPVector2.Zero;
      int spawnCount = frame.ComponentCount<SpawnPoint>();
      if (spawnCount != 0)
      {
        int index = spawnIndex;
        if (index == -1)
        {
          index = frame.RNG->Next(0, spawnCount);
        }

        int count = 0;
        foreach (var (spawn, spawnPoint) in frame.Unsafe.GetComponentBlockIterator<SpawnPoint>())
        {
          if (count == index)
          {
            position = frame.Get<Transform2D>(spawn).Position;
            break;
          }
          count++;
        }
      }
      return position;
    }
  }
}
