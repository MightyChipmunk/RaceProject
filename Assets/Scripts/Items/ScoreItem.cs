using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreItem : MonoBehaviour
{
    public float rotSpeed = 30.0f;
    PlayerController pc;

    MeshRenderer mr;
    BoxCollider collider;

    private void Start()
    {
        gameObject.SetActive(true);
        mr = GetComponent<MeshRenderer>();
        collider = GetComponent<BoxCollider>();
    }
    void Update()
    {
        // 계속 빙빙 돌게
        transform.eulerAngles += rotSpeed * Time.deltaTime * Vector3.up;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out pc))
        {
            StartCoroutine("Disable");
        }
    }

    IEnumerator Disable()
    {
        // 플레이어와 닿으면 렌더러와 콜라이더를 비활성화
        mr.enabled = false;
        collider.enabled = false;
        // 점수 증가
        if (!pc.Is2P)
            GameManager.Instance.Score += 1000;
        else if (pc.Is2P)
            GameManager_Multi.Instance.Score += 1000;
        // 15초 후에 재생성
        yield return new WaitForSeconds(15);
        mr.enabled = true;
        collider.enabled = true;
    }
}
