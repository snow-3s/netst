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
            Debug.Log(role);
            //Init Photon Audio Group 
            if (role.Equals("werewolf"))
            {
                PhotonVoiceNetwork.Client.ChangeAudioGroups(new byte[] { }, new byte[] { 0,2 });
            }
            else
            {
                PhotonVoiceNetwork.Client.ChangeAudioGroups(new byte[] { }, new byte[] { 0 });
            }
            PhotonVoiceNetwork.Client.GlobalAudioGroup = 0;
        }
    }

}