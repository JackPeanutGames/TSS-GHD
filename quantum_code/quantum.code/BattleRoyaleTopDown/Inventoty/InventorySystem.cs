using Photon.Deterministic;

namespace Quantum
{
  public unsafe class InventorySystem : SystemSignalsOnly, ISignalOnCharacterDie
  {
    public void OnCharacterDie(Frame frame, EntityRef targetCharacter, EntityRef sourceCharacter)
    {
      Transform2D characterTransform = frame.Get<Transform2D>(targetCharacter);
      WeaponInventory weaponInventory = frame.Get<WeaponInventory>(targetCharacter);
      PowerUpInventoty powerUpInventory = frame.Get<PowerUpInventoty>(targetCharacter);

      weaponInventory.DropAll(frame, targetCharacter);
      powerUpInventory.OnDropPowerUps(frame, characterTransform.Position);
    }
  }
}