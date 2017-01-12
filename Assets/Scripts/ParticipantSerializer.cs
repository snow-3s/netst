using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using System.Text;
using System;

public static class ParticipantSerializer{

    public static void Register()
    {
        PhotonPeer.RegisterType(typeof(Participant), (byte)'C', SerializeParticipant, DeserialiedParticipant);
    }
    private static byte[] SerializeParticipant(object customobject)
    {
        Participant participant = (Participant)customobject;
        byte[] roleByte = Protocol.Serialize(participant.GetRole());
        byte[] aliveByte = Protocol.Serialize(participant.isSurvive());
        var bytes = new byte[sizeof(int)*2 + roleByte.Length + aliveByte.Length];
        int index = 0;
        Protocol.Serialize(participant.GetPlayerId(),bytes,ref index);
        Protocol.Serialize(roleByte.Length, bytes, ref index);
        Array.Copy(roleByte, 0, bytes, index, roleByte.Length);
        index += roleByte.Length;
        Array.Copy(aliveByte, 0, bytes, index, aliveByte.Length);
        index += aliveByte.Length;

        return bytes;
    }

    private static object DeserialiedParticipant(byte[] bytes)
    {
        int playerId;
        string role;
        bool alive;
        int roleByteSize;
        byte[] roleByte;
        byte[] aliveByte = new byte[1];
        int index = 0;
        Protocol.Deserialize(out playerId, bytes, ref index);
        Protocol.Deserialize(out roleByteSize, bytes, ref index);
        roleByte = new byte[roleByteSize];
        Array.Copy(bytes, index, roleByte, 0, roleByteSize);
        role = (string)Protocol.Deserialize(roleByte).ToString();
        index += roleByteSize;
        Array.Copy(bytes, index, aliveByte, 0, sizeof(bool));
        alive = (bool)Protocol.Deserialize(aliveByte);
        Participant participant = new Participant(playerId, role);
        return participant;
    }
}