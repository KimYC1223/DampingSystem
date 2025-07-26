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
    /// <b>Vector2 damping system class based on second-order system</b><br />
    /// 2차 시스템으로 구현한 Vector2 타입의 감쇠 시스템의 클래스<br /><br />
    /// <b>Reference :</b>
    /// <a href="https://www.youtube.com/watch?v=KPoeNZZ6H4s">Giving Personality to Procedural Animations using Math</a>
    /// </summary>
    public class DampingSystemVector2 : DampingSystem<Vector2>
    {
        //=================================================================================================================================
        /// <summary>
        /// <b>Constructor of a Vector2 damping system class based on second-order system</b><br />
        /// 2차 시스템을 기반으로 한 Vector2 타입의 감쇠 시스템 클래스의 생성자
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
        public DampingSystemVector2(float frequency, float dampingRatio, float initialResponse, Vector2 initialCondition) : 
            base(frequency, dampingRatio, initialResponse, initialCondition)
        {
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
}
