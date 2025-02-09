using Quantum;
using UnityEngine;

public class GameEntityView : EntityView, IEntityView
{
  private GameManager _gameManager;

  public bool IsLocal => EntityRef == _gameManager.LocalView.EntityRef;

  public void Init(GameManager gameManager)
  {
    _gameManager = gameManager;
  }
}
