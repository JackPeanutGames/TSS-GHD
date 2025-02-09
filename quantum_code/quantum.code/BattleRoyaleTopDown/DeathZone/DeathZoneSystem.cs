using System;
using System.Collections.Generic;
using System.Linq;
using Quantum.Collections;
using Photon.Deterministic;

namespace Quantum
{
  public unsafe class DeathZoneSystem : SystemMainThread, ISignalOnWaitingDeathZoneEnd, ISignalOnZoneShrinkingEnd
  {
    public override void OnInit(Frame frame)
    {
      DeathZone* zone = frame.Unsafe.GetPointerSingleton<DeathZone>();
      DeathZoneConfig config = frame.FindAsset<DeathZoneConfig>(zone->Config.Id);

      zone->CurrentCenter = FPVector2.Zero;
      zone->InitialIterationCenter = zone->CurrentCenter;
      zone->CurrentRadius = config.Radius[0];
      zone->ChangeStateDelay = config.Timers[0];

      zone->TargetRadius = config.Radius[1];
      zone->TargetCenter = FPVector2Helper.RandomInsideCircle(frame, zone->CurrentCenter, zone->CurrentRadius - zone->TargetRadius);
      zone->Iteration = 0;
    }

    public override void Update(Frame frame)
    {
      DeathZone* zone = frame.Unsafe.GetPointerSingleton<DeathZone>();
      DeathZoneConfig config = frame.FindAsset<DeathZoneConfig>(zone->Config.Id);

      zone->ChangeStateDelay -= frame.DeltaTime;
      switch (zone->State)
      {
        case DeathZoneState.Waiting:
          zone->UpdateDamage(frame);
          if (zone->ChangeStateDelay <= 0)
          {
            frame.Signals.OnWaitingDeathZoneEnd();
          }
          break;
        case DeathZoneState.ZoneShrinking:
          zone->UpdateDamage(frame);
          if (ZoneShrinkingEndCondition(frame, zone))
          {
            frame.Signals.OnZoneShrinkingEnd();
          }
          else
          {
            zone->UpdateSize(frame);
          }
          break;
        default:
          break;
      }
    }

    private bool ZoneShrinkingEndCondition(Frame frame, DeathZone* zone)
    {
      if (zone->CurrentRadius <= zone->TargetRadius && zone->CurrentCenter == zone->TargetCenter)
      {
        return true;
      }
      return false;
    }


    public void OnWaitingDeathZoneEnd(Frame frame)
    {
      DeathZone* zone = frame.Unsafe.GetPointerSingleton<DeathZone>();
      DeathZoneConfig config = frame.FindAsset<DeathZoneConfig>(zone->Config.Id);
      zone->State = DeathZoneState.ZoneShrinking;
      int index = zone->GetCurrentZoneIndex(frame);
      zone->ChangeStateDelay = config.Timers[index];
      frame.Events.StartZoneShrinking();
    }

    public void OnZoneShrinkingEnd(Frame frame)
    {
      DeathZone* zone = frame.Unsafe.GetPointerSingleton<DeathZone>();
      DeathZoneConfig config = frame.FindAsset<DeathZoneConfig>(zone->Config.Id);

      zone->Iteration++;
      zone->Iteration = FPMath.Clamp(zone->Iteration, 0, config.Radius.Length - 1);

      int nextIndex = zone->GetNextZoneIndex(frame);
      zone->TargetRadius = config.Radius[nextIndex];
      zone->ChangeStateDelay = config.Timers[nextIndex];

      FP allowedRadius = 0;
      if (zone->CurrentRadius != zone->TargetRadius)
      {
        allowedRadius = zone->CurrentRadius - zone->TargetRadius;
      }
      else
      {
        allowedRadius = zone->CurrentRadius * 2;
      }

      zone->TargetCenter = FPVector2Helper.RandomInsideCircle(frame, zone->CurrentCenter, allowedRadius);
      zone->InitialIterationCenter = zone->CurrentCenter;
      zone->State = DeathZoneState.Waiting;
    }
  }
}