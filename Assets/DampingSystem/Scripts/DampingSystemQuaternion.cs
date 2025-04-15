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
    /// <b>Quaternion damping system class based on second-order system</b><br />
    /// 2차 시스템으로 구현한 Quaternion 타입의 감쇠 시스템의 클래스<br /><br />
    /// <b>Reference :</b>
    /// <a href="https://www.youtube.com/watch?v=KPoeNZZ6H4s">Giving Personality to Procedural Animations using Math</a>
    /// </summary>
    public class DampingSystemQuaternion : DampingSystem<Quaternion>
    {
        //=================================================================================================================================
        /// <summary>
        /// <b>Constructor of a Quaternion damping system class based on second-order system</b><br />
        /// 2차 시스템을 기반으로 한 Quaternion 타입의 감쇠 시스템 클래스의 생성자
        /// </summary>
        /// <param name="frequency"><b>Natural frequency of the damping system ( > 0 )</b><br />
        /// 감쇠 시스템의 자연 진동수 ( 0 초과 )</param>
        /// <param name="dampingRatio"><b>Damping ratio of the damping system ( > 0 )</b><br />
        /// 감쇠 시스템의 감쇠비 ( 0 초과 )</param>
        /// <param name="initialResponse"><b>Initial response of the damping system</b><br />
        /// 감쇠 시스템의 초기 응답</param>
        /// <param name="initialCondition"><b>Initial condition of the damping system</b><br />
        /// 감쇠 시스템의 초기 조건</param>
        //================================================================================================================================
        public DampingSystemQuaternion(float frequency, float dampingRatio, float initialResponse, Quaternion initialCondition) : 
            base(frequency, dampingRatio, initialResponse, initialCondition)
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
