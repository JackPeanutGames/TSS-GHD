using Photon.Deterministic;

namespace Quantum
{
  public unsafe partial struct WeaponInventory
  {
    public void Collect(Frame frame, AssetRefWeaponConfig weaponConfig, EntityRef character)
    {
      Weapon* weaponSlot = FindCollectSlot(frame, weaponConfig);
      WeaponConfig config = frame.FindAsset<WeaponConfig>(weaponConfig.Id);
      if (weaponSlot->Config.Id == weaponConfig.Id)
      {
        weaponSlot->TotalAmmo += config.InitialAmmo;
      }
      else
      {
        if (weaponSlot->Config != default)
        {
          WeaponConfig oldConfig = frame.FindAsset<WeaponConfig>(weaponSlot->Config.Id);
          oldConfig.DropWeapon(frame, character);
        }
        weaponSlot->Init(frame, config);
      }
    }

    public void DropAll(Frame frame, EntityRef character)
    {
      for (int i = 1; i < Weapons.Length; i++)
      {
        Weapon* weapon = Weapons.GetPointer(i);
        WeaponConfig config = frame.FindAsset<WeaponConfig>(weapon->Config.Id);
        if (config == null)
        {
          continue;
        }
        config.DropWeapon(frame, character);
      }
    }


    private Weapon* FindCollectSlot(Frame frame, AssetRefWeaponConfig weaponConfig)
    {
      int index = 0;
      if (CheckNewWaepon(frame, weaponConfig, out index))
      {
        if (FindFreeSlot(frame, out index))
        {
          return Weapons.GetPointer(index);
        }
        else
        {
          if (CurrentWeaponIndex == 0)
          {
            return Weapons.GetPointer(1);
          }
          else
          {
            return Weapons.GetPointer(CurrentWeaponIndex);
          }
        }
      }
      return Weapons.GetPointer(index);
    }

    private bool CheckNewWaepon(Frame frame, AssetRefWeaponConfig weaponConfig, out int index)
    {
      index = 0;
      for (; index < Weapons.Length; index++)
      {
        Weapon* currentWeapon = Weapons.GetPointer(index);
        if (currentWeapon->Config == weaponConfig)
        {
          return false;
        }
      }
      return true;
    }

    private bool FindFreeSlot(Frame frame, out int index)
    {
      index = 0;
      for (; index < Weapons.Length; index++)
      {
        Weapon* currentWeapon = Weapons.GetPointer(index);
        if (currentWeapon->Config == null)
        {
          return true;
        }
      }
      return false;
    }
  }
}
