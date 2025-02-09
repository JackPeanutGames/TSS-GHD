using Photon.Deterministic;
using System;

namespace Quantum
{
  [Serializable]
  public unsafe partial class GrenadeConfig : ProjectileConfig
  {
    public FP ExplosionRadius = 2;
    public FP LaunchHeight = 2;
    public FP LaunchYVelocity = 2;

    public override void UpdateProjectilePosition(Frame frame, EntityRef projectile)
    {
    }
    public override void SpawnProjectile(Frame frame, EntityRef characterEntity, FPVector2 direction)
    {
      EntityRef projectile = frame.Create(ProjectilePrototype);
      ProjectileFields* fields = frame.Unsafe.GetPointer<ProjectileFields>(projectile);
      Transform2D* transform = frame.Unsafe.GetPointer<Transform2D>(projectile);
      PhysicsBody2D* body = frame.Unsafe.GetPointer<PhysicsBody2D>(projectile);
      Transform2DVertical* transformVertical = frame.Unsafe.GetPointer<Transform2DVertical>(projectile);
      CustomGravity* customGravity = frame.Unsafe.GetPointer<CustomGravity>(projectile);

      Transform2D* characterTransform = frame.Unsafe.GetPointer<Transform2D>(characterEntity);
      transform->Position = characterTransform->Position;

      fields->ProjectileConfig = this;
      fields->Direction = direction * Velocity;
      fields->Source = characterEntity;
      fields->TTL = TTL;

      body->Velocity = direction * Velocity;

      transformVertical->Position = LaunchHeight;
      customGravity->VerticalSpeed = LaunchYVelocity;
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
        if(SelfDamage == false && hits[i].Entity == fields->Source)
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