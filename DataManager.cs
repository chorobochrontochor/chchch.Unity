using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace chchch
{
    public static class DataManager
    {
        [DllImport("__Internal")]
        private static extern void SyncFiles();

        public delegate void PersistBinaryDataCallback(bool p_success);

        public static void PersistBinaryData<T>(T p_data, string p_fileName, PersistBinaryDataCallback p_callback) where T : class
        {
            bool success = false;
            string filePath = Application.persistentDataPath + p_fileName;

            try
            {
                FileStream fileStream = File.OpenWrite(filePath);

                if (fileStream.CanWrite)
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();

                    binaryFormatter.Serialize(fileStream, p_data);

                    if (Application.platform == RuntimePlatform.WebGLPlayer)
                    {
                        SyncFiles();
                    }

                    success = true;
                }

                fileStream.Close();
            }
            catch (Exception exception)
            {
                Logger.DebugLogError(exception.Message);
            }

            p_callback(success);
        }

        public delegate void LoadBinaryDataCallback<T>(bool p_success, T p_result) where T : class;

        public static void LoadBinaryData<T>(string p_fileName, LoadBinaryDataCallback<T> p_callback) where T : class
        {
            T result = null;
            string filePath = Application.persistentDataPath + p_fileName;

            try
            {
                if (File.Exists(filePath))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();

                    FileStream fileStream = File.OpenRead(filePath);

                    if (fileStream.CanRead)
                    {
                        result = (T) binaryFormatter.Deserialize(fileStream);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.DebugLogError(exception.Message);
            }

            p_callback(result != null, result);
        }
    }
}