using UnityEngine;
using Mirror;

public class PlayerSetup : NetworkBehaviour{

    [SerializeField]
    Behaviour[] componentsToDisble;

    [SerializeField]
    private string remoteLayerName = "Layer.Player.Remote";

    Camera sceneCamera;

    private void Start() {
        
        if(!isLocalPlayer) {
            DisableComponents();
            AssignRemoteLayer();
        } 
        else {
            sceneCamera = Camera.main;
            if(sceneCamera != null) {
                sceneCamera.gameObject.SetActive(false);
            }
        }

        
    }

    public override void OnStartClient() {
        base.OnStartClient();

        string netId = GetComponent<NetworkIdentity>().netId.ToString();
        Player player = GetComponent<Player>();

        GameManager.RegisterPlayer(netId, player);
    }

    private void AssignRemoteLayer() {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    private void DisableComponents() {
        for (int i = 0; i < componentsToDisble.Length; i++) {
            componentsToDisble[i].enabled = false;
        }
    }

    private void OnDisable() {
        if(sceneCamera != null) {
            sceneCamera.gameObject.SetActive(true);
        }

        GameManager.UnregisterPlayer(transform.name);
    }
}
