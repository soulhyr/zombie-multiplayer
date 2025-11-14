using System;
using Photon.Pun;
using UnityEngine;

public class Woman : MonoBehaviourPun
{
    private PhotonView view;
    private Animator ani;
    private float speed = 3f;

    void Awake()
    {
        view = GetComponent<PhotonView>();
        ani = GetComponent<Animator>();
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
                ani.SetInteger("state", 1);
            }
            else
            {
                ani.SetInteger("state", 0);
            }
        }
    }
}