using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSingle : MonoBehaviour
{
    private TrackCheckpoints trackCheckpoints;
    private MeshRenderer meshRenderer;
    private BoxCollider boxCollider;

    [SerializeField]
    bool isStart = false;

    PlayerController pc;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();

        GameManager.Instance.OnLapEnd += Show;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.TryGetComponent<PlayerController>(out pc) && !pc.Is2P)
        {
            if (trackCheckpoints.PlayerThroughCheckpoint(this))
            {
                Hide();
                if (!isStart)
                    GameManager.Instance.Score += 1500;
            }
        }
    }
    public void SetTrackCheckpoints(TrackCheckpoints trackCheckpoints)
    {
        this.trackCheckpoints = trackCheckpoints;
    }

    public void Show(object sender, System.EventArgs e)
    {
        if (!isStart)
            meshRenderer.enabled = true;
        boxCollider.enabled = true;
    }

    public void Hide()
    {
        meshRenderer.enabled = false;
        boxCollider.enabled = false;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnLapEnd -= Show;
    }
}

