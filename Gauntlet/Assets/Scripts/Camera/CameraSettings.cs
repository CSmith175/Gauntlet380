using UnityEngine;

[CreateAssetMenu(fileName = "New Camera Settings", menuName = "CameraSettings")]
public class CameraSettings : ScriptableObject
{
    [Tooltip("Smooth damping applied to the camera. Higher values causes the camera to move like its in molases")] [Range(0, 0.15f)] public float _cameraDamping = 0.05f;
    [Tooltip("Minimum height from the camera to the players")] [Range(5, 25)] public int _cameraMinumumHeight = 10;
    [Tooltip("Additional height applied to the camera post adjustment. controls screen edge bounds. Higher values means more visible bounds but smaller players")] [Range(0, 10)] public int _cameraZoomOut = 3;
}
