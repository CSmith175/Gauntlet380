using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorcerer : MeleeEnemy
{
    public float minimumDisappearCooldown = 1.0f;
    public float maximumDisappearCooldown = 3.0f;

    private void OnDisable()
    {
        CancelInvoke("Disappear");
    }

    private void Start()
    {
        InvokeRepeating("Disappear", minimumDisappearCooldown, Random.Range(minimumDisappearCooldown, maximumDisappearCooldown));
    }

    //Can't InvokeRepeating a coroutine so that's why this function exists
    private void Disappear()
    {
        StartCoroutine(Disappearing());
    }

    private IEnumerator Disappearing()
    {
        //Won't be blinking if no players are loaded in to chase
        if (chasingPlayer)
        {
            GetComponent<Renderer>().enabled = false;
            yield return new WaitForSeconds(minimumDisappearCooldown * .75f);
            GetComponent<Renderer>().enabled = true;
        }
        else
        {
            yield return null;
        }
    }
}
