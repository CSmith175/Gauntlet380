using UnityEngine;

public class DoorLogic : MonoBehaviour
{
    public bool IsOpened
    {
        get;
        private set;
    }

    private void OnEnable()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        IsOpened = false;
    }

    public void OpenDoor()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        IsOpened = true;
    }
}
