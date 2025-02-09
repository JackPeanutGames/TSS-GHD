using Photon.Deterministic;

namespace Quantum
{
  public unsafe partial struct PowerUpInventoty
  {
    public void OnDropPowerUps(Frame frame, FPVector2 position)
    {
      for (int i = 0; i < Slots.Length; i++)
      {
        var slot = Slots.GetPointer(i);
        if(slot->Config == null)
        {
          continue;
        }
        for (int j = 0; j < slot->Amount; j++)
        {
          PowerUpConfig config = frame.FindAsset<PowerUpConfig>(slot->Config.Id);
          EntityRef powerUp = frame.Create(config.CollectiblePrototype);
          Transform2D* transform = frame.Unsafe.GetPointer<Transform2D>(powerUp);
          transform->Position = FPVector2Helper.RandomInsideCircle(frame, position, 1);
        }
      }
    }

    public void OnCollect(Frame frame, EntityRef powerUpEntity, AssetRefPowerUpConfig config)
    {
      int i = 0;
      for (; i < Slots.Length; i++)
      {
        var slot = Slots.GetPointer(i);

        if(slot->Config == default)
        {
          break;
        }
        if (slot->Config == config)
        {
          slot->Amount++;
          return;
        }
      }

      Slots.GetPointer(i)->Config = config;
      Slots.GetPointer(i)->Amount = 1;
    }
  }
}
