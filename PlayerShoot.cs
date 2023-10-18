using UnityEngine;
using Mirror;

public class PlayerShoot : NetworkBehaviour
{

    public PlayerWeapon weapon;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    void Start()
    {
        if (cam == null) {
            Debug.LogError("[Script error] {PlayerShoot.cs} No cam set, Script Disabled");
            this.enabled = false;
        }
    }

    private void Update() {
        if (Input.GetButtonDown("Fire1")) {
            Shoot();
        }
    }

    [Client]
    private void Shoot() {
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weapon.range, mask)) {
            if(hit.collider.tag == "Player") {
                CmdPlayerShot(hit.collider.name, weapon.damage);
            }
        }
    }

    [Command]
    private void CmdPlayerShot(string playerID, float damage) {
        Debug.Log("[Raycast log] {PlayerShoot.cs} " + playerID + " a été touché.");

        Player player = GameManager.GetPlayer(playerID);
        player.TakeDamage(damage);
    }
}
