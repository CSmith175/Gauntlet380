using UnityEngine;

public class NarrationManager : MonoBehaviour
{
    private static NarrationEvent _triggerNarration;
    private NarrationConstructor _narrationConstructor;
    [SerializeField] private TextDisplay _display;

    [Header("Random Rambiling frequency")]
    [SerializeField] private float _rambleFrequencyMin = 10f;
    [SerializeField] private float _rambleFrequencyMax = 30f;

    private float _currentTime = 0;
    private float _currentInterval = 0;
    private NarrationInputParing[] _blankInputs = new NarrationInputParing[0]; 


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

    private void Update()
    {
        if(_currentTime + _currentInterval < Time.time)
        {
            _currentTime = Time.time;
            _currentInterval = Random.Range(_rambleFrequencyMin, _rambleFrequencyMax);

            PlayNarration(NarrationType.Ramble, _blankInputs);
        }
    }
}
