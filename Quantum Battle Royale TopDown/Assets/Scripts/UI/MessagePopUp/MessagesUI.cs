using Photon.Deterministic;
using System.Collections.Generic;
using UnityEngine;
using Quantum;
using System;

public struct MessageData
{
  public string Header;
  public string Message;
  public float Time;
}

public class MessagesUI : MonoBehaviour
{
  // Start is called before the first frame update

  private List<MessageData> Messages = new List<MessageData>();
  private List<Message> MessageBoxes = new List<Message>();
  private int LastMessageCount = 0;
  public Message MessagePrefab;
  public float MessageTimeout = 3;
  public float MessageOffsetY = -60;
  void Start()
  {
    QuantumEvent.Subscribe<EventMessage>(this, ProcessMessageEvent);
  }

  private void OnDestroy()
  {
    QuantumEvent.UnsubscribeListener(this);
  }

  private void ProcessMessageEvent(EventMessage e)
  {
    AddMesssage(e.header, e.text);
  }

  public void AddMesssage(string header, string message)
  {
    Messages.Add(new MessageData
    {
      Header = header,
      Message = message,
      Time = Time.time
    });
  }
  // Update is called once per frame
  void Update()
  {
    var newMessages = Messages.Count - LastMessageCount;
    if (newMessages > 0)
    {
      for (int i = LastMessageCount; i < Messages.Count; i++)
      {
        var data = Messages[i];
        var ui = Instantiate(MessagePrefab, transform);
        ui.Init(data);
        MessageBoxes.Add(ui);
      }
    }

    // removing old messages
    for (int i = 0; i < Messages.Count; i++)
    {
      var message = Messages[i];
      var age = Time.time - message.Time;
      var messageIsNew = age < MessageTimeout;
      if (messageIsNew == false)
      {
        Messages.RemoveAt(i);
        var t = MessageBoxes[i];
        MessageBoxes.RemoveAt(i);
        Destroy(t.gameObject);
      }
      else
      {
        break;
      }
    }

    for (int i = Messages.Count - 1; i >= 0; i--)
    {
      var t = MessageBoxes[i].GetComponent<RectTransform>();
      t.anchoredPosition = new Vector2(0, MessageOffsetY * i);
    }

    LastMessageCount = Messages.Count;
  }
}
