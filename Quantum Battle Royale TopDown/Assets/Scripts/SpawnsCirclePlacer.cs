using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpawnsCirclePlacer : MonoBehaviour
{
  public float Radius;
  public int NumberOfObjects;
  public GameObject Prefab;
  public bool DeleteOldObjects = false;

  public void PlaceObjects()
  {
    if (DeleteOldObjects)
    {
      for (int i = transform.childCount - 1; i >= 0; i--)
      {
        DestroyImmediate(transform.GetChild(i).gameObject);
      }
    }

    List<Vector3> positions = new List<Vector3>();

    float angleBetweenObjects = Mathf.PI * 2f / NumberOfObjects;

    for (int i = 0; i < NumberOfObjects; i++)
    {
      float angle = i * angleBetweenObjects;
      Vector3 position = new Vector3(Mathf.Cos(angle) * Radius, 0f, Mathf.Sin(angle) * Radius);

      positions.Add(position);
    }

    positions = ShuffleList(positions);

    for (int i = 0; i < NumberOfObjects; i++)
    {
      Vector3 position = positions[i];

      GameObject obj = Instantiate(Prefab, position, Quaternion.identity);
      obj.transform.parent = transform;

      obj.name = "SpawnPoint-" + i;
    }
  }

  private List<Vector3> ShuffleList(List<Vector3> list)
  {
    for (int i = 0; i < list.Count; i++)
    {
      Vector3 temp = list[i];
      int randomIndex = Random.Range(i, list.Count);
      list[i] = list[randomIndex];
      list[randomIndex] = temp;
    }

    return list;
  }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SpawnsCirclePlacer))]
public class CirclePlacerEditor : Editor
{

  public override void OnInspectorGUI()
  {
    base.OnInspectorGUI();

    SpawnsCirclePlacer placer = target as SpawnsCirclePlacer;

    if (GUILayout.Button("Place Spawn"))
    {
      placer.PlaceObjects();
    }
  }
}
#endif