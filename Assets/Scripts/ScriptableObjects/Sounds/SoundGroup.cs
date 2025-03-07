using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// A group of sounds.
/// </summary>
[CreateAssetMenu(fileName = "New Sound Group", menuName = "Sounds/Sound Group")]
public class SoundGroup : ScriptableObject
{
    //  ------------------ Public ------------------
    [Tooltip("Sounds in question")]
    public Sound[] Sounds;
}
