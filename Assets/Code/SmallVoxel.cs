using UnityEngine;
using System.Collections;

public class SmallVoxel : MonoBehaviour
{
    public Sprite m_sprite;
    public string m_name;
    GameObject m_player;

    bool m_isActive = false;

	void Start ()
    {
        m_player = GameObject.Find("First Person Controller");
        if (m_player == null)
        {
            m_player = GameObject.FindWithTag("Player");
        }
        GetComponent<MeshCollider>().enabled = false; //disable collision until collectable
        rigidbody.AddForce(Vector3.up * 300);
        rigidbody.AddTorque(Vector3.up * 300);
        //print(transform.position);

        StartCoroutine(Activate());
    }

    void Update ()
    {
        if (m_isActive)
        {
           //using Hook's law to calculate the spring force towards the player
           float X, K;

            K = 30; //coefficient of restitution
            X = Vector3.Distance(transform.position, m_player.transform.position); //distance
            rigidbody.AddForce((transform.position - m_player.transform.position) * -(K / X));
            if (X < 1.5) //if we are near enough, we add the item to the inventory and make it disappear
            {
                m_player.gameObject.GetComponent<InventoryManager>().AddItem(m_sprite, m_name, 1);
                //m_player.GetComponent<InventoryManager>().RefreshInventory();
                Destroy(this.gameObject);
            }
        }
    }

    IEnumerator Activate()
    {
        yield return new WaitForSeconds(.5f);
        m_isActive = true;
        GetComponent<MeshCollider>().enabled = true;
    }
}
