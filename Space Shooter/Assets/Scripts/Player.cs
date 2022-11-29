using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    // prefabs
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shieldVisualizer;

    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1.0f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _tripleShotEnabled = true;
    [SerializeField]
    private bool _speedBoostEnabled = true;
    [SerializeField]
    private bool _shieldEnabled = true;

    [SerializeField]
    private int _score;
    private UIManager _uiManager;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0,0,0);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("spawn manager is null");
        }

        if (_uiManager == null)
        {
            Debug.LogError("ui manager is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            if (_tripleShotEnabled)
            {
                FireTripleShot();
            }
            else
            {
                FireLaser();
            }
            
        }
    }

    public void Damage()
    {
        if (_shieldEnabled)
        {
            _shieldEnabled = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;

        _uiManager.UpdateLives(_lives);
        
        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }   
    }
    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        Instantiate(_laserPrefab, transform.position + new Vector3(0,0.8f,0), Quaternion.identity);
        // quaternion identity: https://docs.unity3d.com/ScriptReference/Quaternion-identity.html
    }

    void FireTripleShot()
    {
        _canFire = Time.time + _fireRate;
        Instantiate(_tripleShotPrefab, transform.position - new Vector3(0.6f,0,0), Quaternion.identity);
    }

    public void TipleShotActive()
    {
        _tripleShotEnabled = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        while (_tripleShotEnabled)
        {
            yield return new WaitForSeconds(5.0f);
            _tripleShotEnabled = false;
        }
    }

    public void SpeedBoostActive()
    {
        _speedBoostEnabled = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
        
    }
    IEnumerator SpeedBoostPowerDownRoutine()
    {
        while (_speedBoostEnabled)
        {
            yield return new WaitForSeconds(5.0f);
            _speedBoostEnabled = false;
        }
    }

     public void ShieldActive()
    {
        _shieldEnabled = true;
        _shieldVisualizer.SetActive(true);
    }

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

        if (_speedBoostEnabled)
        {
            transform.Translate(direction * _speed * 2.0f * Time.deltaTime);
        }
        else 
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }
        

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

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
