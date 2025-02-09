using Quantum;
using UnityEngine;

public class CharacterAnimations : QuantumCallbacks
{
  private EntityView _entityView;
  private Animator _animator;

  private GameManager _gameManager;

  private void Awake()
  {
    _animator = GetComponentInChildren<Animator>();

    _entityView = GetComponent<EntityView>();
    _gameManager = FindObjectOfType<GameManager>();
  }

  public override void OnUpdateView(QuantumGame game)
  {
    Frame frame = game.Frames.Predicted;
    if(_entityView == null || frame.Exists(_entityView.EntityRef) == false)
    {
      return;
    }

    float speed = frame.Get<KCC>(_entityView.EntityRef).Velocity.Magnitude.AsFloat;
    if (speed == 0)
    {
      _animator.SetBool("StopRun", true);
    }
    else
    {
      _animator.SetBool("StopRun", false);

    }
    _animator.SetFloat("Speed", speed);
  }
}
