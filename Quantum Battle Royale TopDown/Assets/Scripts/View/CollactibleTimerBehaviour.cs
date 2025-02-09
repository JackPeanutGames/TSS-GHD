using Quantum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollactibleTimerBehaviour : QuantumCallbacks
{
  private EntityView _entityView;
  private SpriteRenderer _sprite;
  void Start()
  {
    _sprite = GetComponent<SpriteRenderer>();
    _entityView = GetComponentInParent<EntityView>();
  }

  public override void OnUpdateView(QuantumGame game)
  {
    if (_entityView == null) return;

    Frame frame = game.Frames.Verified;
    if(frame.Exists(_entityView.EntityRef) == false)
    {
      return;
    }

    Collectible collectilbe = frame.Get<Collectible>(_entityView.EntityRef);
    CollectibleConfig config = frame.FindAsset<CollectibleConfig>(collectilbe.Config.Id);
    float timer = collectilbe.TimerToCollect.AsFloat / config.TimeToCollect.AsFloat;
    timer = 1 - timer;
    _sprite.transform.localScale = new Vector3(timer, timer, timer);
  }
}
