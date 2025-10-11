using UnityEngine;

public class CameraPositions : MonoBehaviour
{
    [Header("Stage Camera Positions")]
    [SerializeField] private Vector2[] cameraPositions;

    public Vector2[] GetStageCameraPositions()
    {
        return cameraPositions;
    }
}
