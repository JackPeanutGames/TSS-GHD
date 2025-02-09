using Quantum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeWeaponInput : MonoBehaviour
{
  private byte _lastWeaponSelected;
  private GameManager _gameManager;
  private PlayerInput _playerInput;


  private void Start()
  {
    _playerInput = GetComponent<PlayerInput>();
    _gameManager = FindObjectOfType<GameManager>();
  }


  public void SetWeaponSelected(byte lastSelected)
  {
    _lastWeaponSelected = lastSelected;
  }
  public byte GetWaeponIndexInput()
  {
    var temp = _lastWeaponSelected;
    _lastWeaponSelected = byte.MaxValue;
    return temp;
  }

  private void Update()
  {
    if (QuantumRunner.Default == null || QuantumRunner.Default.Game == null)
      return;

    Frame frame = QuantumRunner.Default.Game.Frames.Predicted;
    if (frame == null)
      return;

    if (_gameManager.LocalView == null || frame.Exists(_gameManager.LocalView.EntityRef) == false)
      return;

    UpdateScrollInput(frame);
    CheckChangeWeaponButton(frame);
  }

  private void SetNextWeaponIndex(Frame frame, WeaponInventory inventory, bool IsWeaponUp)
  {
    int currentWeaponIndex = inventory.CurrentWeaponIndex;
    do
    {
      currentWeaponIndex += IsWeaponUp ? 1 : -1;
      if (currentWeaponIndex > Constants.INVENTORY_SIZE - 1)
      {
        currentWeaponIndex = 0;
      }
      if (currentWeaponIndex < 0)
      {
        currentWeaponIndex = Constants.INVENTORY_SIZE - 1;
      }

    } while (inventory.Weapons[currentWeaponIndex].Config == null);
    SetWeaponSelected((byte)currentWeaponIndex);
  }

  private void CheckChangeWeaponButton(Frame frame)
  {
    WeaponInventory inventory = frame.Get<WeaponInventory>(_gameManager.LocalView.EntityRef);

    if (_playerInput.actions["ChangeWeaponUp"].WasPerformedThisFrame())
    {
      SetNextWeaponIndex(frame, inventory, true);
    }
    if (_playerInput.actions["ChangeWeaponDown"].WasPerformedThisFrame())
    {
      SetNextWeaponIndex(frame, inventory, false);
    }
  }


  private void UpdateScrollInput(Frame frame)
  {
    WeaponInventory inventory = frame.Get<WeaponInventory>(_gameManager.LocalView.EntityRef);

    float scrollValue = _playerInput.actions["MouseScroll"].ReadValue<float>();
    if (scrollValue == 0 || _lastWeaponSelected != byte.MaxValue)
    {
      return;
    }

    if (scrollValue > 0)
    {
      SetNextWeaponIndex(frame, inventory, true);
    }
    else if (scrollValue < 0)
    {
      SetNextWeaponIndex(frame, inventory, false);
    }
  }
}
