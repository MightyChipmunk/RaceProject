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
        // ��� ���� ����
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
        // �÷��̾�� ������ �������� �ݶ��̴��� ��Ȱ��ȭ
        mr.enabled = false;
        collider.enabled = false;
        // TODO
        // 15�� �Ŀ� �����
        yield return new WaitForSeconds(15);
        mr.enabled = true;
        collider.enabled = true;
    }
}
