using UnityEngine;
/// <summary>
/// 该脚本用于监控玩家的鼠标变量
/// 马罗 2023 08 26
/// V 1.0.0
/// 目前鼠标点击物块即查看角色在该位置时的视角
/// </summary>
public class MouseSystem : MonoBehaviour
{
    Camera mainCamera;
    void Awake()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
            RoomSystem.instance.OpenPlayerVisible((int)worldPos.x, (int)worldPos.y);
        }
    }
}
