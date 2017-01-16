using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParameter : Photon.MonoBehaviour{

    string role;

    public void SetRole(string role)
    {
        if (PhotonNetwork.player.ID == GetComponent<PhotonView>().ownerId)
        {
            this.role = role;
            //Notify Role
            GameObject prefabConfirmer = (GameObject)Resources.Load("Prefabs/Confirmer");
            GameObject confirmer = Instantiate(prefabConfirmer, new Vector3(), Quaternion.identity);
            confirmer.GetComponent<Confirmer>().SetText("Role: " + role);
            confirmer.GetComponent<Confirmer>().SetCallback(() => { Debug.Log(role); });            
        }
    }

}