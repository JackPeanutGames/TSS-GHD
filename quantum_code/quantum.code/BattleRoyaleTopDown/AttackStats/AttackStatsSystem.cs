using Photon.Deterministic;

namespace Quantum
{
  public unsafe class AttackSystem : SystemSignalsOnly, ISignalOnComponentAdded<AttackStats>
  {

    public void OnAdded(Frame frame, EntityRef entity, AttackStats* component)
    {
      component->CurrentValue = component->InitialValue;
    }
  }
}
