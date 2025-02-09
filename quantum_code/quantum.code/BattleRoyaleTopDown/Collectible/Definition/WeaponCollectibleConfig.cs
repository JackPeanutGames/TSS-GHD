using Photon.Deterministic;
using System;


namespace Quantum
{
  [Serializable]

  public unsafe partial class WeaponCollectibleConfig : CollectibleConfig
  {
    public AssetRefWeaponConfig WeaponConfig;

    public override void OnCollect(Frame frame, EntityRef collectibleEntity, EntityRef characterEntity)
    {
      WeaponInventory* inventory = frame.Unsafe.GetPointer<WeaponInventory>(characterEntity);
      inventory->Collect(frame, WeaponConfig, characterEntity);
      frame.Destroy(collectibleEntity);
    }
  }
}
