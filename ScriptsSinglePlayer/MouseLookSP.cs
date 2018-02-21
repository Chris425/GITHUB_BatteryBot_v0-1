using UnityEngine;
using System.Collections;

//ALL CREDIT GOES TO 

//http://answers.unity3d.com/questions/29741/mouse-look-script.html


[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLookSP : MonoBehaviour
{

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public static float sensitivityX = 5F;
    public static float sensitivityY = 5F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationY = 0F;

    public static void increaseSensitivity(float increment)
    {
        sensitivityX += increment;
        sensitivityY += increment;
    }
    public static void decreaseSensitivity(float decrement)
    {
        sensitivityX -= decrement;
        sensitivityY -= decrement;
    }

    public static float getSensitivityX()
    {
        return sensitivityX;
    }

    public static float getSensitivityY()
    {
        return sensitivityY;
    }


    void Update()
    {



        if (axes == RotationAxes.MouseXAndY)
        {
            //MOUSE --CDC     
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
            //JOYPAD -CDC
            //float rotationX = transform.localEulerAngles.y + Input.GetAxis("JMouse X") * sensitivityX;

            //MOUSE --CDC 
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            
            //JOYPAD -CDC
            //rotationY += Input.GetAxis("JMouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
        else if (axes == RotationAxes.MouseX)
        {
            //MOUSE --CDC 
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
            //JOYPAD -CDC
            //transform.Rotate(0, Input.GetAxis("JMouse X") * sensitivityX, 0);
        }
        else
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY += Input.GetAxis("JMouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }
    }

    void Start()
    {
    }
}