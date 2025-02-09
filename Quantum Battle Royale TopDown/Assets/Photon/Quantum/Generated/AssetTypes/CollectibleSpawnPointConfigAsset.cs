// <auto-generated>
// This code was auto-generated by a tool, every time
// the tool executes this code will be reset.
//
// If you need to extend the classes generated to add
// fields or methods to them, please create partial  
// declarations in another file.
// </auto-generated>

using Quantum;
using UnityEngine;

[CreateAssetMenu(menuName = "Quantum/CollectibleSpawnPointConfig", order = Quantum.EditorDefines.AssetMenuPriorityStart + 52)]
public partial class CollectibleSpawnPointConfigAsset : AssetBase {
  public Quantum.CollectibleSpawnPointConfig Settings;

  public override Quantum.AssetObject AssetObject => Settings;
  
  public override void Reset() {
    if (Settings == null) {
      Settings = new Quantum.CollectibleSpawnPointConfig();
    }
    base.Reset();
  }
}

public static partial class CollectibleSpawnPointConfigAssetExts {
  public static CollectibleSpawnPointConfigAsset GetUnityAsset(this CollectibleSpawnPointConfig data) {
    return data == null ? null : UnityDB.FindAsset<CollectibleSpawnPointConfigAsset>(data);
  }
}
