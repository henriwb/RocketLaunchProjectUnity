using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainScript : MonoBehaviour {

    [Header("GameObjects")]
    public GameObject Camera;
    public static MainScript script;
    [HideInInspector]
    public GameObject followObj;
    public Slider sliderFuel;
    public GameObject launchDirection;
    public GameObject launchBase;
    public Text fuelText;
    public Text maxHeightText;
    public Text fuelTime;
    public Text windForceText;
    public GameObject LaunchBase;
    public GameObject rocketPrefab;
    public GameObject objectRoot;
    public float windForce = 0f;
    private bool TrailOn = true;
    




    private void Awake()
    {
        script = this;
    }
    // Use this for initialization
    void Start () {
        StartCoroutine(WindForceRoutine());
	}
	
	// Update is called once per frame
	void Update () {
        if (followObj != null)
        {
            UpdateCameraPos();
            //UpdateFuelSlider();
        }
	}

    //Reinicia a Cena
    public void LaodScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }


    //Lança o Foguete
    public void LaunchRocket()
    {
        if (followObj != null)
        {
            Destroy(followObj);
        }
        followObj = Instantiate(rocketPrefab, objectRoot.transform);
        followObj.SetActive(true);
        EventSystem.current.currentSelectedGameObject.gameObject.SetActive(false);
        if(TrailOn)
            followObj.GetComponent<Foguete>().trail.enabled = true;
    }

    //Atualiza posição da camera
    private void UpdateCameraPos()
    {
            Camera.transform.position = new Vector3(followObj.transform.position.x, followObj.transform.position.y + 3f, Camera.transform.position.z);
    }

    //Atualiza a barra de combustível
    public void UpdateFuelSlider(float fuel)
    {
        sliderFuel.value = fuel;
        fuelText.color = Color.white;
        fuelText.text = "Fuel: " + fuel.ToString();
        if (fuel <= 0)
        {
            fuelText.text = "Fuel: 0";
            fuelText.color = Color.red;
        }
    }

    //Retorna a posição que a base de lançamento está apontada
    public Vector3 GetLaunchDirection()
    {
        Transform T1 = launchDirection.transform;
        Transform T2 = launchBase.transform;
        return new Vector3( T1.position.x - T2.position.x, T1.position.y - T2.position.y);
    }

    //Atualiza altura máxima
    public void UpdateMaxHeight(float height)
    {
        maxHeightText.text = "Max Height " + height.ToString("F2");
        maxHeightText.color = Color.green;
    }

    //Conta o tempo de combustível
    public void StartFuelTimer()
    {
        StartCoroutine(FuelTimeRoutine());
    }

    IEnumerator FuelTimeRoutine()
    {
        float time = 0f;
        while (sliderFuel.value > 0)
        {
            yield return new WaitForSeconds(0.1f);
            time += 0.1f;
            fuelTime.text = "Fuel Timer: " + time.ToString("F1");

        }
    }

    //Ativa/Desativa o trail randerer
    public void SetTrailTrajetory()
    {
        if (EventSystem.current.currentSelectedGameObject.GetComponent<Toggle>().isOn)
        {
            TrailOn = true;
        }else
        {
            TrailOn = false;
        }
    }

    //Atualiza a direção do vento
    IEnumerator WindForceRoutine()
    {
        //int i = 0;
        while (true)
        {
            if (Random.Range(0, 100) > 50)
                windForce += 1f;
            else windForce -= 1f;
            windForceText.text = "Wind: " + windForce.ToString("F1");
            yield return new WaitForSeconds(0.5f);
        }
    }
}
