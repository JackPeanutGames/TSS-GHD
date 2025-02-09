using Photon.Deterministic;
using System;

namespace Quantum
{
  [Serializable]
  public unsafe partial class RocketProjectileConfig : SimpleProjectileConfig
  {
    public FP ExplosionRadius = 2;
    public FPAnimationCurve VelocityCurve;

    public override void UpdateProjectilePosition(Frame frame, EntityRef projectile)
    {
      Transform2D* transform = frame.Unsafe.GetPointer<Transform2D>(projectile);
      ProjectileFields* fields = frame.Unsafe.GetPointer<ProjectileFields>(projectile);
      FP lifeTime = FPMath.Abs(fields->TTL / TTL);
      FP currentVelocity = VelocityCurve.Evaluate(1 - lifeTime);
      fields->Direction = fields->Direction.Normalized * currentVelocity * Velocity;
      transform->Position += fields->Direction * frame.DeltaTime;
    }

    public override void ProjectileAction(Frame frame, EntityRef projectile, EntityRef targetCharacter)
    {
      ProjectileFields* fields = frame.Unsafe.GetPointer<ProjectileFields>(projectile);
      Transform2D* transform = frame.Unsafe.GetPointer<Transform2D>(projectile);
      var shape = Shape2D.CreateCircle(ExplosionRadius);
      var layer = frame.Layers.GetLayerMask("Character");
      var hits = frame.Physics2D.OverlapShape(transform->Position, 0, shape, layer);
      for (int i = 0; i < hits.Count; i++)
      {
        if (SelfDamage == false && hits[i].Entity == fields->Source)
        {
          continue;
        }
        if (frame.Unsafe.TryGetPointer<AttackStats>(fields->Source, out var attackStats))
        {
          attackStats->PerformAttack(frame, hits[i].Entity, fields->Source, Damage);
        }
        else
        {
          Log.Warn("There is no AttackStats component attached to the source entity.");
        }
      }
      frame.Destroy(projectile);
    }
  }
}