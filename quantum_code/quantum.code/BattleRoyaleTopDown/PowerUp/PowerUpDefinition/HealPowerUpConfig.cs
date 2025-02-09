using System;
using Photon.Deterministic;

namespace Quantum
{
  [Serializable]

  public unsafe partial class  HealPowerUpConfig : PowerUpConfig
  {
    public FP HealthIncreaseAmount = 10;
    public FP HealPercentage = 10;

    public override unsafe void Action(Frame frame, EntityRef powerUp, EntityRef targetCharacter, AssetRefPowerUpConfig config)
    {
      if(frame.Unsafe.TryGetPointer<PowerUpInventoty>(targetCharacter, out var powerUpInventoty))
      {
        powerUpInventoty->OnCollect(frame, powerUp, config);
      }

      if (frame.Unsafe.TryGetPointer<Health>(targetCharacter, out var health))
      {
        health->MaxValue += HealthIncreaseAmount;
        health->IncreaseHealth(frame, targetCharacter, HealPercentage / 100);
      }
      frame.Destroy(powerUp);
    }
  }
}
