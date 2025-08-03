using Unity.Mathematics;

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
    /// <b>Float3 damping system class based on second-order system</b><br />
    /// 2차 시스템으로 구현한 Float3 타입의 감쇠 시스템의 클래스<br /><br />
    /// <b>Reference :</b>
    /// <a href="https://www.youtube.com/watch?v=KPoeNZZ6H4s">Giving Personality to Procedural Animations using Math</a>
    /// </summary>
    public class DampingSystemFloat3 : DampingSystem<float3>
    {
        //=================================================================================================================================
        /// <summary>
        /// <b>Constructor of a float damping system class based on second-order system</b><br />
        /// 2차 시스템을 기반으로 한 Float 타입의 감쇠 시스템 클래스의 생성자
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
        public DampingSystemFloat3(float frequency, float dampingRatio, float initialResponse, float3 initialCondition) : 
            base(frequency, dampingRatio, initialResponse, initialCondition)
        {    
        }

        protected override float3 GetXd(float3 x, float dt)
        {
            return (x - Xp) / dt;
        }

        protected override float3 GetY(float dt)
        {
            return Y + dt * Yd;
        }

        protected override float3 GetYd(float k1_stable, float k2_stable, float3 x, float3 xd, float dt)
        {
            return Yd + dt * (x + K3 * xd - Y - k1_stable * Yd) / k2_stable;
        }
    }
}
