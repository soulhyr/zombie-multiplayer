using System.Linq;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReadyLobbyManager : MonoBehaviour
{
    public GameObject woman1;
    public GameObject woman2;
    public GameObject nickNameArea;
    public TMP_InputField txtNickName;
    public Button btnEdit;
    public Button btnStart;
    public Button btnReady;

    private PhotonView view;
    
    void Start()
    {
        SetButton(btnStart, PhotonNetwork.IsMasterClient);
        SetButton(btnStart, !PhotonNetwork.IsMasterClient);
        
        btnStart.gameObject.SetActive(PhotonNetwork.IsMasterClient);
        btnReady.gameObject.SetActive(!PhotonNetwork.IsMasterClient);

        btnEdit.onClick.AddListener(() =>
        {
            btnEdit.interactable = false;
            PhotonNetwork.NickName = txtNickName.text;
            nickNameArea.SetActive(false);
            
            if (PhotonNetwork.IsMasterClient)
            {
                SetPlayer(PhotonNetwork.NickName);
                SetReady(PhotonNetwork.NickName);
                Debug.Log($"NickName:{PhotonNetwork.NickName}");
                btnStart.gameObject.SetActive(true);
                btnStart.onClick.AddListener(() =>
                {
                    PhotonNetwork.LoadLevel("ReadyLobby");
                });
                SetButton(btnStart, false);
            }
            else
            {
                view.RPC("SetPlayer", RpcTarget.MasterClient, PhotonNetwork.NickName);
                btnReady.gameObject.SetActive(true);
                btnReady.onClick.AddListener(() =>
                {
                    view.RPC("SetReady", RpcTarget.MasterClient, PhotonNetwork.NickName);
                });
            }
        });

        if (PhotonNetwork.IsMasterClient)
        {
            woman1.SetActive(true);
        }
        else
        {
            woman2.SetActive(true);
        }
    }

    private void SetButton(Button btn, bool isActive)
    {
        if (isActive)
        {
            btn.image.color = new Color32(112, 188, 255, 255);
            btn.interactable = true;
        }
        else
        {
            btn.image.color = new Color32(112, 188, 255, 255);
            btn.interactable = false;
        }

        btn.gameObject.SetActive(true);
    }
    
    [PunRPC]
    void SetPlayer(string nickName)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            DataManager.Instance.roomMemberInfos.Add(new RoomMemberInfo(nickName, false));
            Debug.Log("Player Joined!");
        }
    }
    
    [PunRPC]
    void SetReady(string nickName)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            RoomMemberInfo found = DataManager.Instance.roomMemberInfos.Find(x => x.nickName == nickName);
            found.isReady = true;
            Debug.Log("Player Ready!");
            SetButton(btnStart, true);
            int count = DataManager.Instance.roomMemberInfos.Count(x => !x.isReady);
            if (count == 0)
            {
                SetButton(btnStart, true);
            }
        }
    }
}
