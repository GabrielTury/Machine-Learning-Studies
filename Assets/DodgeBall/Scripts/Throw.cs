using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    public GameObject spherePrefab;
    public GameObject cubePrefab;
    public Material green;
    public Material red;

    DodgeBallPerceptron p;
    // Start is called before the first frame update
    void Awake()
    {
        p = GetComponent<DodgeBallPerceptron>();
    }
    private void Start()
    {
        Time.timeScale = 1.0f;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            GameObject g = Instantiate(spherePrefab, Camera.main.transform.position, Camera.main.transform.rotation);
            g.GetComponent<Renderer>().material = red;
            g.GetComponent<Rigidbody>().AddForce(0, 0, 500);
            p.SendInput(0, 0, 0); // forma 0 (sphere), cor 0 (red), output desejado 0 (crouch)
        }
        else if (Input.GetKeyDown("2"))
        {
            GameObject g = Instantiate(spherePrefab, Camera.main.transform.position, Camera.main.transform.rotation);
            g.GetComponent<Renderer>().material = green;
            g.GetComponent<Rigidbody>().AddForce(0, 0, 500);
            p.SendInput(0, 1, 1); // forma 0 (sphere), cor 1 (green), output desejado 1 (ignora)
        }
        else if(Input.GetKeyDown("3"))
        {
            GameObject g = Instantiate(cubePrefab, Camera.main.transform.position, Camera.main.transform.rotation);
            g.GetComponent<Renderer>().material = red;
            g.GetComponent<Rigidbody>().AddForce(0, 0, 500);
            p.SendInput(1, 0, 1); // forma 1 (cube), cor 0 (red), output desejado 1 (ignora)
        }
        else if(Input.GetKeyDown("4"))
        {
            GameObject g = Instantiate(cubePrefab, Camera.main.transform.position, Camera.main.transform.rotation);
            g.GetComponent<Renderer>().material = green;
            g.GetComponent<Rigidbody>().AddForce(0, 0, 500);
            p.SendInput(1, 1, 1); // forma 1 (cube), cor 1 (green), output desejado 1 (ignora)
        }
    }
}
