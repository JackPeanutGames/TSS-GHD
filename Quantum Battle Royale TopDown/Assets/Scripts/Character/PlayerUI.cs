using TMPro;
using Quantum;
using UnityEngine;
using Photon.Realtime;
using Photon.Deterministic;

public unsafe class PlayerUI : QuantumCallbacks
{

  [Header("References")]
  public SpriteRenderer Health;
  public SpriteRenderer WeaponAmmo;
  public SpriteRenderer WeaponFireRate;

  public TMP_Text PlayerName;

  [Header("Configurations")]
  public Color EnemyColor;
  public Color WeaponColor;

  private GameManager _gameManager;
  private EntityView _entityView;

  [SerializeField]
  private Quaternion _initialRotation;
  [SerializeField]
  private Vector3 _relativePosition;

  protected override void OnEnable()
  {
    base.OnEnable();
    _gameManager = FindObjectOfType<GameManager>();
    _entityView = GetComponentInParent<EntityView>();
  }

  private void Update()
  {
    transform.rotation = _initialRotation;
    transform.position = transform.parent.transform.position + _relativePosition;
  }


  private void Start()
  {
    var f = _gameManager.Game.Frames.Predicted;
    PlayerCharacter character = f.Get<PlayerCharacter>(_entityView.EntityRef);
    PlayerRef player = f.Get<PlayerLink>(character.Owner).Player;
    PlayerName.text = f.GetPlayerData(player).PlayerName;

    if (_gameManager.Game.PlayerIsLocal(player) == false)
    {
      Health.color = EnemyColor;
      PlayerName.color = EnemyColor;
    }
  }

  public override void OnUpdateView(QuantumGame game)
  {
    //var f = _gameManager.Game.Frames.Predicted;

    //var weaponInventory = f.Get<WeaponInventory>(_entityView.EntityRef);
    //var currentWeapon = weaponInventory.Weapons[weaponInventory.CurrentWeaponIndex];
    //var weaponData = f.FindAsset<WeaponConfig>(currentWeapon.Config.Id);

    //var health = f.Get<Health>(_entityView.EntityRef);

    //var healthRatio = (health.CurrentValue / health.MaxValue).AsFloat;
    //var ammoRatio = (float)currentWeapon.CurrentAmmo / weaponData.MaxAmmo;

    //var fireRateRatio = Mathf.Clamp01(currentWeapon.FireRateTimer.AsFloat / (1 / weaponData.FireRate.AsFloat));

    //Health.size = new Vector2(healthRatio * 2, 0.25f);
    //WeaponAmmo.size = new Vector2(ammoRatio * 2, 0.25f);
    //WeaponFireRate.size = new Vector2((1 - fireRateRatio) * 2, 0.10f);

    //WeaponColor.a = currentWeapon.IsRecharging ? 0.5f : 1;
    //WeaponAmmo.color = WeaponColor;
  }
}