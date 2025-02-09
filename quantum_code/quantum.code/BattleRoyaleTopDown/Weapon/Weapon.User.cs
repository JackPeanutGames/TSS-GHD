using Photon.Deterministic;

namespace Quantum
{
  public unsafe partial struct Weapon
  {
    public void Init(Frame frame, AssetRefWeaponConfig configRef)
    {
      Config = configRef;
      WeaponConfig config = frame.FindAsset<WeaponConfig>(configRef.Id);
      TotalAmmo = config.InitialAmmo;
      IsRecharging = false;
      CurrentAmmo = config.MaxAmmo;
      FireRateTimer = 0;
      RechargeRate = 0;
      ChargeTime = 0;
    }
    public void UpdateTimers(Frame frame)
    {
      FireRateTimer -= frame.DeltaTime;
      DelayToStartRechargeTimer -= frame.DeltaTime;
      RechargeRate -= frame.DeltaTime;
    }

    public bool CanRecharge(Frame f)
    {
      WeaponConfig config = f.FindAsset<WeaponConfig>(Config.Id);
      return DelayToStartRechargeTimer < 0 && RechargeRate <= 0 && CurrentAmmo < config.MaxAmmo;
    }

    public bool CanShoot(Frame f)
    {
      return FireRateTimer <= FP._0 && IsRecharging == false && CurrentAmmo > 0;
    }

    public void ReloadAmmo(Frame frame)
    {
      WeaponConfig config = frame.FindAsset<WeaponConfig>(Config.Id);
      if (config.InfiniteAmmo == false)
      {

        if (TotalAmmo == 0)
        {
          IsRecharging = false;
          return;
        }
        TotalAmmo--;
      }
      RechargeRate = config.RechargeTimer / (FP)config.MaxAmmo;
      CurrentAmmo++;

      if (CurrentAmmo == config.MaxAmmo)
      {
        IsRecharging = false;
      }
    }
  }
}
