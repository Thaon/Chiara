using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectedBlockUI : MonoBehaviour
{
    GameObject m_player;
    Sprite m_grass;
    Sprite m_dirt;
    Sprite m_stone;
    Sprite m_sand;


    void Start ()
    {
        m_player = GameObject.FindWithTag("Player");
        m_grass = Resources.Load<Sprite>("Grass");
        m_dirt = Resources.Load<Sprite>("Dirt");
        m_stone = Resources.Load<Sprite>("Stone");
        m_sand = Resources.Load<Sprite>("Sand");

    }

    void Update ()
    {
        switch (m_player.GetComponent<PlayerPickAndPlace>().m_selectedBlock)
        {
            case 1:
                GetComponent<Image>().sprite = m_grass;
            break;
            case 2:
                GetComponent<Image>().sprite = m_stone;
            break;
            case 3:
                GetComponent<Image>().sprite = m_dirt;
            break;
            case 4:
                GetComponent<Image>().sprite = m_sand;
            break;
        }
	    
	}
}
