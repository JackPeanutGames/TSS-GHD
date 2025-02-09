using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.Deterministic;

namespace Quantum
{
  public unsafe class MovementSystem : SystemMainThreadFilter<MovementSystem.Filter>
  {
    public struct Filter
    {
      public EntityRef Entity;
      public Transform2D* Transform;
      public PlayerCharacter* PlayerCharacter;
      public KCC* Kcc;
    }

    public override void Update(Frame frame, ref Filter filter)
    {
      EntityRef entity = filter.Entity;
      Transform2D* transform = filter.Transform;
      PlayerCharacter* playerCharacter = filter.PlayerCharacter;
      KCC* kcc = filter.Kcc;

      PlayerLink* link = frame.Unsafe.GetPointer<PlayerLink>(playerCharacter->Owner);

      Input* input = frame.GetPlayerInput(link->Player);
      MoveCharacter(frame, input, entity, kcc);
      RotateCharacter(frame, input, entity, transform);

    }


    private void MoveCharacter(Frame frame, Input* input, EntityRef entity, KCC* kcc)
    {
      KCCSettings kccSettings = frame.FindAsset<KCCSettings>(kcc->Settings.Id);
      KCCMovementData kccMovementData = kccSettings.ComputeRawMovement(frame, entity, input->MovementDirection.Normalized);
      kccSettings.SteerAndMove(frame, entity, in kccMovementData);
    }

    private void RotateCharacter(Frame f, Input* input, EntityRef entity, Transform2D* transform)
    {
      FPVector2 rotationDirection = default;

      if (input->ActionDirection.Magnitude >= FP._0_10)
      {
        rotationDirection = input->ActionDirection.Normalized;
      }
      else
      {
        FPVector2 moveDirection = input->MovementDirection.Normalized;
        if (moveDirection != default)
        {
          rotationDirection = moveDirection;
        }
      }
      if (rotationDirection != default)
      {
        transform->Rotation = FPVector2.RadiansSignedSkipNormalize(FPVector2.Up, rotationDirection);
      }
    }
  }
}
