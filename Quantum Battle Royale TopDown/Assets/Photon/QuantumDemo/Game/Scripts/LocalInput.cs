using System;
using Photon.Deterministic;
using Quantum;
using UnityEngine;
using UnityEngine.InputSystem;


public class LocalInput : MonoBehaviour
{

  private PlayerInput _playerInput;
  private GameManager _gameManager;
  private ChangeWeaponInput _changeWeaponInput;


  private void OnEnable()
  {
    QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback));
    _playerInput = GetComponent<PlayerInput>();
    _gameManager = FindObjectOfType<GameManager>();
    _changeWeaponInput = GetComponent<ChangeWeaponInput>();

  }

  public void PollInput(CallbackPollInput callback)
  {
    Quantum.Input input = new Quantum.Input();

    input.Fire = _playerInput.actions["Aim"].IsPressed() | _playerInput.actions["MouseFire"].IsPressed();
    input.SpecialSkill = _playerInput.actions["Special"].IsPressed() | _playerInput.actions["MouseSpecial"].IsPressed();

    FPVector2 direction = new FPVector2();
    if (input.Fire == true)
    {
      direction = _playerInput.actions["Aim"].ReadValue<Vector2>().ToFPVector2();
    }

    if (input.SpecialSkill == true)
    {
      direction = _playerInput.actions["SpecialAim"].ReadValue<Vector2>().ToFPVector2();
    }

    input.MovementDirection = _playerInput.actions["Move"].ReadValue<Vector2>().ToFPVector2();

    byte weaponIndex = _changeWeaponInput.GetWaeponIndexInput();
    if (weaponIndex != byte.MaxValue)
    {
      input.WeaponIndex = weaponIndex;
    }
    else
    {
      input.WeaponIndex = byte.MaxValue;
    }



#if UNITY_STANDALONE
    direction = GetDirectionToMouse();
#endif
    input.ActionDirection = direction;
    if (direction == FPVector2.Zero)
    {
      input.Fire = false;
      input.SpecialSkill = false;
    }

    callback.SetInput(input, DeterministicInputFlags.Repeatable);
  }



  private FPVector2 GetDirectionToMouse()
  {
    if (QuantumRunner.Default == null || QuantumRunner.Default.Game == null)
      return default;

    Frame frame = QuantumRunner.Default.Game.Frames.Predicted;
    if (frame == null)
      return default;

    if (_gameManager.LocalView == null)
      return default;

    FPVector2 localCharacterPosition = frame.Get<Transform2D>(_gameManager.LocalView.EntityRef).Position;

    Vector2 mousePosition = _playerInput.actions["Point"].ReadValue<Vector2>();
    Ray ray = Camera.main.ScreenPointToRay(mousePosition);
    UnityEngine.Plane plane = new UnityEngine.Plane(Vector3.up, Vector3.zero);

    if (plane.Raycast(ray, out var enter))
    {
      return (ray.GetPoint(enter).ToFPVector2() - localCharacterPosition);
    }

    return default;
  }
}
