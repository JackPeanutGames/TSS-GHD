using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityViewEvents : MonoBehaviour
{
  public GameObject InstantiateVFXPrefab;
  public GameObject DestroyVFXPrefab;

  public void OnInstantite()
  {
    if (InstantiateVFXPrefab != null)
    {
      Instantiate(InstantiateVFXPrefab, transform.position, Quaternion.identity);
    }
  }

  public void OnDestroyEntity()
  {
    if (DestroyVFXPrefab != null)
    {
      Instantiate(DestroyVFXPrefab, transform.position, Quaternion.identity);
    }
  }
}
