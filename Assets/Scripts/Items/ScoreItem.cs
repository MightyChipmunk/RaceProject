using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreItem : MonoBehaviour
{
    public float rotSpeed = 30.0f;
    PlayerStat ps;

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
        if (other.TryGetComponent<PlayerStat>(out ps))
        {
            StartCoroutine("Disable");
        }
    }

    IEnumerator Disable()
    {
        // 플레이어와 닿으면 렌더러와 콜라이더를 비활성화
        mr.enabled = false;
        collider.enabled = false;
        // TODO
        // 15초 후에 재생성
        yield return new WaitForSeconds(15);
        mr.enabled = true;
        collider.enabled = true;
    }
}
