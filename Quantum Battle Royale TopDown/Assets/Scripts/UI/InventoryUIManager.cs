using Quantum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUIManager : QuantumCallbacks
{
  [SerializeField]
  private Image _healthUI;
  public Image[] _selectItemIndicators;
  public TextMeshProUGUI[] _ammoCount;
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
    UpdateHealthBar(frame);
  }

  private void UpdateAmmoCount(Frame frame)
  {
    WeaponInventory weaponInventory = frame.Get<WeaponInventory>(_gameManager.LocalView.EntityRef);
    for (int i = 0; i < _ammoCount.Length; i++)
    {
      if(weaponInventory.Weapons[i+1].Config == null)
      {
        _ammoCount[i].text = "";
        continue;
      }
      _ammoCount[i].text = (weaponInventory.Weapons[i+1].TotalAmmo + weaponInventory.Weapons[i+1].CurrentAmmo).ToString();
    }
  }

  private void UpdateHealthBar(Frame frame)
  {
    Health health = frame.Get<Health>(_gameManager.LocalView.EntityRef);
    _healthUI.fillAmount = (health.CurrentValue / health.MaxValue).AsFloat;
  }

  private void UpdateSelectedItem(Frame frame)
  {
    WeaponInventory weaponInventory = frame.Get<WeaponInventory>(_gameManager.LocalView.EntityRef);
    for (int i = 0; i < _selectItemIndicators.Length; i++)
    {
      if (weaponInventory.Weapons[i].Config == null)
      {
        _selectItemIndicators[i].enabled = false;
        continue;
      }
      _selectItemIndicators[i].enabled = i == weaponInventory.CurrentWeaponIndex;
    }
  }
}
