using Photon.Deterministic;

namespace Quantum
{
  public unsafe class HealthSystem : SystemMainThreadFilter<HealthSystem.Filter>, ISignalOnComponentAdded<Health>, ISignalOnCharacterDamage
  {
    private static readonly FP HealPercentage = FP._0_10 + FP._0_03;
    private static readonly FP DelayToStartHealing = 3;
    private static readonly FP DelayToHeal = 1;

    public struct Filter
    {
      public EntityRef Entity;
      public Health* Health;
    }

    public void OnAdded(Frame frame, EntityRef entity, Health* component)
    {
      SetNextHealTick(frame, component);
      component->CurrentValue = component->InitialValue;
    }

    public override void Update(Frame frame, ref Filter filter)
    {
      EntityRef entity = filter.Entity;
      Health* health = filter.Health;


      if (health->CurrentValue == health->MaxValue || frame.Number * frame.DeltaTime < filter.Health->StartHealingTime)
      {
        return;
      }

      if (filter.Health->CanSelfHeal == false || filter.Health->IsDead)
      {
        return;
      }

      if (frame.Number * frame.DeltaTime >= filter.Health->NextHealTime)
      {
        //health->IncreaseHealth(frame, entity, HealPercentage);
        SetNextHealTick(frame, filter.Health);
      }
    }

    public void OnCharacterDamage(Frame frame, EntityRef targetCharacter, EntityRef sourceCharacter, int damage)
    {
      if (frame.Unsafe.TryGetPointer<Health>(targetCharacter, out var health))
      {
        SetNextStartHealingTick(frame, health);
        health->CurrentValue -= damage;

        frame.Events.CharacterDamaged(targetCharacter, damage);

        if (health->CurrentValue <= 0)
        {
          health->IsDead = true;
          frame.Signals.OnCharacterDie(targetCharacter, sourceCharacter);
        }
      }
    }

    private void SetNextStartHealingTick(Frame frame, Health* healthComponent)
    {
      healthComponent->StartHealingTime = frame.Number * frame.DeltaTime + DelayToStartHealing;
      SetNextHealTick(frame, healthComponent, healthComponent->StartHealingTime + DelayToHeal);
    }

    private void SetNextHealTick(Frame frame, Health* healthComponent, FP value = default)
    {
      if (value == default)
      {
        value = frame.Number * frame.DeltaTime + DelayToHeal;
      }

      healthComponent->NextHealTime = value;
    }
  }
}
