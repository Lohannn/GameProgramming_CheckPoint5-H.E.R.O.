using Unity.VisualScripting;
using UnityEngine;

public class CameraPositionManager : MonoBehaviour
{
    [Header("Camera Position Manager Settings")]
    [SerializeField] private int cameraTogglerNumber;

    private Camera cam;
    private Vector2[] camPositions;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Start()
    {
        if (GameObject.FindGameObjectWithTag("CameraManager") != null)
        {
            camPositions = GameObject.FindGameObjectWithTag("CameraManager").GetComponent<CameraPositions>().GetStageCameraPositions();
        }
    }

    public void ChangeCameraPosition()
    {
        if (cam == null) return;

        if ((Vector2)cam.transform.position != camPositions[cameraTogglerNumber])
        {
            cam.transform.position = new Vector3(camPositions[cameraTogglerNumber].x, camPositions[cameraTogglerNumber].y, -10);
        }
    }
}
