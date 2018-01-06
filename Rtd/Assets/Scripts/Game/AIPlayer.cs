using Assets.Scripts.Car;
using UnityEngine;
using System.Collections.Generic;
using System;
using Random = System.Random;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
class AIPlayer : Player
{
    CarControl control;
    List<Vector3> path = new List<Vector3>();
    private int pathIndex;
    float distFromPath = 20.0f;

    float currentSpeed = 0.0f;
    private bool isBreaking = false;
    public float breakForce = 500.0f;

    private bool isRunning = true;
    float timer = 1.5f;
    float resetTimer = 500.0f;
    float m_Downforce = 100.0f;
    CarSpirit spirit;
    UnityEngine.AI.NavMeshAgent agent;
    private static Random rand = new Random();

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        spirit = GetComponent<CarSpirit>();
        control = GetComponent<CarControl>();
        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.autoBraking = false;
        agent.radius = 8.0f;
        agent.speed = 20.0f;
        agent.acceleration = 0.5f;
        if (control.wheelPairs[1].steering)
        {
            frontWheelpair = control.wheelPairs[1];
            backWheelPair = control.wheelPairs[0];
        }
        else
        {
            frontWheelpair = control.wheelPairs[0];
            backWheelPair = control.wheelPairs[1];
        }
    }

    void FixedUpdate()
    {
        if (!startRace)
            return;
        GetSteer();
        Move();
        Breaking();
        control.VisualizeWheel(control.wheelPairs[1]);
        control.VisualizeWheel(control.wheelPairs[0]);
        if (!agent.isOnNavMesh)
        {
            Respawn(false);
        }

        if (spirit._powerUp != null)
            if (rand.Next() % 5 == 0)
                spirit.UsePowerUp();
    }

    void GetSteer()
    {
        if (pathIndex >= path.Count)
        {
            pathIndex = 0;
            GetPath();
        }
        Vector3 steerVector = transform.InverseTransformPoint(new Vector3(
        path[pathIndex].x,
        transform.position.y,
        path[pathIndex].z)
        );
        float newSteer = spirit.MaxSteeringAngle * (steerVector.x / steerVector.magnitude);
        if (control.wheelPairs[0].steering)
        {
            control.wheelPairs[0].rightWheelColider.steerAngle = control.wheelPairs[0].leftWheelColider.steerAngle = newSteer;
        }
        else
        {
            control.wheelPairs[1].rightWheelColider.steerAngle = control.wheelPairs[1].leftWheelColider.steerAngle = newSteer;
        }
        if (steerVector.magnitude <= distFromPath)
        {
            pathIndex++;
            if (pathIndex >= path.Count)
            {
                pathIndex = 0;
                GetPath();
            }
        }

        if (Math.Abs(newSteer) >= 8 && GetComponent<Rigidbody>().velocity.magnitude >= 15)
        {
            if (distFromPath != 0.0f)
            {
                isBreaking = true;
            }
        }
        else
        {
            isBreaking = false;
        }
    }

    void GetPath()
    {
        path.Clear();
        if (map.checkpoints.Length != checkpointOffest + 1)
        {
            agent.destination = map.checkpoints[checkpointOffest + 1].transform.position;
        }
        foreach (var point in agent.path.corners)
        {
            path.Add(point);
        }
    }

    void Move()
    {

        if (isRunning)
        {
            WheelCollider wheelsCollider;
            foreach (var col in control.wheelPairs)
            {
                for (int i = 0; i < 2; i++)
                {
                    wheelsCollider = (i == 1 ? col.leftWheelColider : col.rightWheelColider);
                    currentSpeed = 2 * 22 / 7 * wheelsCollider.radius * wheelsCollider.rpm * 60 / 1000;
                    currentSpeed = Mathf.Round(currentSpeed);

                    if (currentSpeed < spirit.maxSpeed && !isBreaking)
                    {
                        //accelarate the car
                        wheelsCollider.motorTorque = spirit.MaxMotorTorque;
                        wheelsCollider.motorTorque = Mathf.Lerp(wheelsCollider.motorTorque, 0, wheelsCollider.rpm / spirit.maxRPM);
                    }
                    else
                    {
                        wheelsCollider.motorTorque = 0;
                    }
                    AddDownForce();
                }
            }

        }
        else if (isRunning == false)
        {
            driveBackwards();
        }
    }

    private void AddDownForce()
    {
        //wheelsCollider[2].attachedRigidbody.AddForce(-transform.up*m_Downforce * wheelsCollider[3].attachedRigidbody.velocity.magnitude);
        GetComponent<Rigidbody>().AddRelativeForce(-transform.up * m_Downforce * GetComponent<Rigidbody>().velocity.magnitude);
    }

    void Breaking()
    {

        if (isBreaking)
        {
            control.wheelPairs[1].leftWheelColider.brakeTorque = breakForce;
            control.wheelPairs[1].rightWheelColider.brakeTorque = breakForce;
            control.wheelPairs[0].leftWheelColider.brakeTorque = breakForce;
            control.wheelPairs[0].rightWheelColider.brakeTorque = breakForce;

            control.wheelPairs[1].leftWheelColider.motorTorque = 0.0f;
            control.wheelPairs[1].rightWheelColider.motorTorque = 0.0f;
            control.wheelPairs[0].leftWheelColider.motorTorque = 0.0f;
            control.wheelPairs[0].rightWheelColider.motorTorque = 0.0f;
        }
        else
        {
            control.wheelPairs[1].leftWheelColider.brakeTorque = 0.0f;
            control.wheelPairs[1].rightWheelColider.brakeTorque = 0.0f;
            control.wheelPairs[0].leftWheelColider.brakeTorque = 0.0f;
            control.wheelPairs[0].rightWheelColider.brakeTorque = 0.0f;
        }
    }
    void driveBackwards()
    {
        WheelCollider wheelsCollider;
        foreach (var col in control.wheelPairs)
        {
            for (int i = 0; i < 2; i++)
            {
                wheelsCollider = (i == 1 ? col.leftWheelColider : col.rightWheelColider);
                wheelsCollider.motorTorque = -spirit.MaxMotorTorque;
                wheelsCollider.motorTorque = Mathf.Lerp(wheelsCollider.motorTorque, 0, wheelsCollider.rpm / spirit.maxRPM);

                timer -= Time.deltaTime;

                if (timer <= 0)
                {
                    timer = 0;
                    isRunning = true;
                    timer = resetTimer;
                    wheelsCollider.motorTorque = 0;
                }
            }
        }
    }
}
