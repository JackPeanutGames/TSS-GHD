using Quantum;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CharacterHealingView : QuantumCallbacks
{
  private ParticleSystem _healingVFX;
  private EntityView _entityView;

  private void Start()
  {
    QuantumEvent.Subscribe<EventCharacterHealed>(this, OnCharacterHealed);
    _entityView = GetComponentInParent<EntityView>();
    _healingVFX = GetComponentInChildren<ParticleSystem>();
  }

  private void OnCharacterHealed(EventCharacterHealed e)
  {
    if (e.character != _entityView.EntityRef)
    {
      return;
    }
    _healingVFX.Play();
  }
}
