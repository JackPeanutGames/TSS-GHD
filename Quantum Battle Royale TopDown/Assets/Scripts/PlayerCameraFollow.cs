using Photon.Deterministic;
using Quantum;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class PlayerCameraFollow : MonoBehaviour
{
  public float InterpolationSpeed = 1;
  public Vector3 BaseOffset;
  public float InputSpeed = 10;

  private GameManager _gameManager;

  private PlayerInput _playerInput;


  private void OnEnable()
  {
    _gameManager = FindObjectOfType<GameManager>();
    _playerInput = FindObjectOfType<PlayerInput>();

  }


  private void Update()
  {
    if (QuantumRunner.Default == null || _gameManager == null)
    {
      return;
    }

    if(_gameManager.LocalView == null)
    {
      UpdateCameraPositionByInput();
      return;
    }

    Vector3 targetPosition = _gameManager.LocalView.transform.position + BaseOffset;
    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * InterpolationSpeed);
  }

  private void UpdateCameraPositionByInput()
  {
    var direction = _playerInput.actions["Move"].ReadValue<Vector2>();
    transform.position += new Vector3(direction.x, 0, direction.y) * InputSpeed * Time.deltaTime;
  }
}
