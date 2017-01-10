using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleSelecter
{
    static string[] roleArray = { "villager", "villager", "fortuneTeller", "werewolf", "villager", "villager", "shaman", "psyco", "werewolf", "hunter", "villager", "villager", "werewolf", "villager", "villager", "villager", "villager", "villager", "villager", "werewolf" };
    public static Dictionary<int, Participant> Run(PhotonPlayer[] players)
    {
        //人数に応じた役職
        List<string> roleList = new List<string>();
        for (int i = 0; i < players.Length; i++)
        {
            roleList.Add(roleArray[i]);
        }
        //シャッフル
        for(int i= 0; i < roleList.Count; i++)
        {
            string temp = roleList[i];
            int randomIndex = Random.Range(0, roleList.Count);
            roleList[i] = roleList[randomIndex];
            roleList[randomIndex] = temp;
        }
        //役職振り
        Dictionary<int, Participant> participants = new Dictionary<int, Participant>();
        for (int i = 0; i < players.Length; i++)
        {
            participants.Add(players[i].ID, new Participant(players[i].ID, roleList[i]));
        }
        return participants;
    }

}
