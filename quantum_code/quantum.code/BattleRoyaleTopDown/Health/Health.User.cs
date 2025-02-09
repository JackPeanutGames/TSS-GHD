using Photon.Deterministic;

namespace Quantum
{
  public unsafe partial struct Health
  {

    public void IncreaseHealth(Frame frame, EntityRef character, FP percentage)
    {
      frame.Events.CharacterHealed(character);
      FP healValue = FPMath.Ceiling(MaxValue * percentage);
      CurrentValue += healValue;
      CurrentValue = FPMath.Clamp(CurrentValue, 0, MaxValue);
    }
  }
}
