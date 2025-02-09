using Photon.Deterministic;

namespace Quantum
{
  public unsafe class ScoreSystem : SystemSignalsOnly, ISignalOnCharacterDie
  {
    public void OnCharacterDie(Frame frame, EntityRef targetCharacter, EntityRef sourceCharacter)
    {
      if (frame.Unsafe.TryGetPointer<PlayerScore>(sourceCharacter, out var score))
      {
        score->Kills++;
      }
    }
  }
}
