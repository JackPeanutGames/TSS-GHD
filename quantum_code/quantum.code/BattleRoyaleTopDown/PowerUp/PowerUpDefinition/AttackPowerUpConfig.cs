using System;
using Photon.Deterministic;

namespace Quantum
{
  [Serializable]

  public unsafe partial class AttackPowerUpConfig : PowerUpConfig
  {
    public FP AttackIncreaseAmount = 10;

    public override unsafe void Action(Frame frame, EntityRef powerUp, EntityRef targetCharacter, AssetRefPowerUpConfig config)
    {
      if (frame.Unsafe.TryGetPointer<PowerUpInventoty>(targetCharacter, out var powerUpInventory))
      {
        powerUpInventory->OnCollect(frame, powerUp, config);
      }

      if (frame.Unsafe.TryGetPointer<AttackStats>(targetCharacter, out var attackStats))
      {
        attackStats->CurrentValue += AttackIncreaseAmount;
      }
      else
      {
        Log.Error("There is no AttackStats component!");
      }

      frame.Destroy(powerUp);
    }
  }
}
