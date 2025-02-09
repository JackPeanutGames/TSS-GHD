using Photon.Deterministic;
using System;

namespace Quantum
{
  [Flags]
  public enum DestroyFlag
  {
    None = 0,
    TTL = 1,
    Range = 2
  }

  public unsafe abstract partial class ProjectileConfig
  {
    public AssetRefEntityPrototype ProjectilePrototype;
    public DestroyFlag DestroyCondition;
    public FP TTL;
    public int Damage;
    public FP Range;
    public FP Velocity;
    public bool SelfDamage = false;

    public virtual void UpdateProjectilePosition(Frame frame, EntityRef projectile)
    {
      Transform2D* transform = frame.Unsafe.GetPointer<Transform2D>(projectile);
      ProjectileFields* fields = frame.Unsafe.GetPointer<ProjectileFields>(projectile);
      transform->Position += fields->Direction * frame.DeltaTime;
    }

    public virtual void ProjectileAction(Frame frame, EntityRef projectile, EntityRef targetCharacter)
    {
      if (frame.Exists(targetCharacter))
      {
        ProjectileFields* projectileFields = frame.Unsafe.GetPointer<ProjectileFields>(projectile);
        if (frame.Unsafe.TryGetPointer<AttackStats>(projectileFields->Source, out var attackStats))
        {
          attackStats->PerformAttack(frame, targetCharacter, projectileFields->Source, Damage);
        }
        else
        {
          Log.Error("There is no AttackStats!");
        }
      }
      frame.Destroy(projectile);
    }

    public virtual bool CheckCollision(Frame frame, EntityRef projectile, ProjectileFields* projectileFields, Transform2D* transform)
    {
      return false;
    }

    public virtual void SpawnProjectile(Frame frame, EntityRef characterEntity, FPVector2 direction)
    {
      EntityRef projectile = frame.Create(ProjectilePrototype);

      ProjectileFields* projectileFields = frame.Unsafe.GetPointer<ProjectileFields>(projectile);
      Transform2D* projectileTransform = frame.Unsafe.GetPointer<Transform2D>(projectile);

      projectileFields->ProjectileConfig = this;
      projectileFields->TTL = TTL;

      Transform2D* characterTransform = frame.Unsafe.GetPointer<Transform2D>(characterEntity);

      projectileTransform->Position = characterTransform->Position;

      projectileFields->Direction = direction * Velocity;
      projectileFields->Source = characterEntity;
      projectileFields->InitialPosition = characterTransform->Position;
    }

    public virtual void CheckDestroyCondition(Frame frame, EntityRef projectile)
    {
      ProjectileFields* projectileFields = frame.Unsafe.GetPointer<ProjectileFields>(projectile);
      Transform2D* projectileTransform = frame.Unsafe.GetPointer<Transform2D>(projectile);

      if ((DestroyCondition & DestroyFlag.TTL) == DestroyFlag.TTL)
      {
        if (projectileFields->TTL <= 0)
        {
          ProjectileAction(frame, projectile, EntityRef.None);
          return;
        }

        projectileFields->TTL -= frame.DeltaTime;
      }

      if ((DestroyCondition & DestroyFlag.Range) == DestroyFlag.Range)
      {
        FP distanceSquared = FPVector2.DistanceSquared(projectileTransform->Position, projectileFields->InitialPosition);
        bool projectileIsTooFar = FPMath.Sqrt(distanceSquared) > Range;

        if (projectileIsTooFar)
        {
          ProjectileAction(frame, projectile, EntityRef.None);
          return;
        }
      }
    }
  }
}