using Photon.Deterministic;
using System;


namespace Quantum
{
  public unsafe abstract partial class CollectibleConfig
  {
    public FP TimeToCollect = 2;
    public AssetRefEntityPrototype CollectiblePrototype;

    public abstract void OnCollect(Frame frame, EntityRef collectibleEntity, EntityRef characterEntity);

    public virtual void CreateCollectible(Frame frame, FPVector2 position)
    {
      EntityRef collectible = frame.Create(CollectiblePrototype);
      Transform2D* transform = frame.Unsafe.GetPointer<Transform2D>(collectible);

      transform->Position = FPVector2Helper.RandomInsideCircle(frame, position, 1);
    }
  }
}
