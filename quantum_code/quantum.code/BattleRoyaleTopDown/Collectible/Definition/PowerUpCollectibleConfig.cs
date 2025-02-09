using Photon.Deterministic;
using System;


namespace Quantum
{
  [Serializable]

  public unsafe partial class PowerUpCollectibleConfig : CollectibleConfig
  {
    public AssetRefPowerUpConfig PowerUpConfig;
    public override void OnCollect(Frame frame, EntityRef collectibleEntity, EntityRef characterEntity)
    {
      PowerUpConfig config = frame.FindAsset<PowerUpConfig>(PowerUpConfig.Id);
      config.Action(frame, collectibleEntity, characterEntity, PowerUpConfig);
      frame.Events.CollectPowerUp(characterEntity, config);
    }
  }
}
