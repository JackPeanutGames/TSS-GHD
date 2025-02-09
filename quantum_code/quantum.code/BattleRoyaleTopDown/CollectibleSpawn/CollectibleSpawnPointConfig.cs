using System;
using Photon.Deterministic;

namespace Quantum
{
  public unsafe partial class CollectibleSpawnPointConfig : AssetObject
  {
    public AssetRefCollectibleConfig[] Collectibles;
  }
}
