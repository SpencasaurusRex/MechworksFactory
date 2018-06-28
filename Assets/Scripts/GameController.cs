using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tiles;
using System;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public const int MAX_ROBOTS = 1024;
    public World world;
    public MovementController movementController;
    public GameObject robotPrefab;

    int currentId;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            throw new Exception("Cannot have more than one GameController");
        }

        world = new World();
        movementController = new MovementController(this, world);
    }

    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            CreateRobot(i, 0);
        }
    }

    void OnDrawGizmos()
    {
        if (world == null) return;
        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                if (world.GetTile(x, y).type == TileType.Robot)
                {
                    Gizmos.DrawCube(new Vector3(x, y), Vector3.one);
                }
            }
        }
    }

    public bool CreateRobot(Int64 x, Int64 y)
    {
        int id = currentId++;
        if (!movementController.CreateEntity(id, x, y, 1))
        {
            return false;
        }
        var robotObject = Instantiate<GameObject>(robotPrefab);
        var robot = robotObject.GetComponent<RobotController>();
        robot.id = id;
        return true;
    }

    void FixedUpdate()
    {
        movementController.FixedUpdate();    
    }
}
