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

        if (other.TryGetComponent<PlayerController>(out pc))
        {
            if (trackCheckpoints.PlayerThroughCheckpoint(this))
                Hide();
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

}

