using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ludiq;
using System.Linq;
using System.IO;
using System;

namespace Bolt.Extend
{
    public class LevelGraphBinaryManager
    {
        private LevelGraphBinaryManager m_Instance = null;

        public LevelGraphBinaryManager Instance
        {
            get
            {
                if(m_Instance == null)
                {
                    m_Instance = new LevelGraphBinaryManager();
                }

                return m_Instance;
            }
        }

        public void SerializeGraph(FlowGraph graph,string path)
        {
            var units = graph.units.Where(x => x is Unit).Select(x => x as Unit);
            var directory = Path.GetDirectoryName(path);
            if(!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using(var stream = File.Open(path, FileMode.Create))
            {
                var writer = new BinaryWriter(stream);
                SerializeUnits(writer, units);
            }
        }

        public FlowGraph DeserializeGraph(string path)
        {
            if(!File.Exists(path))
            {
                Debug.LogError($"[DeserializeGraph] don't exists file. filePath : {path}");
                return null;
            }

            using(var stream = File.Open(path,FileMode.Open))
            {
                var reader = new BinaryReader(stream);
                return DeserializeGraph(reader);
            }
        }

        
        private void SerializeUnits(BinaryWriter writer,IEnumerable<Unit> units)
        {
            writer.Write(units.Count());
            foreach(var unit in units)
            {
                writer.Write(unit.GetType().Name);
                //长度占位
                writer.Write(0);
                var start = writer.BaseStream.Position;
                unit.BinarySerialize(writer);
                var end = writer.BaseStream.Position;
                writer.BaseStream.Seek(start - 4, SeekOrigin.Begin);
                writer.Write((int)(end - start));
                writer.BaseStream.Seek(end, SeekOrigin.Begin);
            }
        }

        List<Unit> m_UnitList = new List<Unit>();

        public FlowGraph DeserializeGraph(BinaryReader reader)
        {
            m_UnitList.Clear();
            FlowGraph graph = new FlowGraph();
            int count = reader.ReadInt32();
            long[] positions = new long[count];
            for (int i = 0; i < count; i++)
            {
                var unitName = reader.ReadString();
                int len = reader.ReadInt32();
                reader.BaseStream.Seek(len, SeekOrigin.Current);
                var unit = AutoBinaryUnits.GetUnit(unitName);
                m_UnitList.Add(unit);
                if (unit == null)
                {
                    Debug.LogError($"[DeserlializeGraph] can't find unit. unitName: {unitName}");
                }
            }

            for (int i = 0; i < count; i++)
            {
                reader.BaseStream.Seek(positions[i], SeekOrigin.Begin);
#if UNITY_EDITOR
                try
                {
#endif
                    m_UnitList[i].BinaryDeserialize(reader);
#if UNITY_EDITOR
                }
                catch(Exception e)
                {
                    Debug.LogError($"[LevelGraphBinaryError] type:{m_UnitList[i]?.GetType().Name} \n" + e.Message);
                    Debug.LogError(e.StackTrace);
                }
#endif
            }
            return graph;
        }
    }
}
