namespace Quantum
{
  public unsafe abstract partial class PowerUpConfig
  {
    public AssetRefEntityPrototype CollectiblePrototype;
    public virtual unsafe void Action(Frame frame, EntityRef powerUp, EntityRef targetCharacter, AssetRefPowerUpConfig config)
    {
    }
  }
}