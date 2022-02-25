using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerFather = null;
    [SerializeField] private GameObject playerBaby = null;
    [SerializeField] private List<GameObject> environment = null;
    [SerializeField] private GameObject cameraSphere = null;

    private Rigidbody fatherRigidbody;
    private Animator fatherAnimator; 
    private Animator childsAnimator;
    private PlayerScript playerScript;
    [SerializeField] private float fatherSpeed = 15;
    private ParticleSystem childsParticleSystem;
    float time = 0;
    private float _move = 1.2f;
    private float _lastFrameFingerPostionX;
    private float _moveFactorX;

    public bool endGameX2 = false;
    public bool endGameX4 = false;
    public bool endGameX6 = false;
    public bool endGameX8 = false;

    [SerializeField] private GameObject childs = null;
    private int childsCount = 1;


    [SerializeField] private float boundrey = 0;

    [SerializeField] private Slider playerBar = null;

    [SerializeField] private Material[] childMaterial = null;

    private void Awake()
    {

        environment = new List<GameObject>(GameObject.FindGameObjectsWithTag("Environment"));
        playerFather = GameObject.FindGameObjectWithTag("Player");
        playerBaby = GameObject.FindGameObjectWithTag("Baby");
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        fatherRigidbody = playerFather.GetComponent<Rigidbody>();
        fatherAnimator = playerFather.GetComponent<Animator>();
        playerBar.value = 50;
    }
    private void FixedUpdate()
    {

        childsAnimator = childs.transform.GetChild(childsCount).GetComponent<Animator>();
        childsParticleSystem = childs.transform.GetChild(childsCount).GetComponent<ParticleSystem>();

            InputSystem();
            MoverSystem();
            EnvironmentSystem();
            TurnSystem();

        if(playerBar.value <= playerBar.minValue)
        {
            if (childsCount > 0)
            {
                childsParticleSystem.Play();
                time += Time.deltaTime;
                childsAnimator.SetTrigger("Turn");
                if (time > 0.80f)
                {
                    childsCount = 0;
                    childs.transform.GetChild(childsCount).gameObject.SetActive(true);
                    childs.transform.GetChild(1).gameObject.SetActive(false);
                    childs.transform.GetChild(2).gameObject.SetActive(false);
                    childs.transform.GetChild(3).gameObject.SetActive(false);
                    playerBar.minValue -= 100;
                    playerBar.maxValue -= 100;
                    playerBar.value = playerBar.minValue + 1;
                    time = 0;
                }
            }
        }
        FinishSystem();
    }

    private void InputSystem()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _lastFrameFingerPostionX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButton(0))
        {
            _moveFactorX = Input.mousePosition.x - _lastFrameFingerPostionX;
            _lastFrameFingerPostionX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _moveFactorX = 0;
        }
        fatherRigidbody.velocity = new Vector3(fatherRigidbody.velocity.x, fatherRigidbody.velocity.y, fatherSpeed);
        fatherAnimator.SetFloat("Father Speed", Mathf.Abs(fatherSpeed));
        float xBondrey = Mathf.Clamp(value: playerFather.transform.position.x, min: -boundrey, max: boundrey);
        playerFather.transform.position = new Vector3(xBondrey, playerFather.transform.position.y, playerFather.transform.position.z);
    }

    private void MoverSystem()
    {
        float swaerSystem = Time.fixedDeltaTime * _move * 0.8f * _moveFactorX;
        playerFather.transform.Translate(swaerSystem, 0, 0);
    }

    private void EnvironmentSystem()
    {
        if (playerScript.plusBar)
        {
            playerBar.value += 20;
            playerScript.plusBar = false;
        }
        if (playerScript.minusBar)
        {
            playerBar.value -= 20;
            playerScript.minusBar = false;
        }
        if (playerScript.greenGate)
        {
            playerBar.value += 50;
            playerScript.greenGate = false;
        }
        if (playerScript.redGate)
        {
            playerBar.value -= 50;
            playerScript.redGate = false;
        }
        for (int i = 0; i < environment.Count; i++)
        {
            if (!environment[i])
            {
                environment.RemoveAt(i);
            }
        }
        for (int i = 0; i < environment.Count; i++)
        {
            environment[i].transform.Rotate(new Vector3(0f, 90 * Time.deltaTime, 0f));
        }
    }
    private void TurnSystem()
    {
        if (playerBar.value >= playerBar.maxValue)
        {
            if (childsCount < 3)
            {
                childsParticleSystem.Play();
                time += Time.deltaTime;
                childsAnimator.SetTrigger("Turn");
                if (time > 0.80f)
                {
                    if (childsCount == 0 || childsCount == 1)
                    {
                        childsCount = 2;
                    }
                    else if (childsCount == 2)
                    {
                        childsCount = 3;
                    }
                    childs.transform.GetChild(childsCount).gameObject.SetActive(true);
                    childs.transform.GetChild(childsCount - 1).gameObject.SetActive(false);
                    playerBar.minValue += 100;
                    playerBar.maxValue += 100;
                    playerBar.value++;
                    time = 0;
                }
            }
        }
    }

    private void FinishSystem()
    {
        if (playerScript.finish)
        {
            fatherRigidbody.velocity = new Vector3(fatherRigidbody.velocity.x, 1.5f, fatherRigidbody.velocity.z);
        } 
        if (playerBar.value < playerBar.maxValue / 2 && childsCount == 0)
        {
            if (playerScript.x2)
            {
                cameraSphere.transform.Rotate(new Vector3(0, 90 * Time.deltaTime, 0));
                fatherRigidbody.velocity = Vector3.zero;
                fatherAnimator.SetBool("Father Win Bool", playerScript.finish);
                childsAnimator.SetBool("Win Bool", playerScript.finish);
            }
        }
         if (playerBar.value >= playerBar.maxValue / 2 && childsCount == 0)
        {
            if (playerScript.x4)
            {
                cameraSphere.transform.Rotate(new Vector3(0, 90 * Time.deltaTime, 0));
                fatherRigidbody.velocity = Vector3.zero;
                fatherAnimator.SetBool("Father Win Bool", playerScript.finish);
                childsAnimator.SetBool("Win Bool", playerScript.finish);
            }

        }
         if (playerBar.value < playerBar.maxValue / 2 && (childsCount == 2 || childsCount == 3))
        {
            if (playerScript.x6)
            {
                cameraSphere.transform.Rotate(new Vector3(0, 90 * Time.deltaTime, 0));
                fatherRigidbody.velocity = Vector3.zero;
                fatherAnimator.SetBool("Father Win Bool", playerScript.finish);
                childsAnimator.SetBool("Win Bool", playerScript.finish);
            }

        }
         if (playerBar.value >= playerBar.maxValue / 2 && (childsCount == 2 || childsCount == 3))
        {
            if (playerScript.x8)
            {
                cameraSphere.transform.Rotate(new Vector3(0, 90 * Time.deltaTime, 0));
                fatherRigidbody.velocity = Vector3.zero;
                fatherAnimator.SetBool("Father Win Bool", playerScript.finish);
                childsAnimator.SetBool("Win Bool", playerScript.finish);
            }
        }
    }
    private void FailSystem()
    {
        fatherRigidbody.velocity = Vector3.zero;
        fatherAnimator.SetBool("Father Fail Bool", true);
        childsAnimator.SetBool("Fail Bool", true);
    }
}