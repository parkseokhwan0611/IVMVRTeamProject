using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    public XRNode inputSource;
    public float speed = 1.0f;
    public Rigidbody rb; // Rigidbody를 사용합니다.
    public Transform cameraRig;
    public Transform headTransform;

    private Vector2 inputAxis;

    [Tooltip("Footstep audio clip to be played.")]
    public AudioClip footstepClip; // 발자국 소리 클립

    [Tooltip("Coughing audio clip to be looped.")]
    public AudioClip coughClip; // 기침 소리 클립

    private AudioSource audioSource; // AudioSource는 내부적으로 추가
    private bool isMoving = false; // 플레이어 움직임 상태
    private bool isTowelActive = false; // 타월 효과 활성화 상태

    private float footstepInterval = 0.5f; // 발자국 소리 간격
    private float nextFootstepTime = 0f; // 다음 발자국 소리 재생 시간

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트를 가져옵니다.
        rb.freezeRotation = true; // 플레이어의 회전은 직접 제어하므로 Rigidbody의 회전을 고정합니다.
        cameraRig = Camera.main.transform.parent; // XR Rig의 부모를 참조합니다.

        // AudioSource 생성 및 초기화
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // 시작 시 자동 재생 안 함

        // 기침 소리 재생 시작
        StartCoughSound();
    }

    void Update()
    {
        // 입력 장치에서 조이스틱 축 값 가져오기
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
    }

    private void FixedUpdate()
    {
        // 카메라 방향을 기준으로 회전 계산하기
        Quaternion headYaw = Quaternion.Euler(0, headTransform.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);

        // Rigidbody를 이용한 이동
        Vector3 move = direction * speed;
        rb.velocity = new Vector3(move.x, rb.velocity.y, move.z); // X, Z 이동, Y축은 중력에 맡김

        // 움직임 상태 업데이트
        if (direction.magnitude > 0.1f)
        {
            isMoving = true;

            // 발자국 소리를 일정 간격으로 재생
            if (Time.time >= nextFootstepTime)
            {
                PlayFootstepSound();
                nextFootstepTime = Time.time + footstepInterval; // 다음 소리 재생 시간 설정
            }
        }
        else
        {
            isMoving = false;
        }
    }

    private void PlayFootstepSound()
    {
        // 발자국 소리를 한 번 재생
        if (footstepClip != null)
        {
            audioSource.PlayOneShot(footstepClip);
        }
    }

    /// <summary>
    /// 타월 효과 시작. 기침 소리를 멈춥니다.
    /// </summary>
    public void ActivateTowelEffect()
    {

        Debug.Log("들어옴");
        isTowelActive = true;

        // 기침 소리 중지
        if (audioSource.isPlaying && audioSource.clip == coughClip)
        {
            audioSource.Stop();
        }
    }

    /// <summary>
    /// 타월 효과 종료. 기침 소리를 재개합니다.
    /// </summary>
    public void DeactivateTowelEffect()
    {


        isTowelActive = false;

        // 기침 소리 재개
        if (coughClip != null && !audioSource.isPlaying)
        {
            StartCoughSound();
        }
    }

    /// <summary>
    /// 게임 시작 시 기침 소리 재생 시작
    /// </summary>
    public void StartCoughSound()
    {
        if (coughClip != null && !isTowelActive)
        {
            audioSource.clip = coughClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
    
}
