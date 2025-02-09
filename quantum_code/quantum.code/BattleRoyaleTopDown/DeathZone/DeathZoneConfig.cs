using System;
using Photon.Deterministic;

namespace Quantum
{
  public unsafe partial class DeathZoneConfig : AssetObject
  {
    public FP ReductionRate = 1;
    public FP DamageInterval;
    
    public FP[] Radius;
    public FP[] Timers;
    public int[] Damages;

    public bool DebugMode = false;
  }
}
