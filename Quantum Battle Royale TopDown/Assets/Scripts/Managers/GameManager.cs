using Quantum;
using System.Collections;
using UnityEngine;

public class GameManager : QuantumCallbacks
{
  public QuantumGame Game => QuantumRunner.Default.Game;

  private EntityView _localView = null;
  public EntityView LocalView { get { return _localView; } }

  private EntityViewsManager _viewsManager;
  public EntityViewsManager ViewsManager { get { return _viewsManager; } }

  protected override void OnEnable()
  {
    base.OnEnable();

    _viewsManager = FindObjectOfType<EntityViewsManager>();
    _viewsManager.Init(this);
  }

	private IEnumerator Start()
	{
		yield return new WaitForSeconds(2);

		if(_localView == null)
		{
			var allViews = FindObjectsOfType<GameEntityView>();
			int randomId = Random.Range(0, allViews.Length);
			_localView = allViews[randomId];
		}
	}

  public void OnEntityViewCreated(EntityView entityView)
  {
    QuantumGame game = QuantumRunner.Default.Game;
    Frame frame = game.Frames.Predicted;

    if (frame.Has<PlayerCharacter>(entityView.EntityRef) == false)
    {
      return;
    }

    if(entityView is GameEntityView gameEntityView)
    {
      gameEntityView.Init(this);
    }
    
    PlayerCharacter character = frame.Get<PlayerCharacter>(entityView.EntityRef);
    PlayerLink playerLink = frame.Get<PlayerLink>(character.Owner);
    if (_localView == null && game.PlayerIsLocal(playerLink.Player) == true)
    {
      _localView = entityView;
    }
  }
}
