using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public Rigidbody2D RB;
    public Animator AN;
    public SpriteRenderer SR;
    public PhotonView PV;
    public Text NickNameText;

    public float moveSpeed = 5f;

    private Vector3 curPos;
    private bool isMoving = false;

    void Awake()
    {
        NickNameText.text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
        NickNameText.color = PV.IsMine ? Color.green : Color.red;
    }

    void Update()
    {
        if (PV.IsMine)
        {
            HandleMovement();
        }
        else
        {
            SmoothSyncPosition();
        }
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        RB.velocity = new Vector2(moveInput * moveSpeed, RB.velocity.y);

        if (moveInput != 0)
        {
            isMoving = true;
            AN.SetBool("isWalking", true); // Walk 애니메이션 실행
            PV.RPC("FlipXRPC", RpcTarget.AllBuffered, moveInput);
        }
        else
        {
            isMoving = false;
            AN.SetBool("isWalking", false); // Idle 애니메이션 실행
        }
    }

    [PunRPC]
    void FlipXRPC(float moveInput)
    {
        SR.flipX = moveInput < 0;
    }

    void SmoothSyncPosition()
    {
        if ((transform.position - curPos).sqrMagnitude >= 100)
            transform.position = curPos;
        else
            transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(isMoving); // isMoving 정보를 전송
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();
            isMoving = (bool)stream.ReceiveNext(); // isMoving 정보를 수신
        }
    }
}