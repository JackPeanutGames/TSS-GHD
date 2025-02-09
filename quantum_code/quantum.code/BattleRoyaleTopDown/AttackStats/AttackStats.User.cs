using Photon.Deterministic;

namespace Quantum
{
  public unsafe partial struct AttackStats
  {

    public void PerformAttack(Frame frame, EntityRef targetCharacter, EntityRef sourceCharacter, int baseDamage)
    {
      int damage = (int)FPMath.Ceiling(baseDamage * (1 + CurrentValue/100));
      frame.Signals.OnCharacterDamage(targetCharacter, sourceCharacter,  damage);
    }
  }
}
