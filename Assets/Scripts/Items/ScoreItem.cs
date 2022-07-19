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
        // ��� ���� ����
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
        // �÷��̾�� ������ �������� �ݶ��̴��� ��Ȱ��ȭ
        mr.enabled = false;
        collider.enabled = false;
        // ���� ����
        if (!pc.Is2P)
            GameManager.Instance.Score += 1000;
        else if (pc.Is2P)
            GameManager_Multi.Instance.Score += 1000;
        // 15�� �Ŀ� �����
        yield return new WaitForSeconds(15);
        mr.enabled = true;
        collider.enabled = true;
    }
}
