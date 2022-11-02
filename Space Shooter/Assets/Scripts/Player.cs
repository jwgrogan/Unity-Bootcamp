using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    // Start is called before the first frame update
    void Start()
    {
        //set starting position to (0,0,0)
        transform.position = new Vector3(0,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    // create movement method
    void CalculateMovement()
    {
        // need to create these variables to get input
        float horizonatalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizonatalInput, verticalInput, 0);

        // same as new Vector3(1,0,0);
        // Time.deltaTime = 1 second
        // 5 multiplies the 1 meter to 5 meters

        // transform.Translate(Vector3.right * horizonatalInput * _speed * Time.deltaTime);
        // transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime); 

        transform.Translate(direction * _speed * Time.deltaTime);

        // wrap x axis movement
        if (transform.position.x > 11)
        {
            transform.position = new Vector3(-11,transform.position.y,0);
        }
        else if (transform.position.x < -11)
        {
            transform.position = new Vector3(11, transform.position.y,0);
        }
        
        // bound y axis movement
        // if (transform.position.y >= 0 )
        // {
        //     transform.position = new Vector3(transform.position.x,0,0);
        // }
        // else if (transform.position.y <= -3.8f)
        // {
        //     transform.position = new Vector3(transform.position.x,-3.8f,0);
        // }

        // easier option
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0),0);
    }
}
