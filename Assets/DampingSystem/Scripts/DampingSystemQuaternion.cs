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

        protected override Quaternion GetXd(Quaternion x, float dt)
        {
            if (dt == 0) return Quaternion.identity;
            return Quaternion.SlerpUnclamped(Quaternion.identity, x * Quaternion.Inverse(Xp), 1f / dt);
        }

        protected override Quaternion GetY(float dt)
        {
            return Quaternion.SlerpUnclamped(Y, Y * Yd, dt);
        }

        protected override Quaternion GetYd(float k1_stable, float k2_stable, Quaternion x, Quaternion xd, float dt)
        {
            var targetRotation = x * Quaternion.SlerpUnclamped(Quaternion.identity, xd, K3) * Quaternion.Inverse(Y);
            var dampingTerm = Quaternion.SlerpUnclamped(Quaternion.identity, Yd, k1_stable);
            var finalRotation = targetRotation * Quaternion.Inverse(dampingTerm);
            
            return Quaternion.SlerpUnclamped(Yd, finalRotation, dt / k2_stable);
        }
    }
}
