using UnityEngine;

namespace PrisonControl
{
    public class PrisonYardGun : MonoBehaviour
    {
        [SerializeField]
        private Transform cam, bulletSpawnPos;

        bool isShooting;

        [SerializeField]
        private GameObject pf_bullet;

        float heldTimer = 0;
        float bulletSpeed;
        float bulletDelay;

        AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            bulletSpeed = 30;
            bulletDelay = 0.1f;
        }

        void Update()
        {

            Shoot();



#if !UNITY_EDITOR
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                {
                    isShooting = true;
                }
                else
                if (touch.phase == TouchPhase.Ended)
                {
                    isShooting = false;
                }
            }
#else
            if (Input.GetMouseButton(0))
            {
                isShooting = true;
            }
            else
            {
                isShooting = false;
            }
#endif

        }

        void Shoot()
        {

            heldTimer += Time.deltaTime;
            if (heldTimer < bulletDelay)
            {
                return;
            }

            heldTimer = 0;


            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity))
            {
                // Face gun in the direction of crosshair hit

                Vector3 dir = hit.point - transform.position;
                transform.rotation = Quaternion.LookRotation(dir);

                if (!isShooting)
                    return;

                GameObject bullet = Instantiate(pf_bullet, bulletSpawnPos.position, Quaternion.identity);
                bullet.GetComponent<Rigidbody>().AddForce(bulletSpawnPos.transform.forward * bulletSpeed, ForceMode.Impulse);

                audioSource.Play();

                Destroy(bullet, 1);

                if (hit.collider.CompareTag("gunTarget"))
                {
                    Debug.Log("shoot");
                    Debug.Log(hit.collider.name);
                    hit.collider.GetComponent<PrisonYardAnimation>().GetHit();
                }
            }
        }
    }
}
