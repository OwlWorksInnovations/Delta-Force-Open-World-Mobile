using UnityEngine;

public class GunManager : MonoBehaviour
{
    public GameObject bullet;
    private float ammo = 30;
    private float canFire = -1.0f;
    private float fireRate = 0.15f;
    public GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > canFire)
        {
            if (ammo > 0)
            {
                canFire = Time.time + fireRate;
                Instantiate(bullet, new Vector3(player.transform.position.x + 2.0f, player.transform.position.y + 1.0f, player.transform.position.z), Quaternion.identity);
            }
        }
    }
}
