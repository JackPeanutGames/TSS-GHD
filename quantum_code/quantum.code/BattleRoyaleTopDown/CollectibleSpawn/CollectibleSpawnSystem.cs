using System;
using Quantum;

namespace Quantum
{
  public unsafe class CollectibleSpawnSystem : SystemSignalsOnly
  {
    public override void OnInit(Frame frame)
    {
      var collectibleSpawnFilter = frame.Filter<Transform2D, CollectibleSpawnPoint>();
      while (collectibleSpawnFilter.NextUnsafe(out var entity, out var transform, out var spawnPoint))
      {
        CollectibleSpawnPointConfig spawnConfig = frame.FindAsset<CollectibleSpawnPointConfig>(spawnPoint->Config.Id);
        int prototypeIndex = frame.RNG->Next(0, spawnConfig.Collectibles.Length);
        if(spawnConfig.Collectibles[prototypeIndex] != null)
        {
          CollectibleConfig collectibleConfig = frame.FindAsset<CollectibleConfig>(spawnConfig.Collectibles[prototypeIndex].Id);
          EntityRef collectibleEntity = frame.Create(collectibleConfig.CollectiblePrototype);
          Transform2D* collectibleTransform = frame.Unsafe.GetPointer<Transform2D>(collectibleEntity);
          collectibleTransform->Position = transform->Position;
        }
      }
    }
  }
}
