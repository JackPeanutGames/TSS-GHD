using Quantum;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AttackPowerUpView : QuantumCallbacks
{
  private ParticleSystem _powerUpVFX;
  private EntityView _entityView;

  private void Start()
  {
    QuantumEvent.Subscribe<EventCollectPowerUp>(this, OnCollectPowerUp);
    _entityView = GetComponentInParent<EntityView>();
    _powerUpVFX = GetComponentInChildren<ParticleSystem>();
  }

  private void OnCollectPowerUp(EventCollectPowerUp e)
  {
    if(e.powerUpConfig.GetType() != typeof(AttackPowerUpConfig))
    {
      return;
    }
    if (e.character != _entityView.EntityRef)
    {
      return;
    }
    _powerUpVFX.Play();
  }
}
