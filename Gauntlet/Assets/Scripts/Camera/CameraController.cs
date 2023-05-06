using System.Collections.Generic;
using UnityEngine;

public class CameraController: MonoBehaviour
{
    #region "variables Set from Settings
    //set by the initilize function (Standin for a constructor due to Monobehavior quirks)
    private float _cameraMinHeight = 10;
    private float _cameraDamping = 0.05f;
    private float _cameraZoomPadding = 3;
    #endregion

    #region "Internal Variables"
    //player transforms
    private Transform[] _playerTransforms;

    //delegate for controlling single/multi player cameras
    private delegate void CameraDisplayFunction();
    private CameraDisplayFunction _currentDisplayFunction;

    //private property for getting the camera's transform easier
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

    //Vectors used to track camera velocity
    private Vector3 _cameraOldPos;
    private Vector3 _currentVelocity;
    #endregion


    //runs current camera mode every frame
    private void LateUpdate()
    {
        if (_currentDisplayFunction != null)
        {
            _currentDisplayFunction.Invoke();
        }
    }


    #region "Public Functions for Player Manager"
    /// <summary>
    /// Initilization function (standin for constructor)
    /// </summary>
    /// <param name="minHeight"> The minimum height the camerea can go to </param>
    /// <param name="cameraDamping"> The camera's movment damping, prevents harsh movments. higher value = for molases </param>
    /// <param name="cameraZoomPadding"> Additional zoomout staticaly applied to the camera. </param>
    public void IntilizeCameraController(CameraSettings settings)
    {
        _cameraDamping = settings._cameraDamping;
        _cameraMinHeight = settings._cameraMinumumHeight;
        _cameraZoomPadding = settings._cameraZoomOut;
    }

    /// <summary>
    /// Call from player manager and pass in player transforms as an array. order dosen't matter
    /// </summary>
    /// <param name="playerTransforms"> player scripts on all of all current players after a player add/drop </param>
    public void UpdateCamera(Player[] players)
    {
        _playerTransforms = ConvertPlayersArrayToTransforms(players);

        if (_playerTransforms != null)
        {
            if(_playerTransforms.Length > 1)
            {
                ChangeCameraMode(CameraModes.Multiplayer);
                return;
            }
            else if(_playerTransforms.Length == 1)
            {
                ChangeCameraMode(CameraModes.SinglePlayer);
            }
        }
        else
        {
            Transform[] returnArray = new Transform[1];
            returnArray[0] = transform;
            Debug.LogWarning("No Players In Game, Unsure where to display camera");
            _playerTransforms = returnArray;
        }
    }
    #endregion

    #region "Camera Mode Functions"

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
        if (_playerTransforms != null)
        {
            Vector3 camAvreagePosition;
            PlayerPositionDistance[] farthestPlayerPair = CaculateLargestPlayerDistance(_playerTransforms);

            float x = 0;
            float z = 0;

            float distanceMagnitude = 0;

            if (farthestPlayerPair != null)
            {
                for (int i = 0; i < farthestPlayerPair.Length; i++)
                {
                    x += farthestPlayerPair[i].playerPos.x;
                    z += farthestPlayerPair[i].playerPos.z;

                    distanceMagnitude += farthestPlayerPair[i].playerDist;
                }
            }

            x /= _playerTransforms.Length;
            z /= _playerTransforms.Length;


            camAvreagePosition = new Vector3(x, distanceMagnitude, z); //prevents the camera from getting to zoomed in

            _currentVelocity = CameraTransform.position - _cameraOldPos; //caculates current movment vector from last frame position

            camAvreagePosition = Vector3.SmoothDamp(CameraTransform.position, camAvreagePosition, ref _currentVelocity, _cameraDamping);
            camAvreagePosition.y = Mathf.Clamp(distanceMagnitude, _cameraMinHeight, 1000);
            camAvreagePosition.y += _cameraZoomPadding;

            CameraTransform.position = camAvreagePosition;

            _cameraOldPos = CameraTransform.position; //sets position for next caculation
        }
    }
    //simplified function for displaying game when there is only one player
    private void SingleplayerCameraDisplayMode()
    {
        if(_playerTransforms != null)
        {
            if (_playerTransforms.Length < 1)
                return;

              _currentVelocity = CameraTransform.position - _cameraOldPos; //caculates current movment vector from last frame position

            Vector3 newCamPos = _playerTransforms[0].position + (Vector3.up * (_cameraMinHeight + _cameraZoomPadding));
            newCamPos = Vector3.SmoothDamp(CameraTransform.position, newCamPos, ref _currentVelocity, _cameraDamping);

            CameraTransform.position = newCamPos;

            _cameraOldPos = CameraTransform.position; //sets position for next caculation
        }
    }
    #endregion

    #region "Helper Functions"
    //Converts player script array into an array of their transforms
    private Transform[] ConvertPlayersArrayToTransforms(Player[] players)
    {
        if (players != null)
        {
            List<Transform> activePlayerTransforms = new List<Transform>();

            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null)
                {
                    activePlayerTransforms.Add(players[i].transform);
                }
            }

            return activePlayerTransforms.ToArray();
        }
        else
        {
            Transform[] returnArray = new Transform[1];
            returnArray[0] = transform;
            Debug.LogWarning("No Players In Game, Unsure where to display camera");
            return returnArray;
        }
    }

    /// <summary>
    /// Caculates the largest distance between two players
    /// </summary>
    private PlayerPositionDistance[] CaculateLargestPlayerDistance(Transform[] playerTransforms)
    {
        List<PlayerPositionDistance> distances = new List<PlayerPositionDistance>();
        Vector3 centerPoint = Vector3.zero; 


        for (int i = 0; i < playerTransforms.Length; i++)
        {
            Vector3 playerCorrectedPosition = playerTransforms[i].position;
            playerCorrectedPosition.y = 0; //ignores Y distance
            playerCorrectedPosition.x /= AspectRatioWidthScale(); //adjusts the distance for diffrent aspect ratios

            PlayerPositionDistance currentPosDist;

            currentPosDist.playerDist = Vector3.Distance(playerCorrectedPosition, centerPoint);
            currentPosDist.playerPos = playerTransforms[i].position;

            distances.Add(currentPosDist);
        }


        if(distances.Count >= 2)
        {
            distances.Sort(DistanceSort);

            PlayerPositionDistance[] returnInfo = new PlayerPositionDistance[2];
            returnInfo[0] = distances[0];
            returnInfo[1] = distances[1];

            return returnInfo;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Caculates the screen's aspect ratio. x is width, y is height. height is always 1
    /// </summary>
    /// <returns></returns>
    private float AspectRatioWidthScale()
    {
        return Screen.width / (float)Screen.height;
    }

    //sorts distances by highest first
    private int DistanceSort(PlayerPositionDistance a, PlayerPositionDistance b)
    {
        if(a.playerDist < b.playerDist)
        {
            return 1;
        }
        else if(a.playerDist > b.playerDist)
        {
            return -1;
        }
        return 0;
    }
    #endregion

    //struct for pairing distances and positions of players.
    private struct PlayerPositionDistance
    {
        public Vector3 playerPos;
        public float playerDist;
    }
}