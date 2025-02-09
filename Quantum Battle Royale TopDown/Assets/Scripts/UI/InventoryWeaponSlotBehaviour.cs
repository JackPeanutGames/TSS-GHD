using Quantum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryWeaponSlotBehaviour : QuantumCallbacks
{
  public int SlotIndex;
  [SerializeField]
  public Image _selectItemIndicators;
  [SerializeField]
  public Image _icon;
  [SerializeField]
  public TextMeshProUGUI _ammoCount;
  [SerializeField]
  private Image _reloadIndicator;
  private GameManager _gameManager;
  private ChangeWeaponInput _changeWeaponInput;


  private void Start()
  {
    _gameManager = FindObjectOfType<GameManager>();
    _changeWeaponInput = FindObjectOfType<ChangeWeaponInput>();

  }

  public void OnClick()
  {
    _changeWeaponInput.SetWeaponSelected((byte)SlotIndex);
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

    WeaponInventory weaponInventory = frame.Get<WeaponInventory>(_gameManager.LocalView.EntityRef);
    Weapon weapon = weaponInventory.Weapons[SlotIndex];

    UpdateSelectedItem(frame, weaponInventory, weapon);
    UpdateAmmoCount(frame, weapon);
    UpdateIcon(frame, weapon);
  }

  public void UpdateIcon(Frame frame, Weapon weapon)
  {
    WeaponConfig config = frame.FindAsset<WeaponConfig>(weapon.Config.Id);
    _icon.enabled = config != null;
  }

  private void UpdateAmmoCount(Frame frame, Weapon weapon)
  {
    WeaponConfig config = frame.FindAsset<WeaponConfig>(weapon.Config.Id);
    if (config == null)
    {
      _ammoCount.text = "";
      return;
    }
    if (config.InfiniteAmmo)
    {
      _ammoCount.text = "...";
      return;
    }
    int totalAmmo = (weapon.TotalAmmo + weapon.CurrentAmmo);
    _ammoCount.text = totalAmmo.ToString();
  }

  private void UpdateSelectedItem(Frame frame, WeaponInventory weaponInventory, Weapon weapon)
  {
    if (weapon.Config == null)
    {
      _selectItemIndicators.enabled = false;
      return;
    }
    _selectItemIndicators.enabled = SlotIndex == weaponInventory.CurrentWeaponIndex;
  }
}
