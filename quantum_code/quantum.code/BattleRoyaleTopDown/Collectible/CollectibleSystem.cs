using Photon.Deterministic;

namespace Quantum
{
  public unsafe class CollectibleSystem : SystemMainThreadFilter<CollectibleSystem.Filter>, ISignalOnDropCollectible, ISignalOnTrigger2D, ISignalOnTriggerExit2D
  {
    public struct Filter
    {
      public EntityRef Entity;
      public Collectible* Collectible;
    }

    public void OnTrigger2D(Frame frame, TriggerInfo2D info)
    {
      if (frame.Unsafe.TryGetPointer<Collectible>(info.Entity, out var collectible))
      {
        if (frame.Has<PlayerCharacter>(info.Other))
        {
          collectible->TargetCharacter = info.Other;
        }
      }
    }

    public void OnTriggerExit2D(Frame frame, ExitInfo2D info)
    {
      if (frame.Unsafe.TryGetPointer<Collectible>(info.Entity, out var collectible))
      {
        if (collectible->TargetCharacter == info.Other)
        {
          collectible->TargetCharacter = EntityRef.None;
          CollectibleConfig config = frame.FindAsset<CollectibleConfig>(collectible->Config.Id);
          collectible->TimerToCollect = config.TimeToCollect;
        }
      }
    }

    public override void Update(Frame frame, ref Filter filter)
    {
      EntityRef collectibleEntity = filter.Entity;
      Collectible* collectible = filter.Collectible;

      if (frame.Exists(collectible->TargetCharacter))
      {
        collectible->TimerToCollect -= frame.DeltaTime;

        if (collectible->TimerToCollect <= 0)
        {
          CollectibleConfig config = frame.FindAsset<CollectibleConfig>(collectible->Config.Id);
          frame.Events.CollectItem(collectible->TargetCharacter, config);
          config.OnCollect(frame, collectibleEntity, collectible->TargetCharacter);
        }
      }
    }

    public void OnDropCollectible(Frame frame, AssetRefCollectibleConfig collectibleConfigRef, FPVector2 position)
    {
      CollectibleConfig config = frame.FindAsset<CollectibleConfig>(collectibleConfigRef.Id);
      config.CreateCollectible(frame, position);
    }
  }
}