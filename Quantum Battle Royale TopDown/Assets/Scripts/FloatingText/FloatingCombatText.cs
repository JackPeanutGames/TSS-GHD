using Quantum;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCombatText : MonoBehaviour
{
  [Header("References")]
  public Canvas Canvas;
  public RectTransform CanvasTransform;
  public FloatingCombatTextEntry EntryPrefab;

  private Queue<FloatingCombatTextEntry> _entryPool = new Queue<FloatingCombatTextEntry>();
  private GameManager _gameManager;

  private void Start()
  {
    SpawnEntries(16);
    QuantumEvent.Subscribe<EventCharacterDamaged>(this, OnCharacterDamaged);
    _gameManager = FindObjectOfType<GameManager>();
  }

  private void OnDestroy()
  {
    QuantumEvent.UnsubscribeListener(this);
  }

  private unsafe void OnCharacterDamaged(EventCharacterDamaged eventData)
  {
    Frame frame = _gameManager.Game.Frames.Verified;
    if (frame.Exists(eventData.character) == false)
    {
      return;
    }
    if (_entryPool.Count == 0)
    {
      SpawnEntries(16);
    }

    var entry = _entryPool.Dequeue();
    var characterPosition = frame.Get<Transform2D>(eventData.character).Position;

    Vector3 viewportPos = Camera.main.WorldToViewportPoint(characterPosition.ToUnityVector3());

    // If visible, update the position
    float width = CanvasTransform.sizeDelta.x;
    float height = CanvasTransform.sizeDelta.y;
    var pos = new Vector3(width * viewportPos.x - width / 2, height * viewportPos.y - height / 2);

    viewportPos = pos;
    viewportPos.x = Mathf.FloorToInt(viewportPos.x);
    viewportPos.y = Mathf.FloorToInt(viewportPos.y);
    viewportPos.z = 0f;

    entry.RectTransform.anchoredPosition = viewportPos;
    entry.Activate(eventData.damage.AsInt, Vector2.up);
  }

  private void SpawnEntries(int amount)
  {
    for (int i = 0; i < amount; i++)
    {
      var entry = Instantiate(EntryPrefab, transform);
      entry.onAnimationFinished += ReturnObjectToPool;
      entry.Deactivate();
    }
  }

  private void ReturnObjectToPool(FloatingCombatTextEntry entry)
  {
    _entryPool.Enqueue(entry);
  }
}