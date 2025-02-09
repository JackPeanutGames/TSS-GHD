using Photon.Deterministic;

namespace Quantum
{
  public unsafe class ProjectileSystem : SystemMainThreadFilter<ProjectileSystem.Filter>
  {
    public struct Filter
    {
      public EntityRef Entity;
      public Transform2D* Transform;
      public ProjectileFields* ProjectileFields;
    }
    public override void Update(Frame frame, ref Filter filter)
    {
      EntityRef projectileEntity = filter.Entity;
      Transform2D* projectileTransform = filter.Transform;
      ProjectileFields* projectileFields = filter.ProjectileFields;

      if (frame.Exists(projectileFields->Source) == false)
      {
        frame.Destroy(projectileEntity);
        return;
      }

      ProjectileConfig projectileConfig = frame.FindAsset<ProjectileConfig>(projectileFields->ProjectileConfig.Id);
      projectileConfig.CheckDestroyCondition(frame, projectileEntity);

      if (projectileConfig.CheckCollision(frame, projectileEntity, projectileFields, projectileTransform))
      {
        return;
      }

      if(frame.Exists(projectileEntity) == false)
      {
        return;
      }

      projectileConfig.UpdateProjectilePosition(frame, projectileEntity);
    }
  }
}