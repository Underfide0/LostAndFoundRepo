using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoScript : MonoBehaviour
{
    public int ID;
    public Collider photoCollider;
    [SerializeField] private SanityMeter sanityMeter;
    
    public void RestoreSanity()
    {
        sanityMeter.timer += 120f;
    }
}
