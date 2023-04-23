using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController: MonoBehaviour
{
    [SerializeField] private Transform[] playerTransforms;

    [SerializeField] private float cameraHeight;

    private delegate void CameraDisplayFunction();
    private CameraDisplayFunction _currentDisplayFunction;

    private Transform _cameraTransform;
    private Transform CameraTransform
    {
        get
        {
            if(_cameraTransform == null)
            {
                _cameraTransform = Camera.main.transform;
            }
            if (_cameraTransform == null)
            {
                //no main camera in scene, set to this to prevent errors
                _cameraTransform = this.transform;
                Debug.LogWarning("No camera in scene. Make sure there is a camera in the scene and that it is tagged to main camera");
            }
            return _cameraTransform;
        }
    }


    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        ChangeCameraMode(CameraModes.SinglePlayer);
    }

    private void Update()
    {
        if (_currentDisplayFunction != null)
        {
            _currentDisplayFunction.Invoke();
        }
    }


    /// <summary>
    /// Call from player manager and pass in player transforms as an array. order dosen't matter
    /// </summary>
    /// <param name="playerTransforms"> transforms of all current players after a player add/drop </param>
    public void UpdateCamera(Transform[] playerTransforms)
    {
        if(playerTransforms != null)
        {
            if(playerTransforms.Length > 1)
            {
                ChangeCameraMode(CameraModes.Multiplayer);
                return;
            }
            else if(playerTransforms.Length == 1)
            {
                ChangeCameraMode(CameraModes.SinglePlayer);
            }
        }
    }

    //changes delegate for camera mode
    private void ChangeCameraMode(CameraModes mode)
    {
        switch (mode)
        {
            case CameraModes.SinglePlayer:
                _currentDisplayFunction = SingleplayerCameraDisplayMode;
                break;
            case CameraModes.Multiplayer:
                _currentDisplayFunction = MultiplayerCameraDisplayMode;
                break;
            default:
                _currentDisplayFunction = SingleplayerCameraDisplayMode;
                break;
        }
    }


    //function for displaying game when there is more than one player
    private void MultiplayerCameraDisplayMode()
    {
        if (playerTransforms != null)
        {
            Vector3 camAvreagePosition;

            float x = 0;
            float z = 0;

            for (int i = 0; i < playerTransforms.Length; i++)
            {
                x += playerTransforms[i].position.x;
                z += playerTransforms[i].position.z;
            }

            x /= playerTransforms.Length;
            z /= playerTransforms.Length;

            float distanceMagnitude = 0;
            for (int i = 0; i < playerTransforms.Length; i++)
            {
                Vector3 playerDistance = playerTransforms[i].position;
                playerDistance.y = CameraTransform.position.y; //ignores Y distance

                distanceMagnitude += (Vector3.Distance(playerDistance, CameraTransform.position));
            }
            distanceMagnitude = Mathf.Max(distanceMagnitude, 10); //prevents the camera from getting to zoomed in


            camAvreagePosition = new Vector3(x, distanceMagnitude, z);
            CameraTransform.position = camAvreagePosition;
        }
    }

    //simplified function for displaying game when there is only one player
    private void SingleplayerCameraDisplayMode()
    {
        if(playerTransforms != null)
        {
            CameraTransform.position = playerTransforms[0].position + (Vector3.up * cameraHeight);
        }
    }

}
