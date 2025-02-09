using Photon.Deterministic;
using Quantum.Core;

namespace Quantum
{
  public unsafe class CustomGravitySystem : SystemMainThreadFilter<CustomGravitySystem.Filter>
  {
    private FP _groundHeight = 0;
    private FP _bounceThreshold = 0;
    private FP _restitution = FP._0_50;
    private FP _gravityForce = -20;
    public struct Filter
    {
      public EntityRef Entity;
      public Transform2DVertical* TransformVertical;
      public CustomGravity* customGravity;
      public PhysicsCollider2D* Collider;
    }

    public override void Update(Frame f, ref Filter filter)
    {
      EntityRef entity = filter.Entity;
      CustomGravity* customGravity = filter.customGravity;
      Transform2DVertical* transformVertical = filter.TransformVertical;
      PhysicsCollider2D* collider = filter.Collider;

      if (customGravity->VerticalSpeed <= 0)
      {
        var distance = transformVertical->Height + FPMath.Abs(customGravity->VerticalSpeed * f.DeltaTime);
        if (transformVertical->Position <= distance - (collider->Shape.Circle.Radius))
        {
          if (customGravity->VerticalSpeed > _bounceThreshold)
          {
            transformVertical->Position = _groundHeight;
            customGravity->Grounded = true;
          }
          else
          {
            customGravity->VerticalSpeed = -customGravity->VerticalSpeed / (1 + _restitution);
          }
        }
        else
        {
          customGravity->Grounded = false;
        }
      }
      else
      {
        customGravity->Grounded = false;
      }
      if (customGravity->Grounded == false)
      {
        transformVertical->Position += (customGravity->VerticalSpeed) * f.DeltaTime;
        customGravity->VerticalSpeed += _gravityForce * f.DeltaTime;
      }
    }
  }
}