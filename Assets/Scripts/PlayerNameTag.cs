using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class PlayerNameTag : MonoBehaviourPun
{
    [SerializeField] TextMeshProUGUI nameText;
    // Start is called before the first frame update
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
