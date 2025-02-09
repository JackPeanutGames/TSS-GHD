using Photon.Deterministic;
using System;

namespace Quantum
{
  [Serializable]
  public unsafe partial class SimpleProjectileConfig : ProjectileConfig
  {
    public override bool CheckCollision(Frame frame, EntityRef projectile, ProjectileFields* projectileFields, Transform2D* transform)
    {
      if (projectileFields->Direction.Magnitude <= 0)
      {
        return false;
      }

      FPVector2 futurePosition = transform->Position + projectileFields->Direction * frame.DeltaTime;

      if (FPVector2.DistanceSquared(transform->Position, futurePosition) <= FP._0_01)
      {
        return false;
      }


      Physics2D.HitCollection hits = frame.Physics2D.LinecastAll(transform->Position, futurePosition);
      for (int i = 0; i < hits.Count; i++)
      {
        var entity = hits[i].Entity;
        if (entity != EntityRef.None && frame.Has<PlayerCharacter>(entity) && entity != projectileFields->Source)
        {
          transform->Position = hits[i].Point;
          ProjectileAction(frame, projectile, entity);
          return true;
        }

        if (entity == EntityRef.None)
        {
          transform->Position = hits[i].Point;
          ProjectileAction(frame, projectile, EntityRef.None);
          return true;
        }
      }
      return false;
    }
  }
}