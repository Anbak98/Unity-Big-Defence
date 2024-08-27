using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class CursorPosition
{
    [ReadOnly][SerializeField] private Vector3 _posOnDevice;
    [ReadOnly][SerializeField] private Vector3 _posOnScene;
    [ReadOnly][SerializeField] private Vector3 _tilePosOnMouse;

    public void CaculatePosAccordingToRule()
    {
        _posOnDevice = Input.mousePosition;
        _posOnDevice.z = -Camera.main.transform.position.z;

        _posOnScene = Camera.main.ScreenToWorldPoint(_posOnDevice);
    }

    public Vector3 posOnDevice
    {
        get
        {
            CaculatePosAccordingToRule();
            return _posOnDevice;
        }
    }

    public Vector3 posOnScene
    {
        get
        {
            CaculatePosAccordingToRule();
            return _posOnScene;
        }
    }

    public Vector3 tilePosOnMouse
    {
        get
        {
            CaculatePosAccordingToRule();
            _tilePosOnMouse.x = (int)Math.Round(posOnScene.x);
            _tilePosOnMouse.y = (int)Math.Round(posOnScene.y);
            return _tilePosOnMouse;
        }
    }
}
