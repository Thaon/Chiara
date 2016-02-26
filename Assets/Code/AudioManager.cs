using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    //removed blocks sounds
    public AudioClip m_grassRm;
    public AudioClip m_stoneRm;
    public AudioClip m_dirtRm;
    public AudioClip m_sandRm;

    //placed blocks sounds
    public AudioClip m_grassPl;
    public AudioClip m_stonePl;
    public AudioClip m_dirtPl;
    public AudioClip m_sandPl;

    //the world!
    ChunkWorldBuilder m_world;


    // Use this for initialization
    void Start ()
    {
        m_world = (ChunkWorldBuilder)FindObjectOfType(typeof(ChunkWorldBuilder));
    }

    // When game object is enabled
    void OnEnable()
    {
        ChunkWorldBuilder.OnEventBlockDestroyed += PlayDestroyBlockSound;
        ChunkWorldBuilder.OnEventBlockPlaced += PlayPlaceBlockSound;
    }
    // When game object is disabled
    void OnDisable()
    {
        ChunkWorldBuilder.OnEventBlockDestroyed += PlayDestroyBlockSound;
        ChunkWorldBuilder.OnEventBlockPlaced += PlayPlaceBlockSound;
    }

    void PlayDestroyBlockSound(m_voxelType type)
    {
        switch((int)type)
        {
            case 1:
                audio.PlayOneShot(m_grassRm);
            break;

            case 2:
                audio.PlayOneShot(m_stoneRm);
            break;

            case 3:
                audio.PlayOneShot(m_dirtRm);
            break;

            case 4:
                audio.PlayOneShot(m_sandRm);
            break;

            default:
                audio.PlayOneShot(m_grassRm);
            break;
        }
        
    }
    // play the place block sound
    void PlayPlaceBlockSound(m_voxelType type)
    {
        {
            switch ((int)type)
            {
                case 1:
                    audio.PlayOneShot(m_grassPl);
                    break;

                case 2:
                    audio.PlayOneShot(m_stonePl);
                    break;

                case 3:
                    audio.PlayOneShot(m_dirtPl);
                    break;

                case 4:
                    audio.PlayOneShot(m_sandPl);
                    break;

                default:
                    audio.PlayOneShot(m_grassPl);
                    break;
            }

        }
    }
}
