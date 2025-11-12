using System;
using Photon.Pun;
using UnityEngine;

public class Woman : MonoBehaviourPun
{
    private PhotonView view;
    private float speed = 1f;

    void Awake()
    {
        view = GetComponent<PhotonView>();
    }
    
    void Update()
    {
        if (view.IsMine)
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            var dir = new Vector3(horizontal, 0, vertical);
            if (dir != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(dir);
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
        }
    }
}