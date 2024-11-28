using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.Input;
// using UnityEngine.XR.Interaction.Toolkit;
// using Unity.XR.CoreUtils;

public class PlayerMovement : MonoBehaviour
{
    public XRNode inputSource;
    public float speed = 1.0f;
    public CharacterController character;
    public Transform cameraRig;
    public Transform headTransform;
    private Vector2 inputAxis;
    
    void Start()
    {
        character = GetComponent<CharacterController>();
        cameraRig = Camera.main.transform.parent; // XR Rig의 부모를 참조합니다.
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
        
        // 캐릭터 이동시키기
        character.Move(direction * speed * Time.fixedDeltaTime);
    }
}