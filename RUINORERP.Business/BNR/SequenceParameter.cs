﻿﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Business.BNR
{
    /// <summary>
    /// {N:ORDER/00000} 
    /// 序号参数
    /// </summary>
    [ParameterType("N")]
    public class SequenceParameter : IParameterHandler
    {
        private static MemoryMappedFile mSequenceFile;

        private static MemoryMappedViewAccessor mAccessor;

        private static byte[] mBuffer = new byte[64];

        private static Dictionary<string, SequenceItem> mSequenceItems;

        private static System.IO.FileStream mFileStream;

        private static int mRecordSize = 64;

        private static int mRecordCount = 1024 * 100;

        // 批量写入相关字段
        private static Timer mSaveTimer; // 定时保存定时器
        private static readonly object mSaveLock = new object();
        private static volatile bool mHasChanges = false; // 是否有未保存的更改

        static SequenceParameter()
        {
            string filename = AppDomain.CurrentDomain.BaseDirectory + "sequence.data";
            if (!System.IO.File.Exists(filename))
            {
                mFileStream = System.IO.File.Open(filename, System.IO.FileMode.OpenOrCreate);
                byte[] data = new byte[mRecordSize * mRecordCount];
                mFileStream.Write(data, 0, mRecordSize * mRecordCount);
                mFileStream.Flush();
                mFileStream.Close();
            }
            mSequenceFile = MemoryMappedFile.CreateFromFile(filename);
            mAccessor = mSequenceFile.CreateViewAccessor(mRecordSize, 0, MemoryMappedFileAccess.ReadWrite);
            mSequenceItems = new Dictionary<string, SequenceItem>();
            Load();
            
            // 启动定时保存任务，每5秒检查一次是否需要保存
            mSaveTimer = new Timer(SaveChangedItems, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
        }

        public SequenceParameter()
        {

        }


        private SequenceItem GetSequenceItem(string key)
        {
            lock (mSequenceItems)
            {
                SequenceItem result;
                if (!mSequenceItems.TryGetValue(key, out result))
                {
                    result = new SequenceItem();
                    result.Index = mSequenceItems.Count;
                    result.Name = key;
                    mSequenceItems[key] = result;

                }
                return result;
            }

        }

        // 修改为标记更改而不是立即保存
        private static void MarkChanged(SequenceItem item)
        {
            lock (mSaveLock)
            {
                item.IsDirty = true; // 标记为脏数据
                mHasChanges = true;  // 标记有更改需要保存
            }
        }

        // 定时保存任务
        private static void SaveChangedItems(object state)
        {
            if (!mHasChanges) return;
            
            lock (mSaveLock)
            {
                if (!mHasChanges) return;
                
                // 只保存标记为脏数据的项
                foreach (var item in mSequenceItems.Values.Where(i => i.IsDirty))
                {
                    Save(item);
                    item.IsDirty = false; // 清除脏标记
                }
                
                mHasChanges = false;
            }
        }

        private static void Save(SequenceItem item)
        {
            lock (mAccessor)
            {
                string value = item.Name + "=" + item.Value.ToString();
                Int16 length = (Int16)Encoding.UTF8.GetBytes(value, 0, value.Length, mBuffer, 2);
                BitConverter.GetBytes(length).CopyTo(mBuffer, 0);
                mAccessor.WriteArray<byte>(item.Index * mRecordSize, mBuffer, 0, mBuffer.Length);
            }

        }

        private static void Load()
        {
            for (int i = 0; i < 1024; i++)
            {
                mAccessor.ReadArray<byte>(i * mRecordSize, mBuffer, 0, mBuffer.Length);
                int length = BitConverter.ToInt16(mBuffer, 0);
                if (length > 0)
                {
                    string value = Encoding.UTF8.GetString(mBuffer, 2, length);
                    string[] properties = value.Split('=');
                    SequenceItem item = new SequenceItem();
                    item.Index = i;
                    item.Name = properties[0];
                    item.Value = long.Parse(properties[1]);
                    mSequenceItems[item.Name] = item;
                }
                else
                    break;

            }
        }

        public void Execute(StringBuilder sb, string value)
        {
            string[] properties = value.Split('/');
            StringBuilder key = new StringBuilder();
            string[] items = RuleAnalysis.Execute(properties[0]);
            foreach (string p in items)
            {

                string[] sps = RuleAnalysis.GetProperties(p);
                IParameterHandler handler = null;
                if (((BNRFactory)Factory).Handlers.TryGetValue(sps[0], out handler))
                {
                    handler.Execute(key, sps[1]);
                }
            }

            SequenceItem item = GetSequenceItem(key.ToString());
            lock (item)
            {
                item.Value++;
                sb.Append(item.Value.ToString(properties[1]));
            }
            MarkChanged(item); // 标记为需要保存，而不是立即保存

        }

        public class SequenceItem
        {
            // 添加脏标记属性
            public bool IsDirty { get; set; }




            public int Index { get; set; }

            public string Name { get; set; }

            public long Value { get; set; }


        }




        public object Factory
        {
            get;
            set;
        }
    }
}