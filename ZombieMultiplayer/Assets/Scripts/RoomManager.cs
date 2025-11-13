using System.Linq;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    public GameObject woman1;
    public GameObject woman2;
    public Button btnStart;
    public Button btnReady;
    public TMP_Text nickname1;
    public TMP_Text nickname2;

    private bool isReady;
    
    void Start()
    {
        Init();
    }

    private void Init()
    {
        SetButton(btnStart, Pun2Manager.Instance.IsMasterClient);
        SetButton(btnStart, !Pun2Manager.Instance.IsMasterClient);
        btnStart.interactable = false;
        btnStart.gameObject.SetActive(Pun2Manager.Instance.IsMasterClient);
        btnReady.gameObject.SetActive(!Pun2Manager.Instance.IsMasterClient);

        if (Pun2Manager.Instance.IsMasterClient)
        {
            woman1.SetActive(true);
            nickname1.text = Pun2Manager.Instance.NickName;
        }
        else
        {
            woman2.SetActive(true);
            nickname2.text = Pun2Manager.Instance.NickName;
        }
    }

    private void AddEvents()
    {
        btnStart.onClick.AddListener(() =>
        {
            Pun2Manager.Instance.LoadScene("Main");
        });
        
        btnReady.onClick.AddListener(() =>
        {
            btnReady.GetComponentInChildren<TMP_Text>().text = isReady ? "UnReady" : "Ready";
            SetButton(btnStart,isReady);
        });
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
