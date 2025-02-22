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

[CreateAssetMenu(menuName = "Quantum/CollectibleConfig/WeaponCollectibleConfig", order = Quantum.EditorDefines.AssetMenuPriorityStart + 74)]
public partial class WeaponCollectibleConfigAsset : CollectibleConfigAsset {
  public Quantum.WeaponCollectibleConfig Settings;

  public override Quantum.AssetObject AssetObject => Settings;
  
  public override void Reset() {
    if (Settings == null) {
      Settings = new Quantum.WeaponCollectibleConfig();
    }
    base.Reset();
  }
}

public static partial class WeaponCollectibleConfigAssetExts {
  public static WeaponCollectibleConfigAsset GetUnityAsset(this WeaponCollectibleConfig data) {
    return data == null ? null : UnityDB.FindAsset<WeaponCollectibleConfigAsset>(data);
  }
}
