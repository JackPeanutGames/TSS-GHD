using Photon.Deterministic;

namespace Quantum
{
  public unsafe partial struct DeathZone
  {
    public void UpdateSize(Frame frame)
    {
      DeathZoneConfig config = frame.FindAsset<DeathZoneConfig>(Config.Id);
      FP elapsed = config.Timers[Iteration] - ChangeStateDelay;
      FP t = FPMath.Clamp01(elapsed / config.Timers[Iteration]);

      CurrentRadius = FPMath.Lerp(config.Radius[Iteration], TargetRadius, t);
      CurrentCenter = FPVector2.Lerp(InitialIterationCenter, TargetCenter, t);
    }

    public void UpdateDamage(Frame frame)
    {
      DeathZoneConfig config = frame.FindAsset<DeathZoneConfig>(Config.Id);
      if (config.DebugMode)
      {
        Draw.Circle(CurrentCenter, CurrentRadius, new ColorRGBA(200, 200, 200, 50));
        Draw.Circle(TargetCenter, TargetRadius, new ColorRGBA(200, 100, 100, 100));
      }

      DamageDelay -= frame.DeltaTime;
      if (DamageDelay <= 0)
      {
        DamageDelay = config.DamageInterval;

        var playerCharacterFilter = frame.Filter<Transform2D, PlayerCharacter>();
        while (playerCharacterFilter.NextUnsafe(out var entity, out var transform, out var playerCharacter))
        {
          if (IsOnDamageZone(frame, transform->Position))
          {
            int damage = config.Damages[GetCurrentZoneIndex(frame)];
            frame.Signals.OnCharacterDamage(entity, EntityRef.None, damage);
          }
        }
      }
    }

    public bool IsOnTargetSafeArea(Frame frame, FPVector2 position)
    {
      FP distance = FPVector2.Distance(position, TargetCenter);
      if (distance > TargetRadius)
      {
        return false;
      }
      return true;
    }

    private bool IsOnDamageZone(Frame frame, FPVector2 position)
    {
      FP distance = FPVector2.Distance(position, CurrentCenter);
      if (distance > CurrentRadius)
      {
        return true;
      }
      return false;
    }

    public int GetCurrentZoneIndex(Frame frame)
    {
      DeathZoneConfig config = frame.FindAsset<DeathZoneConfig>(Config.Id);
      return Iteration >= config.Radius.Length ? config.Radius.Length - 1 : Iteration;
    }

    public int GetNextZoneIndex(Frame frame)
    {
      DeathZoneConfig config = frame.FindAsset<DeathZoneConfig>(Config.Id);
      int nextIndex = Iteration + 1;
      return nextIndex >= config.Radius.Length ? config.Radius.Length - 1 : nextIndex;
    }
  }
}
