using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionConeTrigger : MonoBehaviour
{
    private AIController ai;
    // Start is called before the first frame update
    void Start()
    {
        ai = GetComponent<AIController>(); //Find script in parent object
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player in vision cone");
            ai.SetPlayerInVisionCone(true);
            

            //Return player is in cone
            //Line of sight detection
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ai.SetPlayerInVisionCone(false);
        }
    }
}
