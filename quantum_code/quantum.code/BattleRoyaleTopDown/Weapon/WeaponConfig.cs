using Photon.Deterministic;
using System;


namespace Quantum
{
  public unsafe partial class WeaponConfig
  {
    public int InitialAmmo = 10;
    public bool InfiniteAmmo = false;
    public FP FireRate;
    public int MaxAmmo;
    public FP RechargeTimer;
    public FP TimeToRecharge;
    public AssetRefProjectileConfig ProjectileConfig;
    public AssetRefCollectibleConfig CollectibleConfig;
    public int IndexOnView;

    public void WeaponAction(Frame frame, Weapon* weapon, EntityRef character, FPVector2 direction)
    {
      weapon->CurrentAmmo -= 1;
      if (weapon->CurrentAmmo == 0)
      {
        weapon->IsRecharging = true;
        weapon->DelayToStartRechargeTimer = -1;
      }
      weapon->DelayToStartRechargeTimer = TimeToRecharge;
      ProjectileConfig projectileConfig = frame.FindAsset<ProjectileConfig>(ProjectileConfig.Id);

      projectileConfig.SpawnProjectile(frame, character, direction);

      weapon->FireRateTimer = 1 / FireRate;
      weapon->ChargeTime = FP._0;
    }

    public void DropWeapon(Frame frame, EntityRef character)
    {
      FPVector2 position = frame.Unsafe.GetPointer<Transform2D>(character)->Position;
      frame.Signals.OnDropCollectible(CollectibleConfig, position);
    }
  }
}