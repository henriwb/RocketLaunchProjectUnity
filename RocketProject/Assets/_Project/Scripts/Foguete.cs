using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foguete : MonoBehaviour {

    [Header("Settings")]
    public float propulsionForce;
    public float fuelForce;
    public float fuel;
    private bool launched = false;
    private bool savedMaxHeight = false;
    public float maxHeight = 0f;
    public bool stopLaunch;

    [Header("GameObjects")]
    public Rigidbody body;
    public GameObject rocketBase;
    public GameObject rocketHead;
    public GameObject rocketParachutes;
    public ParticleSystem particles;
    public TrailRenderer trail;
    //[HideInInspector]
    public ConstantForce constForce;

	// Use this for initialization
	void Start () {
        //MainScript.script.followObj = gameObject;
        MainScript.script.sliderFuel.maxValue = fuel;
        MainScript.script.UpdateFuelSlider(fuel);
        if(!stopLaunch)
            StartCoroutine(LaunchRoutine());
	}
	
	// Update is called once per frame
	void Update () {
        CheckForMaxHeight();
        gameObject.transform.rotation = Quaternion.LookRotation(body.velocity);
	}

    private void FixedUpdate()
    {
        constForce.force = new Vector3(MainScript.script.windForce, 0f, 0f);
    }

    //Primeiro Impulso
    public void StartPropulsion()
    {
        body.AddForce(MainScript.script.GetLaunchDirection().normalized * propulsionForce, ForceMode.Impulse);

    }

    //Força ao gastar o combustível
    private void FuelPropulsion()
    {
        body.AddForce(MainScript.script.GetLaunchDirection() * fuelForce, ForceMode.Impulse);
        fuel -=     1f;
        MainScript.script.UpdateFuelSlider(fuel);
        launched = true;
    }


    //Rotina de lançamento
    IEnumerator LaunchRoutine()
    {
        StartPropulsion();
        yield return new WaitForSeconds(0.5f);
        particles.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        MainScript.script.StartFuelTimer();

        while (fuel > 0)
        {
            FuelPropulsion();
            yield return new WaitForSeconds(0.01f);
        }

        particles.gameObject.SetActive(false);

        while (body.velocity.y > 0f)
        {
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(1.5f);
        particles.gameObject.SetActive(true);
        body.AddForce(body.velocity.normalized * propulsionForce, ForceMode.Impulse);

        yield return new WaitForSeconds(1.5f);
        particles.gameObject.SetActive(false);
        LaunchHead();
    }

    //lançamento da cabeça e ativação do Paraquedas
    private void LaunchHead()
    {
            //Setar base Do Foguete após ejetar
            rocketBase.GetComponent<BoxCollider>().enabled = true;
            rocketBase.GetComponent<Rigidbody>().isKinematic = false;
            MainScript.script.followObj = rocketBase.gameObject;

            // Set Paraquetas após Ejetar
            rocketParachutes.GetComponent<MeshRenderer>().enabled = true;
            rocketParachutes.transform.SetParent(MainScript.script.objectRoot.transform);
            Rigidbody parachuteBody = rocketParachutes.GetComponent<Rigidbody>();
            parachuteBody.isKinematic = false;
            parachuteBody.velocity = body.velocity;
            parachuteBody.angularVelocity = body.angularVelocity;
            parachuteBody.GetComponent<Parachute>().CallRoutine();

            // Set Cabeça do Foguete Após Ejetar
            rocketHead.transform.SetParent(MainScript.script.objectRoot.transform);
            rocketHead.GetComponent<Rigidbody>().isKinematic = false;
            rocketHead.GetComponent<BoxCollider>().enabled = true;
            rocketHead.GetComponent<Rigidbody>().AddForce( new Vector3(0.5f,1f,0f)*propulsionForce*5f, ForceMode.Impulse);

            Destroy(gameObject);
    }

    //salva maior posição alcançada 
    private void CheckForMaxHeight()
    {
        //Debug.Log(body.velocity.y);
        if (launched && !savedMaxHeight && body.velocity.y < 0f)
        {
            savedMaxHeight = true;
            maxHeight = gameObject.transform.position.y;
            MainScript.script.UpdateMaxHeight(maxHeight);
        }
    }

    

    
}
