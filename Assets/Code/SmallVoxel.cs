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
        rigidbody.AddForce(Vector3.up * 300);
        rigidbody.AddTorque(Vector3.up * 300);
        print(transform.position);

        StartCoroutine(Activate());
    }

    void Update ()
    {
        if (m_isActive)
        {
            //using Hook's law to calculate the spring force towards the player
            float K, X;
            K = 30; //coefficient of restitution
            X = Vector3.Distance(transform.position, m_player.transform.position); //distance
            rigidbody.AddForce((transform.position - m_player.transform.position) * -(K / X));
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<InventoryManager>().AddItem(m_sprite, m_name, 1);
            Destroy(this.gameObject);
        }
    }

    IEnumerator Activate()
    {
        yield return new WaitForSeconds(.5f);
        m_isActive = true;
    }
}
