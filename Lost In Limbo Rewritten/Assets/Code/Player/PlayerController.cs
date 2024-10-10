using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Button m_MedicineButton;
    [SerializeField] Text m_MedicineText;

    [Space]

    [SerializeField] Animator m_BloodSplash;
    [SerializeField] GameObject m_DaggerModel;
    [SerializeField] bool m_HasDagger = false;

    [SerializeField] float m_ArmLerpSpeed = 2;
    float m_LerpTimer = 0;

    [Space]
    [SerializeField] float m_PlayersHealth = 100;
    [SerializeField] float m_PlayerMaxHealth = 100;
    [SerializeField] UnityEvent m_OnDeath;
    [SerializeField] UnityEvent m_OnRevived;

    [Space]
    [SerializeField] bool m_LookAtTarget = false;
    [SerializeField] Transform m_RabbitLocation;

    [SerializeField] bool m_PlayWakeAnimationOnStart = true;
    [Header("General")]
    [SerializeField] float m_PlayerWalkSpeed = 2f;
    [SerializeField] float m_PlayerSprintSpeed = 4f;
    [SerializeField] float m_MouseSensitivity = 100f;
    Rigidbody m_Controller;
    [SerializeField] Camera m_PlayersCamera;

    [SerializeField] float m_CameraLookDownClamp = -70;

    float m_PlayersOverallSpeed;
    [SerializeField] bool m_MovementIsEnabled = true;

    Animator m_BodyAnimations;
    Vector3 m_Position;
    float m_MouseLocationY;
    [SerializeField] float m_AnimationLerpSpeed = 5;

    float m_XAnimationLerp;
    float m_ZAnimationLerp;

    float m_CrouchLerp;

    [SerializeField] float m_CrouchLerpSpeed;
    bool m_IsCrouched = false;

    // Stamina
    [Header("Stamina")]
    [SerializeField] Slider m_StaminaSlider;
    [SerializeField] float m_StaminaAmount;
    float m_StaminaTimer = 0;
    bool m_IsAllowedToSprint = true;

    bool m_LookingOverShoulder = false;
    [SerializeField] TransformParent m_ShoulderLook;
    AbilityTransition m_LimboAbilit;

    DataManager m_DataManager;
    int m_HealthKitsOnHand = 0;
    void Start()
    {
        m_LimboAbilit = GetComponent<AbilityTransition>();
        m_Controller = GetComponent<Rigidbody>();

        m_DataManager = FindAnyObjectByType<DataManager>();

        if (!m_PlayersCamera)
            m_PlayersCamera = GetComponentInChildren<Camera>();

        if (m_MovementIsEnabled)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        m_BodyAnimations = GetComponentInChildren<Animator>();
        m_BodyAnimations.SetBool("Wake", m_PlayWakeAnimationOnStart);

        if (m_StaminaSlider)
        {

            m_StaminaSlider.gameObject.SetActive(false);
            m_StaminaTimer = m_StaminaAmount;
            m_StaminaSlider.maxValue = m_StaminaAmount;
        }

        if (m_DataManager)
            AssignPlayerProperties();


        if (m_MedicineText && m_MedicineButton)
        {
            if (m_HealthKitsOnHand > 0)
            {
                m_MedicineButton.interactable = true;
            }

            m_MedicineText.text = "MEDICINE: x" + m_HealthKitsOnHand.ToString();
        }
    }
    public void UseMedicine()
    {
        if (m_HealthKitsOnHand > 0 && m_PlayersHealth != m_PlayerMaxHealth)
        {
            SubtractHealthKit();
            m_PlayersHealth += 30;
            m_PlayersHealth = Mathf.Clamp(m_PlayersHealth, 0, m_PlayerMaxHealth);

            if (m_HealthKitsOnHand == 0)
                m_MedicineButton.interactable = false;

            m_MedicineText.text = "MEDICINE: x" + m_HealthKitsOnHand.ToString();
        }
    }

    public void RevivePlayer()
    {
        m_MovementIsEnabled = true;
        m_OnRevived.Invoke();

        m_PlayersHealth = m_PlayerMaxHealth;
        m_BodyAnimations.SetBool("IsDead", false);
        m_BloodSplash.SetBool("HealthLow", false);
        m_BodyAnimations.SetLayerWeight(4, 0);
    }

    public void TakeDamage(float _amount)
    {
        m_PlayersHealth -= _amount;

        if (m_PlayersHealth <= 20)
        {
            m_BloodSplash.SetBool("HealthLow", true);
            m_BodyAnimations.SetLayerWeight(4, 0.5f);
        }
        else
            m_BloodSplash.SetTrigger("Splash");

        if (m_PlayersHealth <= 0)
        {
            m_BodyAnimations.SetBool("IsDead", true);
            m_OnDeath.Invoke();
            m_MovementIsEnabled = false;
        }
    }

    public void SetWakeState(bool _state)
    {
        m_PlayWakeAnimationOnStart = _state;
    }

    void Update()
    {
        if (!m_MovementIsEnabled)
            return;

        //if (m_LookAtTarget)
        //{
        //    transform.LookAt(m_RabbitLocation.position, Vector3.up);
        //    m_PlayersCamera.transform.LookAt(m_RabbitLocation.position, Vector3.up);
        //}
        //
        //if (m_HasDagger)
        //{
        //    m_LerpTimer = Mathf.Lerp(m_LerpTimer, 2, m_ArmLerpSpeed * Time.deltaTime);
        //}
        //else
        //{
        //    m_LerpTimer = Mathf.Lerp(m_LerpTimer, -1, m_ArmLerpSpeed * Time.deltaTime);
        //}
        //
        //m_LerpTimer = Mathf.Clamp(m_LerpTimer, 0, 1);
        //m_BodyAnimations.SetLayerWeight(3, m_LerpTimer);

        UpdatePlayersMovement();
        UpdateMouseMovement();
    }

    public void AddHealthKit()
    {
        m_HealthKitsOnHand++;
        m_HealthKitsOnHand = Mathf.Clamp(m_HealthKitsOnHand, 0, 5);

        m_MedicineButton.interactable = true;
        m_MedicineText.text = "MEDICINE: x" + m_HealthKitsOnHand.ToString();
    }

    public void SubtractHealthKit()
    {
        m_HealthKitsOnHand--;
        m_HealthKitsOnHand = Mathf.Clamp(m_HealthKitsOnHand, 0, 5);
    }

    public int GetHealthKitAmount()
    {
        return m_HealthKitsOnHand;
    }

    public bool IsHealthKitsFull()
    {
        if (m_HealthKitsOnHand != 5)
            return false;
        else
            return true;
    }

    public void SetLookAtState(bool _state)
    {
        m_LookAtTarget = _state;
    }

    public void SetWorldPosition(Vector3 _pos)
    {
        transform.position = _pos;
    }

    public void SetWorldRotation(Vector3 _pos)
    {
        transform.eulerAngles = new Vector3(_pos.x, _pos.y, _pos.z);
    }

    void UpdatePlayersMovement()
    {
        float Xpos = Input.GetAxis("Horizontal") * Time.deltaTime;
        float Zpos = Input.GetAxis("Vertical") * Time.deltaTime;

        m_Position = transform.right * Xpos + transform.forward * Zpos;

        if (Xpos > 0)
            m_XAnimationLerp = Mathf.Lerp(m_XAnimationLerp, 1, m_AnimationLerpSpeed * Time.deltaTime);
        else if (Xpos < 0)
            m_XAnimationLerp = Mathf.Lerp(m_XAnimationLerp, -1, m_AnimationLerpSpeed * Time.deltaTime);
        else
            m_XAnimationLerp = Mathf.Lerp(m_XAnimationLerp, 0, m_AnimationLerpSpeed * Time.deltaTime);

        if (Zpos > 0)
            m_ZAnimationLerp = Mathf.Lerp(m_ZAnimationLerp, 1, m_AnimationLerpSpeed * Time.deltaTime);
        else if (Zpos < 0)
            m_ZAnimationLerp = Mathf.Lerp(m_ZAnimationLerp, -1, m_AnimationLerpSpeed * Time.deltaTime);
        else
            m_ZAnimationLerp = Mathf.Lerp(m_ZAnimationLerp, 0, m_AnimationLerpSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.C))
        {
            m_IsCrouched = true;
            m_CrouchLerp = Mathf.Lerp(m_CrouchLerp, 1, m_CrouchLerpSpeed * Time.deltaTime);
        }
        else
        {
            m_IsCrouched = false;
            m_CrouchLerp = Mathf.Lerp(m_CrouchLerp, 0, m_CrouchLerpSpeed * Time.deltaTime);
        }
        m_BodyAnimations.SetLayerWeight(1, m_CrouchLerp);

        // Sprinting Behaviour
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Q))
        {
            if (m_ShoulderLook)
                m_ShoulderLook.SetOverShoulderState(true);
            m_LookingOverShoulder = true;
        }
        else
        {
            m_LookingOverShoulder = false;
            if (m_ShoulderLook)
                m_ShoulderLook.SetOverShoulderState(false);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && m_HasDagger && !m_LimboAbilit.IsInLimbo())
        {
            m_BodyAnimations.SetTrigger("AttackDagger");
            SearchForEnemy();
        }


        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift) && !m_IsCrouched && m_IsAllowedToSprint)
        {
            m_PlayersOverallSpeed = m_PlayerSprintSpeed;
            m_BodyAnimations.SetBool("IsSprinting", true);

            m_StaminaTimer -= Time.deltaTime;
            m_StaminaTimer = Mathf.Clamp(m_StaminaTimer, 0, m_StaminaAmount);

            if (m_StaminaTimer <= 0)
                m_IsAllowedToSprint = false;

        }
        else
        {
            m_StaminaTimer += Time.deltaTime;
            m_StaminaTimer = Mathf.Clamp(m_StaminaTimer, 0, m_StaminaAmount);

            if (m_StaminaTimer >= m_StaminaAmount)
                m_IsAllowedToSprint = true;

            m_PlayersOverallSpeed = m_PlayerWalkSpeed;
            m_BodyAnimations.SetBool("IsSprinting", false);
        }

        // Updating stamina slider UI

        if (m_StaminaSlider)
        {

            m_StaminaSlider.value = m_StaminaTimer;

            if (m_StaminaTimer != m_StaminaAmount)
                m_StaminaSlider.gameObject.SetActive(true);
            else
                m_StaminaSlider.gameObject.SetActive(false);
        }

        // Crouching Behaviour
        if (m_IsCrouched)
        {
            m_PlayersOverallSpeed = m_PlayerWalkSpeed / 2;
        }

        m_Controller.MovePosition(transform.position + m_Position.normalized * m_PlayersOverallSpeed * Time.deltaTime);

        m_BodyAnimations.SetFloat("XPos", m_XAnimationLerp);
        m_BodyAnimations.SetFloat("ZPos", m_ZAnimationLerp);

    }

    public void PlayerDaggerState(bool _state)
    {
        m_HasDagger = _state;
        m_DaggerModel.SetActive(_state);
    }

    void UpdateMouseMovement()
    {
        Vector3 RotationAxis;

        float MouseX = Input.GetAxis("Mouse X") * m_MouseSensitivity * Time.deltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * m_MouseSensitivity * Time.deltaTime;


        if (m_LookingOverShoulder || m_LookAtTarget)
        {
            m_PlayersCamera.transform.localEulerAngles = new Vector3(0, 0, 0);
            m_MouseLocationY = 0;
            return;
        }

        transform.Rotate(transform.up, MouseX);

        //RotationAxis.x = 0;
        //RotationAxis.y = MouseX;
        //RotationAxis.z = 0;
        //
        //transform.eulerAngles = new Vector3(RotationAxis.x,RotationAxis.y, RotationAxis.z);

        m_MouseLocationY -= MouseY;
        m_MouseLocationY = Mathf.Clamp(m_MouseLocationY, -30, m_CameraLookDownClamp);
        m_PlayersCamera.transform.localEulerAngles = new Vector3(m_MouseLocationY, 0, 0);
    }

    void SearchForEnemy()
    {
        RaycastHit m_CastInfo;

        if (Physics.Raycast(m_PlayersCamera.transform.position, m_PlayersCamera.transform.forward, out m_CastInfo, 1f))
        {
            if (m_CastInfo.collider.gameObject.GetComponent<LimboMonsterController>())
                m_CastInfo.collider.gameObject.GetComponent<LimboMonsterController>().TakeDamage(20);
        }
    }

    public bool GetMovementState()
    {
        return m_MovementIsEnabled;
    }
    public void SetMovement(bool _state)
    {
        if (_state)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            m_MovementIsEnabled = _state;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            m_MovementIsEnabled = _state;
        }
    }

    void AssignPlayerProperties()
    {
        m_HasDagger = m_DataManager.GetPlayerDaggerState();
        m_MouseSensitivity = m_DataManager.GetSensitivity();
    }

    public void SavePlayerControllerValues()
    {
        m_DataManager.SetPlayerDaggerState(m_HasDagger);
        m_DataManager.SaveSensitvity(m_MouseSensitivity);
    }

}
