using UnityEngine;
using System.Collections;

public class MoveCharacter : MonoBehaviour {

	void Start () {
	
	}
	
	void Update ()
    {
        float Haxis = Input.GetAxis("Horizontal");
        float Vaxis = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump"))
        {
            rigidbody.AddForce(Vector3.up * 5, ForceMode.Impulse);
        }

         GetComponent<Rigidbody>().MovePosition(transform.position + new Vector3(Haxis / 10, 0, Vaxis / 10));
    }
}
