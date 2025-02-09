using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Message : MonoBehaviour
{
  public TextMeshProUGUI HeaderText;
  public TextMeshProUGUI MessageText;

  public void Init(MessageData data)
  {
    HeaderText.text = data.Header;
    MessageText.text = data.Message;
  }

  // Update is called once per frame
  void Update()
  {

  }
}
