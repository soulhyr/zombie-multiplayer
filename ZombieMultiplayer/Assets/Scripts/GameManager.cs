using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start() => CreateCharacter();

    private void CreateCharacter()
    {
        var initPos = Random.insideUnitSphere * 5f;
        initPos.y = 0;
        PhotonNetwork.Instantiate("Woman", initPos, Quaternion.identity);
    }
}