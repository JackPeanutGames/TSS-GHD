using System;
using System.Collections.Generic;
using System.Linq;
using Quantum.Collections;

namespace Quantum
{
  public unsafe class GameFieldsSystem : SystemMainThread, ISignalOnWaitingPlayersEnd, ISignalOnSetupMatchEnd, ISignalOnPresentationEnd, ISignalOnPlayingEnd
  {

    public override void OnInit(Frame frame)
    {
      var game = frame.Unsafe.GetPointerSingleton<GameFields>();
      var gameSettings = frame.FindAsset<GameplaySettings>(game->GameplaySettings.Id);
      game->ChangeStateDelay = gameSettings.WaitForPlayersTime;
      game->State = GameState.WaitingPlayers;
    }

    public override void Update(Frame frame)
    {
      var game = frame.Unsafe.GetPointerSingleton<GameFields>();
      game->ChangeStateDelay -= frame.DeltaTime;
      switch (game->State)
      {
        case GameState.WaitingPlayers:
          {
            if (game->ChangeStateDelay <= 0)
            {
              frame.Signals.OnWaitingPlayersEnd(game);
            }
            break;
          }
        case GameState.SetupMatch:
          {
            if (game->ChangeStateDelay <= 0)
            {
              frame.Signals.OnSetupMatchEnd(game);
            }
            break;
          }
        case GameState.Presentation:
          {
            if (game->ChangeStateDelay <= 0)
            {
              frame.Signals.OnPresentationEnd(game);
            }
            break;
          }
        case GameState.Playing:
          {
            if (EndGameCondition(frame))
            {
              frame.Signals.OnPlayingEnd(game);
            }
            break;
          }
        case GameState.End:
          {
            break;
          }
        default:
          break;
      }
    }

    private bool EndGameCondition(Frame frame)
    {
      int characterCount = frame.ComponentCount<PlayerCharacter>();
      return characterCount == 1;
    }

    public void OnWaitingPlayersEnd(Frame frame, GameFields* game)
    {
      var gameSettings = frame.FindAsset<GameplaySettings>(game->GameplaySettings.Id);
      game->ChangeStateDelay = gameSettings.SetupMatchTime;
      game->State = GameState.SetupMatch;
    }

    public void OnSetupMatchEnd(Frame frame, GameFields* game)
    {
      var gameSettings = frame.FindAsset<GameplaySettings>(game->GameplaySettings.Id);
      game->ChangeStateDelay = gameSettings.PresentationTime;
      game->State = GameState.Presentation;

      frame.Events.OnSetupMatchEnd();
    }

    public void OnPresentationEnd(Frame frame, GameFields* game)
    {
      game->State = GameState.Playing;
    }

    public void OnPlayingEnd(Frame frame, GameFields* game)
    {
      game->State = GameState.End;
      frame.Events.OnGameFinish(*game);

      frame.SystemDisable<MovementSystem>();
      frame.SystemDisable<WeaponSystem>();
    }
  }
}