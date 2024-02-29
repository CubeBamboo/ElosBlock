using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

namespace ElosBlock.Wave
{
    public static class MathUtil
    {
        public static Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            return (1 - t) * (1 - t) * (1 - t) * p0 +
                3 * t * (1 - t) * (1 - t) * p1 +
                3 * t * t * (1 - t) * p2 +
                t * t * t * p3;
        }
    }

    public struct WavePointConfig
    {
        public float mass;
        public float damping; //damping force
        public float elastic;
        public float adjacentPara;

        public readonly WavePointConfig Default => new WavePointConfig(mass: 1f, damping: 0.07f, elastic: 0.005f, adjacentPara: 0.25f);

        public WavePointConfig(float mass, float damping, float elastic, float adjacentPara)
        {
            this.mass = mass;
            this.damping = damping;
            this.elastic = elastic;
            this.adjacentPara = adjacentPara;
        }
    }

    public class WavePoint
    {
        public WavePointConfig config;
        public WaveShapeController controller;

        private float originHeight;

        public WavePoint[] wavePoints;
        public Spline splineBelongsTo;
        public int index;
        public int currIndex => index;

        public float height;
        public float radAngle; //angle in radians

        public float velocity; //positive-Direction: off-centre

        public WavePoint(WavePoint[] wavePoints, WavePointConfig config, float originHeight, WaveShapeController controller)
        {
            this.wavePoints = wavePoints;
            this.originHeight = originHeight;
            this.config = config;
            this.controller = controller;
            velocity = 0;
        }

        public void SetHeight(float newHeight)
        {
            height = newHeight;
            ReculculateVertice();
        }

        public void ReculculateVertice()
        {
            var oldPosition = splineBelongsTo.GetPosition(index);
            var newPosition = new Vector3(height * Mathf.Cos(radAngle), height * Mathf.Sin(radAngle), oldPosition.z);
            splineBelongsTo.SetPosition(index, newPosition);
        }

        public void OnRefresh()
        {
            var prevIndex = (index + (wavePoints.Length - 1)) % wavePoints.Length;
            var nextIndex = (index + 1) % wavePoints.Length;

            //计算当前点下一帧的height
            velocity += -config.elastic * (height - originHeight) / config.mass; //elastic force
            var dampingDeacc = Mathf.Abs(velocity) * config.damping / config.mass; //damping force
            height = Mathf.MoveTowards(height, originHeight, dampingDeacc);

            //计算当前点前后两个点的delta velocity
            var prevDeltaVel = config.adjacentPara * config.elastic * (height - wavePoints[prevIndex].height) / config.mass;
            wavePoints[prevIndex].velocity += prevDeltaVel;
            velocity -= prevDeltaVel;

            var nextDeltaVel = config.adjacentPara * config.elastic * (height - wavePoints[nextIndex].height) / config.mass;
            wavePoints[nextIndex].velocity += nextDeltaVel;
            velocity -= nextDeltaVel;

            //更新当前点的height
            height += velocity;
            ReculculateVertice();
        }
    }

    public class WaveShapeController : MonoBehaviour
    {
        private SpriteShapeController spriteShapeController;
        public WavePoint[] wavePoints; //记录所有顶点的数据

        [Header("Global Settings")]
        [Range(3, 100)] public int divideCount = 50;

        public float globalRadius = 4.0f;
        public Vector3 orginPoint = Vector3.zero;

        public float tangentLength = 0.4f;

        [Header("WavePointConfig")]
        public float mass = 1f;
        public float damping = 0.07f; //damping force
        public float elastic = 0.005f;
        public float adjacentPara = 0.25f;

        [Header("Debug")]
        [SerializeField] private float forceOffset = 1f;

        void Start()
        {
            spriteShapeController = GetComponent<SpriteShapeController>();
            InitWave();
        }

        private void FixedUpdate()
        {
            Refresh();
        }

        private void InitWave()
        {
            wavePoints = new WavePoint[divideCount];

            var spline = spriteShapeController.spline;
            var divideRadians = 2 * Mathf.PI / divideCount;

            //init vertices
            //start from originPoint + Vector3.right
            spline.Clear();
            for (int i = 0; i < divideCount; i++)
            {
                var height = globalRadius;
                var angle = -divideRadians / 2f - divideRadians * i;

                var currPoint = orginPoint +
                    new Vector3(height * Mathf.Cos(angle), height * Mathf.Sin(angle));

                spline.InsertPointAt(i, currPoint);
                spline.SetTangentMode(i, ShapeTangentMode.Continuous);
                spline.SetLeftTangent(i, Quaternion.Euler(0, 0, angle*Mathf.Rad2Deg + 90f) * Vector3.right * tangentLength);
                spline.SetRightTangent(i, Quaternion.Euler(0, 0, angle*Mathf.Rad2Deg - 90f) * Vector3.right * tangentLength);
                
                //wave points object
                wavePoints[i] = new WavePoint(wavePoints, new WavePointConfig(mass, damping, elastic, adjacentPara), globalRadius, this);
                wavePoints[i].splineBelongsTo = spline;
                wavePoints[i].index = i;
                wavePoints[i].height = height;
                wavePoints[i].radAngle = angle;
            }
        }

        private void Refresh()
        {
            //refresh every wave point
            for (int i = 0; i < wavePoints.Length; i++)
            {
                wavePoints[i].OnRefresh();
            }
        }

        public void DebugControlMesh()
        {
            //随机修改一个点的高度
            wavePoints[Random.Range(0, divideCount)].SetHeight(globalRadius + forceOffset);
        }
    }
}