using System;

namespace DampingSystem.Demo
{
    [Serializable]
    public struct DampingSystemInitialCondition
    {
        public float Frequency;
        public float DampingRatio;
        public float InitialResponse;
    }
}