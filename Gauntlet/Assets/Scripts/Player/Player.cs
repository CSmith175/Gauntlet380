using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int _controllerNumber;
    public int ControllerNumber
    {
        get { return _controllerNumber; }
    }

    private ClassData _classData;
    public ClassData ClassData
    {
        get { return _classData; }
    }

    public void InitilizePlayer(int controllerNumber, ClassData classData)
    {
        bool playerControlsInitilized = false;
        _classData = classData;

        foreach(var component in gameObject.GetComponents(typeof(Component)))
        {
            if(!(component is Transform))
            {
                if(component is PlayerControls)
                {
                    SetUpPlayerMovement(component as PlayerControls, controllerNumber, classData);
                    playerControlsInitilized = true;
                }






            }
        }

        if(!playerControlsInitilized)
        {
            SetUpPlayerMovement(null, controllerNumber, classData);
        }

    }



    #region "Helper Functions for Player Initilization

    /// <summary>
    /// Sets up Player Controls monobehavior. Resets one if its passed in, otherwise attatches a new one
    /// </summary>
    /// <param name="controls"> pass in a controls here to reset it instead of creating a new one </param>
    private void SetUpPlayerMovement(PlayerControls controls, int controllerNumber, ClassData classData)
    {
        _controllerNumber = controllerNumber; //sets the held controller number

        if (controls) //resets a currently exsisting controls
        {
            controls.InitilizePlayer(controllerNumber, classData);
        }
        else //creates a new controls and sets it up
        {
            gameObject.AddComponent<PlayerControls>().InitilizePlayer(controllerNumber, classData);

        }
    }

    #endregion
}
