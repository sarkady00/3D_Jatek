using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public bool useOffsetValues;
    public float rotateSpeed;
    public Transform pivot; // ez fogja segíteni a függőleges kamera forgatást
    public float maxViewAngle;
    public float minViewAngle;
    public bool invertCameraY;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!useOffsetValues)
        {
            offset = target.position - transform.position;
        }

        pivot.transform.position = target.transform.position; // elhelyezzük ugyan oda ahol a karakter van
        pivot.transform.parent = target.transform; // megadjuk hogy a karakter child komponense legyen

        Cursor.lockState = CursorLockMode.Locked; // középre zárjuk az egeret és eltüntetjük

    }

    // Update is called once per frame
    void LateUpdate() // update után mozgatjuk a kamerát amikor már tudjuk hol lesz a játékos
    {
        // Lekérjük az egér  x helyzetét és forgatjuk a karaktert
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        target.Rotate(0,horizontal,0);
        
        //ugyan ez y-ra, de csak a kamerát forgatjuk
        float vertical = Input.GetAxis("Mouse Y") * rotateSpeed;
        
        if (invertCameraY) // meg adjuk az esélyt hogy ha valakinek úgy jobb akkor legyen invert camera nézet is
        {
            pivot.Rotate(-vertical,0,0);
        }
        else
        {
            pivot.Rotate(vertical,0,0);
        }

        if (pivot.rotation.eulerAngles.x > maxViewAngle && pivot.rotation.eulerAngles.x < 180 ) // limitáljuk a kamera felfele való forgatását ( ne tudjon ugrálni)
        {
            pivot.rotation = Quaternion.Euler(maxViewAngle,0,0);
        }
        
        if (pivot.rotation.eulerAngles.x > 180 && pivot.rotation.eulerAngles.x < 360 + minViewAngle ) // ugyan ez lefele
        {
            pivot.rotation = Quaternion.Euler(360 + minViewAngle,0,0);
        }

        float desiredYAngle = target.eulerAngles.y;
        float desiredXAngle = pivot.eulerAngles.x;
        Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle,0);
        transform.position = target.position - (rotation * offset); // a kamerát is a karakternek megfelelően forgatjuk
        
        //transform.position = target.position-offset; // a kamerát a karakterhez rögzítjük

        if (transform.position.y < target.position.y)
        {
            transform.position = new Vector3(transform.position.x, target.position.y - 0.5f, transform.position.z);
        }
        
        transform.LookAt(target);
    }
}
