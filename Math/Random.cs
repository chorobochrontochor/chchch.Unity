using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace chchch.Math
{
    public class Ch3Random
    {
        private struct Ch3RandomState : Ch3IRandomState
        {
            private byte[] _state;

            public byte[] State
            {
                get
                {
                    return _state;
                }
            }

            public Ch3RandomState(byte[] p_state)
            {
                _state = p_state;
            }
        }

        private System.Random _random;

        public static Ch3Random New()
        {
            return new Ch3Random();
        }

        public static Ch3Random Restore(Ch3IRandomState p_state)
        {
            return new Ch3Random(p_state);
        }

        public Ch3Random()
        {
            _random = new System.Random();
        }

        public Ch3Random(Ch3IRandomState p_state)
        {
            var memoryStream = new MemoryStream(((Ch3RandomState)p_state).State);

            _random = (System.Random)(new BinaryFormatter()).Deserialize(memoryStream);

            memoryStream.Dispose();
        }

        public int Int()
        {
            return IntBetween(int.MinValue, int.MaxValue);
        }

        public float Float()
        {
            return FloatBetween(float.MinValue, float.MaxValue);
        }

        public double Double()
        {
            return DoubleBetween(double.MinValue, double.MaxValue);
        }

        public int IntBetween(int p_from, int p_to, bool p_fromInclusive = true, bool p_toInclusive = true)
        {
            double from = p_from + (p_fromInclusive ? 0 : 1);
            double to = p_to + (p_toInclusive ? 1 : 0);

            return (int)from + (int)((to - from) * _random.NextDouble());
        }

        public float FloatBetween(float p_from, float p_to, bool p_fromInclusive = true, bool p_toInclusive = true)
        {
            double from = p_from + (p_fromInclusive ? 0 : float.Epsilon);
            double to = p_to + (p_toInclusive ? float.Epsilon : 0);

            return (float) from + (float) ((to - from) * _random.NextDouble());
        }

        public double DoubleBetween(double p_from, double p_to, bool p_fromInclusive = true, bool p_toInclusive = true)
        {
            double from = p_from + (p_fromInclusive ? 0 : double.Epsilon);
            double to = p_to + (p_toInclusive ? double.Epsilon : 0);

            return from + ((to - from) * _random.NextDouble());
        }

        public Ch3IRandomState Save()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();

            binaryFormatter.Serialize(memoryStream, _random);

            byte[] state = memoryStream.ToArray();

            memoryStream.Dispose();

            return new Ch3RandomState(state);
        }
    }
}