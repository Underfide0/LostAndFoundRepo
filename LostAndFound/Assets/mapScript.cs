using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapScript : MonoBehaviour
{
    
    [SerializeField] private GameObject Marks;

    public void activateMarks()
    {
        Marks.SetActive(true);
    }

    public void deactivateMarks()
    {
        Marks.SetActive(false);
    }
}
