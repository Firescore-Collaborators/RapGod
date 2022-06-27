using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlapAndRun_CellTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject cellPos,prisionPos,camera, cam_zoom;
    public Animator anim;

    public System.Action onGateClosed;
 

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 16)
        {
            StartCoroutine(Delay());
            anim.enabled = true;
            anim.SetBool("open", true);
            other.gameObject.GetComponent<SlapAndRun_CollisionDetection>().playerController.OnCellState(cellPos,prisionPos);
        }
    }

    IEnumerator Delay()
    {
      
        yield return new WaitForSeconds(1);
     //   camera.SetActive(true);
        yield return new WaitForSeconds(3);
        cam_zoom.SetActive(true);
        anim.SetBool("open", false);

        onGateClosed?.Invoke();
    }
}
