using System;
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

        public int ArrayIndex<T>(T[] p_elements)
        {
            return IntBetween(0, p_elements.Length, true, false);
        }

        public int[] ArrayIndexes<T>(T[] p_elements, int p_count, bool p_canRepeat)
        {
            int realCount = System.Math.Min(System.Math.Max(0, p_count), p_elements.Length);
            int[] result = new int[realCount];

            if (p_canRepeat)
            {
                for (int i = 0; i < realCount; i++)
                {
                    result[i] = ArrayIndex(p_elements);
                }
            }
            else
            {
                int[] indexes = new int[p_elements.Length];
                for (int i = 0; i < indexes.Length; i++)
                {
                    indexes[i] = i;
                }

                for (int i = 0; i < realCount; i++)
                {
                    int randomIndexesIndex = IntBetween(i, indexes.Length, true, false);

                    result[i] = indexes[randomIndexesIndex];
                    indexes[randomIndexesIndex] = indexes[i];
                }
            }

            return result;
        }

        public T ArrayElement<T>(T[] p_elements)
        {
            return p_elements[ArrayIndex(p_elements)];
        }

        public T[] ArrayElements<T>(T[] p_elements, int p_count, bool p_canRepeat)
        {
            int realCount = System.Math.Min(System.Math.Max(0, p_count), p_elements.Length);
            T[] result = new T[realCount];

            if (p_canRepeat)
            {
                for (int i = 0; i < realCount; i++)
                {
                    result[i] = ArrayElement(p_elements);
                }
            }
            else
            {
                int[] indexes = new int[p_elements.Length];
                for (int i = 0; i < indexes.Length; i++)
                {
                    indexes[i] = i;
                }

                for (int i = 0; i < realCount; i++)
                {
                    int randomIndexesIndex = IntBetween(i, indexes.Length, true, false);

                    result[i] = p_elements[indexes[randomIndexesIndex]];
                    indexes[randomIndexesIndex] = indexes[i];
                }
            }

            return result;
        }

        public int ListIndex<T>(IList<T> p_elements)
        {
            return IntBetween(0, p_elements.Count, true, false);
        }

        public K ListElement<T, K>(T p_elements) where T : IList<K>
        {
            return p_elements[ListIndex(p_elements)];
        }

        public K[] ListElements<T, K>(T p_elements, int p_count, bool p_canRepeat) where T : IList<K>
        {
            int realCount = System.Math.Min(System.Math.Max(0, p_count), p_elements.Count);
            K[] result = new K[realCount];

            if (p_canRepeat)
            {
                for (int i = 0; i < realCount; i++)
                {
                    result[i] = ListElement<T, K>(p_elements);
                }
            }
            else
            {
                int[] indexes = new int[p_elements.Count];
                for (int i = 0; i < indexes.Length; i++)
                {
                    indexes[i] = i;
                }

                for (int i = 0; i < realCount; i++)
                {
                    int randomIndexesIndex = IntBetween(i, indexes.Length, true, false);

                    result[i] = p_elements[indexes[randomIndexesIndex]];
                    indexes[randomIndexesIndex] = indexes[i];
                }
            }

            return result;
        }

        public T EnumElement<T>()
        {
            Array enumValues = Enum.GetValues(typeof(T));
            object[] enumValues2 = new object[enumValues.Length];
            enumValues.CopyTo(enumValues2, 0);

            return (T) ArrayElement(enumValues2);
        }

        public T[] EnumElements<T>(int p_count, bool p_canRepeat)
        {
            Array enumValues = Enum.GetValues(typeof(T));
            object[] enumValues2 = new object[enumValues.Length];
            enumValues.CopyTo(enumValues2, 0);

            int realCount = System.Math.Min(System.Math.Max(0, p_count), enumValues2.Length);
            T[] result = new T[realCount];

            if (p_canRepeat)
            {
                for (int i = 0; i < realCount; i++)
                {
                    result[i] = (T) ArrayElement(enumValues2);
                }
            }
            else
            {
                int[] indexes = new int[enumValues2.Length];
                for (int i = 0; i < indexes.Length; i++)
                {
                    indexes[i] = i;
                }

                for (int i = 0; i < realCount; i++)
                {
                    int randomIndexesIndex = IntBetween(i, indexes.Length, true, false);

                    result[i] = (T) enumValues2[indexes[randomIndexesIndex]];
                    indexes[randomIndexesIndex] = indexes[i];
                }
            }

            return result;
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