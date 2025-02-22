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

[CreateAssetMenu(menuName = "Quantum/PowerUpConfig/AttackPowerUpConfig", order = Quantum.EditorDefines.AssetMenuPriorityStart + 390)]
public partial class AttackPowerUpConfigAsset : PowerUpConfigAsset {
  public Quantum.AttackPowerUpConfig Settings;

  public override Quantum.AssetObject AssetObject => Settings;
  
  public override void Reset() {
    if (Settings == null) {
      Settings = new Quantum.AttackPowerUpConfig();
    }
    base.Reset();
  }
}

public static partial class AttackPowerUpConfigAssetExts {
  public static AttackPowerUpConfigAsset GetUnityAsset(this AttackPowerUpConfig data) {
    return data == null ? null : UnityDB.FindAsset<AttackPowerUpConfigAsset>(data);
  }
}
