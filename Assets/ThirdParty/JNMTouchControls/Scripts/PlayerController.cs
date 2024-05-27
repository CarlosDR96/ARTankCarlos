using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // ... (otros miembros)

    #region Private Members

    private float Gravity = 20.0f;
    private Vector3 _moveDirection = Vector3.zero;

    #endregion

    #region Public Members

    public float Speed = 5.0f;
    public float RotationSpeed = 240.0f;
    public float JumpSpeed = 7.0f;
    public StickController MoveStick;
    public Transform TankFree_Tower;
    public Transform TankFree_Canon;
    public float TowerRotationSpeed = 60.0f;
    public float CanonRotationSpeed = 60.0f;

    #endregion

    private Vector2 MoveStickPos = Vector2.zero;

    void Awake()
    {
        if (MoveStick != null)
        {
            MoveStick.StickChanged += MoveStick_StickChanged;
        }
    }

    private void MoveStick_StickChanged(object sender, StickEventArgs e)
    {
        MoveStickPos = e.Position;
    }

    void Start()
    {
    }

    void Update()
    {
        if (!enabled) return;

        // Rotar la torre y el cañón del tanque usando el stick
        RotateTankComponents();

        // Get Input for axis
        float h = MoveStickPos.x;
        float v = MoveStickPos.y;


    }

    private void RotateTankComponents()
    {
        if (TankFree_Tower != null)
        {
            TankFree_Tower.Rotate(0, MoveStickPos.x * TowerRotationSpeed * Time.deltaTime, 0);
        }

        if (TankFree_Canon != null)
        {
            TankFree_Canon.Rotate(-MoveStickPos.y * CanonRotationSpeed * Time.deltaTime, 0, 0);
        }
    }

}
