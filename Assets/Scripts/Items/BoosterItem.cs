using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterItem : MonoBehaviour
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

    // Update is called once per frame
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
        // �ν�Ʈ������ 5��ŭ �߰�
        if (ps.BoostGauge < 10)
        {
            ps.BoostGauge += 5.0f;
            ps.BoostGauge = Mathf.Clamp(ps.BoostGauge, 0, 10);
        }
        // 15�� �Ŀ� �����
        yield return new WaitForSeconds(15);
        mr.enabled = true;
        collider.enabled = true;
    }
}
