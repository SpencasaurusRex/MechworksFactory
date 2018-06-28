using System;
using System.Collections;
using System.Collections.Generic;
using Tiles;
using UnityEngine;

class Move
{
    public Int64 xStart;
    public Int64 yStart;
    public Int64 xEnd;
    public Int64 yEnd;
    public int progress;
    public int totalTicks;

    public Move(Int64 xStart, Int64 yStart, int dx, int dy, int totalTicks)
    {
        this.xStart = xStart;
        this.yStart = yStart;
        this.xEnd = xStart + dx;
        this.yEnd = yStart + dy;
        this.progress = 0;
        this.totalTicks = totalTicks;
    }
}

public struct MovementState
{
    public Int64 tileX;
    public Int64 tileY;
    public float x;
    public float y;
    public int facing;

    public MovementState(Int64 tileX, Int64 tileY, int facing)
    {
        this.tileX = tileX;
        this.tileY = tileY;
        x = tileX;
        y = tileY;
        this.facing = facing;
    }
}

public enum MovementAction
{
    Forward,
    Back
}

public class MovementController
{
    GameController game;
    World world;
    Dictionary<int, Move> moves;
    Dictionary<int, MovementState> states;

    public MovementController(GameController game, World world)
    {
        this.game = game;
        this.world = world;

        moves = new Dictionary<int, Move>(GameController.MAX_ROBOTS);
        states = new Dictionary<int, MovementState>(GameController.MAX_ROBOTS);
    }

    public void RequestMove(int id, MovementAction action)
    {
        if (moves.ContainsKey(id))
        {
            return;
        }
        int actionModifier = -2 * (int)action + 1;
        int facing = states[id].facing;
        int dx = actionModifier * (Mathf.Abs(facing - 2) - 1);
        int dy = actionModifier * (-Mathf.Abs(facing - 1) + 1);
        Move move = new Move(states[id].tileX, states[id].tileY, dx, dy, 30);
        if (world.GetTile(move.xEnd, move.yEnd).type == TileType.Air)
        {
            moves[id] = move;
            world.SetType(move.xEnd, move.yEnd, TileType.Robot);
        }
    }

    public bool CreateEntity(int id, Int64 x, Int64 y, int facing)
    {
        Debug.Assert(!states.ContainsKey(id), "Entity ID already exists");
        if (world.GetTile(x, y).type == TileType.Air)
        {
            states[id] = new MovementState(x, y, facing);
            world.SetType(x, y, TileType.Robot);
            return true;
        }
        return false;
    }

    public MovementState GetState(int id)
    {
        return states[id];
    }

    List<int> removeMoves = new List<int>(GameController.MAX_ROBOTS);
    public void FixedUpdate()
    {
        var moveKeys = moves.Keys;
        foreach (var moveKey in moveKeys)
        {
            var move = moves[moveKey];
            move.progress++;
            if (move.progress >= move.totalTicks)
            {
                removeMoves.Add(moveKey);
            }

            // Set x, y
            var state = states[moveKey];
            float completeRatio = (float)move.progress / move.totalTicks;
            state.x = move.xStart + completeRatio * (move.xEnd - move.xStart);
            state.y = move.yStart + completeRatio * (move.yEnd - move.yStart);
            states[moveKey] = state;
        }

        for (int i = 0; i < removeMoves.Count; i++)
        {
            int key = removeMoves[i];
            var move = moves[key];
            var state = states[key];
            world.SetType(state.tileX, state.tileY, TileType.Air);
            state.tileX = move.xEnd;
            state.tileY = move.yEnd;
            states[key] = state;
            moves.Remove(key);
        }
        removeMoves.Clear();
    }
}
