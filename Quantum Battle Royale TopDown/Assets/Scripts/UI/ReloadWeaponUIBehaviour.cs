using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Quantum;

public class ReloadWeaponUIBehaviour : QuantumCallbacks
{
  [SerializeField]
  private Image _realodUI;
  [SerializeField]
  private Color _weaponColor;
  private GameManager _gameManager;

  private void Start()
  {
    _gameManager = FindObjectOfType<GameManager>();
  }

  public override void OnUpdateView(QuantumGame game)
  {
    if (_gameManager.LocalView == null)
    {
      return;
    }
    Frame frame = game.Frames.Predicted;
    if (frame.Exists(_gameManager.LocalView.EntityRef) == false)
    {
      return;
    }

    var weaponInventory = frame.Get<WeaponInventory>(_gameManager.LocalView.EntityRef);
    var currentWeapon = weaponInventory.Weapons[weaponInventory.CurrentWeaponIndex];
    var config = frame.FindAsset<WeaponConfig>(currentWeapon.Config.Id);

    var ammoRatio = (float)currentWeapon.CurrentAmmo / config.MaxAmmo;

    //var fireRateRatio = Mathf.Clamp01(currentWeapon.FireRateTimer.AsFloat / (1 / config.FireRate.AsFloat));

    _realodUI.fillAmount = ammoRatio;
    _weaponColor.a = currentWeapon.IsRecharging ? 0.5f : 1;
  }
}
