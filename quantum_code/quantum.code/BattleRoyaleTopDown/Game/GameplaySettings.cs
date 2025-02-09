using System;
using Photon.Deterministic;

namespace Quantum
{
  public unsafe partial class GameplaySettings : AssetObject
  {
    public FP WaitForPlayersTime = 3;
    public FP SetupMatchTime = 0;
    public FP PresentationTime = 3;
  }
}
