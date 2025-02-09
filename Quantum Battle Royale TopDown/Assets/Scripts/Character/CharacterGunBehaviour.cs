using Quantum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGunBehaviour : QuantumCallbacks
{
  public Transform WeaponsParent;

  [SerializeField]
  private GameObject[] _weaponsModels;
  private EntityView _entityView;

  void Start()
  {
    _entityView = GetComponentInParent<EntityView>();
    QuantumEvent.Subscribe<EventChangeWeapon>(this, OnChangeWeapon);
  }

  private void OnChangeWeapon(EventChangeWeapon e)
  {
    if(e.character != _entityView.EntityRef)
    {
      return;
    }

    Frame frame = e.Game.Frames.Verified;
    WeaponConfig config = frame.FindAsset<WeaponConfig>(e.config.Id);
    for (int i = 0; i < _weaponsModels.Length; i++)
    {
      _weaponsModels[i].SetActive(i == config.IndexOnView);
    }
  }
}
