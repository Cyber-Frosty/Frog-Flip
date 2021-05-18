using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayControls : MonoBehaviour
{
    // Start is called before the first frame update
    public void ExitPressed()
    {
        Destroy(gameObject);
    }
}
