using UnityEngine;

[CreateAssetMenu(fileName = "NewData", menuName = "ScriptableObjects/Data")]
public class DataTransition : ScriptableObject
{
    public int numSpicesLevel = 0;
    public int numSpicesCollected = 0;

    public int minutes = 0;
    public float seconds = 0;
}
