using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NarrationManager : MonoBehaviour
{
    private static NarrationEvent _triggerNarration;
    private NarrationConstructor _narrationConstructor;
    [SerializeField] private TextDisplay _display;

    /// <summary>
    /// Event for triggering the narration event
    /// </summary>
    /// <param name="type"> The type of narration, damage, healing, treasure, etc. </param>
    /// <param name="inputs"> The pairs of information type to specific string data to sort into the narration. </param>
    public static void TriggerNarration(NarrationType type, NarrationInputParing[] inputs)
    {
        if (_triggerNarration != null)
        {
            _triggerNarration.Invoke(type, inputs);
        }
    }

    private void PlayNarration(NarrationType type, NarrationInputParing[] inputs)
    {
        if(_narrationConstructor == null)
        {
            _narrationConstructor = new NarrationConstructor();
        }

        if(_display != null)
        {
            _display.DisplayNarration(_narrationConstructor.CreateNarationStringFromEvent(type, inputs));
        }
    }




    //handles subscribing and unsubscribing the narration function
    private void OnEnable()
    {
        if(_triggerNarration == null)
        {
            _triggerNarration = new NarrationEvent();
        }

        _triggerNarration.AddListener(PlayNarration);
    }
    private void OnDisable()
    {
        _triggerNarration.RemoveListener(PlayNarration);
    }
}
