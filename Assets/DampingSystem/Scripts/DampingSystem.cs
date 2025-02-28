using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DampingSystem
{
    /// <summary>
    /// <b>Abstract damping system class based on second-order system</b><br />
    /// 2차 시스템으로 구현한 감쇠 시스템의 추상 클래스
    /// </summary>
    /// <remarks>
    /// <b>Supported data type list :</b><br />
    /// 지원하는 자료형 목록 :<br/>
    /// <ul><li>float</li><li>Vector2</li><li>Vector3</li><li>Vector4</li><li>Quaternion</li></ul>
    /// </remarks>
    /// <typeparam name="T"><b>Type to which damping is to be applied</b><br />
    /// Damping을 적용하고자 하는 타입</typeparam>
    public abstract class DampingSystem<T> where T : struct
    {
        /// <summary>
        /// <b>Constructor of a damping system class based on second-order system</b><br />
        /// 2차 시스템을 기반으로 한 감쇠 시스템 클래스의 생성자
        /// </summary>
        /// <param name="frequnecy"><b>Natural frequency of the damping system ( > 0 )</b><br />
        /// 감쇠 시스템의 자연 진동수 ( 0 초과 )</param>
        /// <param name="dampingRatio"><b>Damping ratio of the damping system ( > 0 )</b><br />
        /// 감쇠 시스템의 감쇠비 ( 0 초과 )</param>
        /// <param name="initialResponse"><b>Initial response of the damping system</b><br />
        /// 감쇠 시스템의 초기 응답</param>
        /// <param name="initialCondition"><b>Initial condition of the damping system</b><br />
        /// 감쇠 시스템의 초기 조건</param>
        public DampingSystem
            (float frequnecy, float dampingRatio, float initialResponse, T initialCondition)
        {
            if( frequnecy <= 0 )
            {
                throw new ArgumentException(
                    "<color=orange>[DampingSystem]</color> Frequency must be greater than 0",
                    nameof(frequnecy));
            }

            if( dampingRatio <= 0 )
            {
                throw new ArgumentException(
                    "<color=orange>[DampingSystem]</color> Damping ratio must be greater than 0",
                    nameof(dampingRatio));
            }

            W = 2 * Mathf.PI * frequnecy;
            Z = dampingRatio;
            D = W * Mathf.Sqrt(initialResponse * initialResponse - 1f);

            K1 = Z / (Mathf.PI * frequnecy);
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

        public T Calculate(T x)
        {
            return Calculate(x, Time.deltaTime);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Calculate(T x, float dt)
        {
            var xd = GetXd(x, dt);
            Xp = x;

            float k1_stable = 0, k2_stable = 0;
            if( W * dt < Z )
            {
                k1_stable = K1;
                k2_stable = Mathf.Max(K2, (dt + K1 )* dt / 2, dt * K1);
            }
            else
            {
                var t1 = Mathf.Exp( -Z * W * dt );
                var alpha = 2 * t1 * (Z <= 1 ? Mathf.Cos (dt * D) : MathF.Cosh(dt * D));
                var beta = t1 * t1;
                var t2 = dt / ( 1 + beta - alpha );
                k1_stable = ( 1 - beta ) * t2;
                k2_stable = dt * t2;
            }

            Y = GetY(dt);
            Yd = GetYd(k1_stable, k2_stable, x, xd, dt);
            Yd = FilterNaN(Yd);
            return Y;
        }

        protected abstract T FilterNaN(T value);
        protected abstract T GetXd(T x, float dt);
        protected abstract T GetY(float dt);
        protected abstract T GetYd(float k1_stable, float k2_stable, T x, T xd, float dt);
    }

    public class DampingSystemFloat : DampingSystem<float>
    {
        public DampingSystemFloat(float frequnecy, float dampingRatio, float initialResponse, float initialCondition) : 
            base(frequnecy, dampingRatio, initialResponse, initialCondition)
        {    
        }

        protected override float FilterNaN(float value)
        {
            return float.IsNaN(value) ? 0 : value;
        }

        protected override float GetXd(float x, float dt)
        {
            return (x - Xp) / dt;
        }

        protected override float GetY(float dt)
        {
            return Y + dt * Yd;
        }

        protected override float GetYd(float k1_stable, float k2_stable, float x, float xd, float dt)
        {
            return Yd + dt * (x + K3 * xd - Y - k1_stable * Yd) / k2_stable;
        }
    }

    public class DampingSystemVector2 : DampingSystem<Vector2>
    {
        public DampingSystemVector2(float frequnecy, float dampingRatio, float initialResponse, Vector2 initialCondition) : 
            base(frequnecy, dampingRatio, initialResponse, initialCondition)
        {
        }

        protected override Vector2 FilterNaN(Vector2 value)
        {
            return new Vector2(
                x : float.IsNaN(value.x) ? 0 : value.x, 
                y : float.IsNaN(value.y) ? 0 : value.y);
        }

        protected override Vector2 GetXd(Vector2 x, float dt)
        {
            return (x - Xp) / dt;
        }

        protected override Vector2 GetY(float dt)
        {
            return Y + dt * Yd;
        }

        protected override Vector2 GetYd(float k1_stable, float k2_stable, Vector2 x, Vector2 xd, float dt)
        {
            return Yd + dt * (x + K3 * xd - Y - k1_stable * Yd) / k2_stable;
        }
    }

    public class DampingSystemVector3 : DampingSystem<Vector3>
    {
        public DampingSystemVector3(float frequnecy, float dampingRatio, float initialResponse, Vector3 initialCondition) : 
            base(frequnecy, dampingRatio, initialResponse, initialCondition)
        {
        }

        protected override Vector3 FilterNaN(Vector3 value)
        {
            return new Vector3(
                x : float.IsNaN(value.x) ? 0 : value.x, 
                y : float.IsNaN(value.y) ? 0 : value.y, 
                z : float.IsNaN(value.z) ? 0 : value.z);
        }

        protected override Vector3 GetXd(Vector3 x, float dt)
        {
            return (x - Xp) / dt;
        }

        protected override Vector3 GetY(float dt)
        {
            return Y + dt * Yd;
        }

        protected override Vector3 GetYd(float k1_stable, float k2_stable, Vector3 x, Vector3 xd, float dt)
        {
            return Yd + dt * (x + K3 * xd - Y - k1_stable * Yd) / k2_stable;
        }
    }

    public class DampingSystemVector4 : DampingSystem<Vector4>
    {
        public DampingSystemVector4(float frequnecy, float dampingRatio, float initialResponse, Vector4 initialCondition) : 
            base(frequnecy, dampingRatio, initialResponse, initialCondition)
        {
        }

        protected override Vector4 FilterNaN(Vector4 value)
        {
            return new Vector4(
                x : float.IsNaN(value.x) ? 0 : value.x, 
                y : float.IsNaN(value.y) ? 0 : value.y, 
                z : float.IsNaN(value.z) ? 0 : value.z, 
                w : float.IsNaN(value.w) ? 0 : value.w);
        }

        protected override Vector4 GetXd(Vector4 x, float dt)
        {
            return (x - Xp) / dt;
        }

        protected override Vector4 GetY(float dt)
        {
            return Y + dt * Yd;
        }

        protected override Vector4 GetYd(float k1_stable, float k2_stable, Vector4 x, Vector4 xd, float dt)
        {
            return Yd + dt * (x + K3 * xd - Y - k1_stable * Yd) / k2_stable;
        }
    }

    public class DampingSystemQuaternion : DampingSystem<Quaternion>
    {
        public DampingSystemQuaternion(float frequnecy, float dampingRatio, float initialResponse, Quaternion initialCondition) : 
            base(frequnecy, dampingRatio, initialResponse, initialCondition)
        {
        }

        protected override Quaternion FilterNaN(Quaternion value)
        {
            return new Quaternion(
                x : float.IsNaN(value.x) ? 0 : value.x, 
                y : float.IsNaN(value.y) ? 0 : value.y, 
                z : float.IsNaN(value.z) ? 0 : value.z, 
                w : float.IsNaN(value.w) ? 0 : value.w);
        }

        protected override Quaternion GetXd(Quaternion x, float dt)
        {
            return Quaternion.Slerp(Quaternion.identity, x * Quaternion.Inverse(Xp), dt);
        }

        protected override Quaternion GetY(float dt)
        {
            return Quaternion.Slerp(Y, Y * Yd, dt);
        }

        protected override Quaternion GetYd(float k1_stable, float k2_stable, Quaternion x, Quaternion xd, float dt)
        {
            var term1 = Quaternion.Lerp(Quaternion.identity, xd, K3);
            var term2 = Quaternion.Lerp(Quaternion.identity, Yd, k1_stable);
            return Quaternion.Slerp(Yd, x * term1 * Quaternion.Inverse(Y) * Quaternion.Inverse(term2), dt/k2_stable);
        }
    }
}