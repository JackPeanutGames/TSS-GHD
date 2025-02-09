using Photon.Deterministic;

namespace Quantum
{
  public unsafe class WeaponSystem : SystemMainThreadFilter<WeaponSystem.Filter>
  {
    public struct Filter
    {
      public EntityRef Entity;
      public PlayerCharacter* PlayerCharacter;
      public Transform2D* Transform;
      public WeaponInventory* WeaponInventoty;
    }

    public override void Update(Frame frame, ref Filter filter)
    {
      EntityRef character = filter.Entity;
      PlayerCharacter* playerCharacter = filter.PlayerCharacter;
      Transform2D* transform = filter.Transform;
      WeaponInventory* inventory = filter.WeaponInventoty;

      PlayerLink* link = frame.Unsafe.GetPointer<PlayerLink>(playerCharacter->Owner);

      Weapon* currentWeapon = inventory->Weapons.GetPointer(inventory->CurrentWeaponIndex);

      currentWeapon->UpdateTimers(frame);

      if (currentWeapon->CanRecharge(frame))
      {
        currentWeapon->ReloadAmmo(frame);
      }

      Input* i = frame.GetPlayerInput(link->Player);
      if (currentWeapon->CanShoot(frame))
      {

        if (i->Fire.IsDown)
        {
          WeaponConfig weaponConfig = frame.FindAsset<WeaponConfig>(currentWeapon->Config.Id);
          weaponConfig.WeaponAction(frame, currentWeapon, character, i->ActionDirection);
        }
      }

      if (i->WeaponIndex != inventory->CurrentWeaponIndex && i->WeaponIndex != byte.MaxValue)
      {
        int nextWeaponIndex = FPMath.Clamp(i->WeaponIndex, 0, Constants.INVENTORY_SIZE - 1);
        TryChangeWeapon(frame, character, inventory, nextWeaponIndex);
      }
    }

    private void TryChangeWeapon(Frame frame, EntityRef character, WeaponInventory* inventory, int weaponIndex)
    {
      if (inventory->Weapons[weaponIndex].Config != null)
      {
        inventory->CurrentWeaponIndex = weaponIndex;
        frame.Events.ChangeWeapon(character, inventory->Weapons[weaponIndex].Config);
      }
    }
  }
}