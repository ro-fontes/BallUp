using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class PlayerNameTag : MonoBehaviourPun
{
    [SerializeField] TextMeshProUGUI nameText;

    void Start()
    {
        if(photonView.IsMine) { return; }
        SetName();
    }

    void SetName()
    {
        nameText.text = photonView.Owner.NickName;
    }
}
