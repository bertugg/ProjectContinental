using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraManager : MonoBehaviour
{
    public Volume deathVolume;

    protected void Awake() =>
        GameManager.Instance.mainCamera = this;

    public void SwitchToDeathCamera()
    {
        DOVirtual.Float(0, 1, 0.5f, value => { deathVolume.weight = value; });
    }
}
