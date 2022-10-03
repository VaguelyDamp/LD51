using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffCard : MonoBehaviour
{
    public enum StaffType {
        Janitor,
        Engineer,
        Conductor,
        Cook,
        None,
    }

    public StaffType staffType;
}
