using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    public RoomInfo info;
  public void SetUP(RoomInfo _info) 
    
    {
        info = _info;
        text.text = _info.Name;
    }
    public void Onclick()
    {
        launcher.instance.Join_Room(info);
    }
}
