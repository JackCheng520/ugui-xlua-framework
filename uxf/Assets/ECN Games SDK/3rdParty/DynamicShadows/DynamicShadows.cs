using UnityEngine;
using System.Collections;

/// <summary>
/// 动态阴影 - LY;
/// </summary>
public class DynamicShadows : MonoBehaviour
{

    private RenderTexture texShadow;
    public GameObject target;
    public float angle = 30;

    public float aroundHeight;  //地面高度；
    private float overAroundHeight; //离地高度；
    private Transform container;

    public Camera sCamera;
    private Renderer render;

    void Start()
    {

        render = GetComponent<Renderer>();

        container = transform.parent;
        sCamera = container.Find("ShadowsCamera").GetComponent<Camera>();


        texShadow = new RenderTexture(128, 128, -1);
        sCamera.targetTexture = texShadow;

        //sCamera.cullingMask.ToString();
        sCamera.cullingMask = 1 << target.layer;



        sCamera.transform.localPosition = new Vector3(2.7f, -5.39f, -4.65f);
        sCamera.transform.localRotation = Quaternion.Euler(new Vector3(-48.936f, -34.305f, 3.186f));
        sCamera.orthographicSize = 1.28f;

        transform.localPosition = new Vector3(0.149f, 0.0199f, -0.687f);
        transform.localRotation = Quaternion.Euler(new Vector3(90, 139.5002f, 0));
        transform.localScale = new Vector3(0.923f, 0.953f, 0.1289f);

        angle = 88.18f;


        //if (GameApp.curSceneId == 1001) {
        //transform.localPosition = new Vector3(0, 0.01f, -0.7f);
        //transform.localRotation = Quaternion.Euler(new Vector3(90,180,0));
        //transform.localScale = new Vector3(0.54f, 0.55f, 0.16f);
        //angle = 88.18f;
        //  }

        //  sCamera.cullingMask = 1 << 10 + 1 << 11 + 1 << 12;

        // sCamera.cullingMask = target.layer;
    }

    void Update()
    {
        render.material.mainTexture = texShadow;
        container.position = target.transform.position; // new Vector3(target.transform.position.x,aroundHeight,target.transform.position.z);
        container.rotation = Quaternion.LookRotation(Vector3.left); // (target.transform.forward);

        container.rotation = Quaternion.AngleAxis(angle, Vector3.up);
        //	overAroundHeight = target.transform.position.y - aroundHeight;

        //	direct = -target.transform.forward;
        //container.position += direct * overAroundHeight * 2f;
    }

    public void dispose()
    {
        if (gameObject == null)
            return;
        GameObject.Destroy(gameObject);
        GameObject.Destroy(gameObject.transform.parent.gameObject);
    }


}
