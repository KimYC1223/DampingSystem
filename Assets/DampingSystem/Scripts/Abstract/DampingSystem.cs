using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DampingSystem
{
    //=====================================================================================================================================
    //  ██████   █████  ███    ███ ██████  ██ ███    ██  ██████       ███████ ██    ██ ███████ ████████ ███████ ███    ███
    //  ██   ██ ██   ██ ████  ████ ██   ██ ██ ████   ██ ██            ██       ██  ██  ██         ██    ██      ████  ████
    //  ██   ██ ███████ ██ ████ ██ ██████  ██ ██ ██  ██ ██   ███      ███████   ████   ███████    ██    █████   ██ ████ ██
    //  ██   ██ ██   ██ ██  ██  ██ ██      ██ ██  ██ ██ ██    ██           ██    ██         ██    ██    ██      ██  ██  ██
    //  ██████  ██   ██ ██      ██ ██      ██ ██   ████  ██████       ███████    ██    ███████    ██    ███████ ██      ██
    //
    //  DAMPING SYSTEM FOR C# By KimYC1223
    //=====================================================================================================================================
    /// <summary>
    /// <b>Abstract damping system class based on second-order system</b><br />
    /// 2차 시스템으로 구현한 감쇠 시스템의 추상 클래스<br /><br />
    /// <b>Reference :</b>
    /// <a href="https://www.youtube.com/watch?v=KPoeNZZ6H4s">Giving Personality to Procedural Animations using Math</a>
    /// </summary>
    /// <remarks>
    /// <b>Supported data type list :</b><br />
    /// 지원하는 자료형 목록 :<br/>
    /// <ul><li>float</li> <li>float2</li> <li>float3</li> <li>float4</li> <li>Vector2</li> <li>Vector3</li> <li>Vector4</li></ul>
    /// </remarks>
    /// <typeparam name="T"><b>Type to which damping is to be applied</b><br />
    /// Damping을 적용하고자 하는 타입</typeparam>
    public abstract class DampingSystem<T> where T : struct
    {
        //=================================================================================================================================
        /// <summary>
        /// <b>Constructor of a damping system class based on second-order system</b><br />
        /// 2차 시스템을 기반으로 한 감쇠 시스템 클래스의 생성자
        /// </summary>
        /// <param name="frequency"><b>Natural frequency of the damping system ( > 0 )</b><br />
        /// 감쇠 시스템의 자연 진동수 ( 0 초과 )</param>
        /// <param name="dampingRatio"><b>Damping ratio of the damping system ( > 0 )</b><br />
        /// 감쇠 시스템의 감쇠비 ( 0 초과 )</param>
        /// <param name="initialResponse"><b>Initial response of the damping system</b><br />
        /// 감쇠 시스템의 초기 응답</param>
        /// <param name="initialCondition"><b>Initial condition of the damping system</b><br />
        /// 감쇠 시스템의 초기 조건</param>
        //=================================================================================================================================
        public DampingSystem (float frequency, float dampingRatio, float initialResponse, T initialCondition)
        {
            if( frequency <= 0 )
            {
                throw new ArgumentException(
                    "<color=orange>[DampingSystem]</color> Frequency must be greater than 0",
                    nameof(frequency));
            }

            if( dampingRatio <= 0 )
            {
                throw new ArgumentException(
                    "<color=orange>[DampingSystem]</color> Damping ratio must be greater than 0",
                    nameof(dampingRatio));
            }

            W = 2 * Mathf.PI * frequency;
            Z = dampingRatio;
            D = W * Mathf.Sqrt(Mathf.Abs(initialResponse * initialResponse - 1f));

            K1 = Z / (Mathf.PI * frequency);
            K2 = 1 / ( W * W );
            K3 = initialResponse * dampingRatio / W;

            Xp = initialCondition;
            Y = initialCondition;
            Yd = default;
        }

        public float W {protected set; get;}
        public float Z {protected set; get;}
        public float D {protected set; get;}
        public float K1 {protected set; get;}
        public float K2 {protected set; get;}
        public float K3 {protected set; get;}

        public T Xp {protected set; get;}
        public T Y {protected set; get;}
        public T Yd {protected set; get;}

        //=================================================================================================================================
        /// <summary>
        /// <b>Calculate the response of the damping system</b><br />
        /// 감쇠 시스템의 응답을 계산합니다.
        /// </summary>
        /// <param name="x"><b>Current input value of the damping system</b><br />
        /// 감쇠 시스템의 현재 입력 값</param>
        /// <returns><b>Response of the damping system</b><br />
        /// 감쇠 시스템의 응답</returns>
        //=================================================================================================================================
        public T Calculate(T x)
        {
            return Calculate(x, Time.deltaTime);
        }

        //=================================================================================================================================
        /// <summary>
        /// <b>Calculate the response of the damping system</b><br />
        /// 감쇠 시스템의 응답을 계산합니다.
        /// </summary>
        /// <param name="x"><b>Current input value of the damping system</b><br />
        /// 감쇠 시스템의 현재 입력 값</param>
        /// <param name="dt"><b>Time step</b><br />
        /// 시간 간격</param>
        /// <returns><b>Response of the damping system</b><br />
        /// 감쇠 시스템의 응답</returns>
        //=================================================================================================================================
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Calculate(T x, float dt)
        {
            var xd = GetXd(x, dt);
            Xp = x;

            float k1Stable, k2Stable;
            if( W * dt < Z )
            {
                k1Stable = K1;
                k2Stable = Mathf.Max(K2, (dt + K1 )* dt / 2, dt * K1);
            }
            else
            {
                var t1 = Mathf.Exp( -Z * W * dt );
                var alpha = 2 * t1 * (Z <= 1 ? Mathf.Cos (dt * D) : MathF.Cosh(dt * D));
                var beta = t1 * t1;
                var t2 = dt / ( 1 + beta - alpha );
                k1Stable = ( 1 - beta ) * t2;
                k2Stable = dt * t2;
            }

            Y = GetY(dt);
            Yd = GetYd(k1Stable, k2Stable, x, xd, dt);
            return Y;
        }

        protected abstract T GetXd(T x, float dt);
        protected abstract T GetY(float dt);
        protected abstract T GetYd(float k1_stable, float k2_stable, T x, T xd, float dt);
    }
}