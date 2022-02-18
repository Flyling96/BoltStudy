using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ludiq;
using System.Linq;
using System.IO;
using System;
using System.Collections.ObjectModel;

namespace Bolt.Extend
{
    public class LevelGraphBinaryManager
    {
        private static LevelGraphBinaryManager m_Instance = null;

        public static LevelGraphBinaryManager Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new LevelGraphBinaryManager();
                }

                return m_Instance;
            }
        }

        public void SerializeGraph(FlowGraph graph, string path)
        {
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (var stream = File.Open(path, FileMode.Create))
            {
                var writer = new BinaryWriter(stream);
                SerializeUnits(writer, graph.units);
                SerializeConnect(writer, graph.controlConnections);
                SerializeConnect(writer, graph.valueConnections);
            }
        }

        public FlowGraph DeserializeGraph(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogError($"[DeserializeGraph] don't exists file. filePath : {path}");
                return null;
            }

            FlowGraph flowGraph = new FlowGraph();

            using (var stream = File.Open(path, FileMode.Open))
            {
                var reader = new BinaryReader(stream);
                DeserializeUnits(reader, flowGraph.units);
                DeserializeControlConnect(reader, flowGraph.controlConnections);
                DeserializeValueConnect(reader, flowGraph.valueConnections);
            }

            return flowGraph;
        }

        public FlowGraph DeserializeGraph(TextAsset asset)
        {
            FlowGraph flowGraph = new FlowGraph();
            using (var stream = new MemoryStream(asset.bytes))
            {
                var reader = new BinaryReader(stream);
                DeserializeUnits(reader, flowGraph.units);
                DeserializeControlConnect(reader, flowGraph.controlConnections);
                DeserializeValueConnect(reader, flowGraph.valueConnections);
            }

            return flowGraph;
        }



        private void SerializeUnits(BinaryWriter writer, IEnumerable<IUnit> units)
        {
            m_UnitList.Clear();
            writer.Write(units.Count());
            foreach (var unit in units)
            {
                m_UnitList.Add(unit);
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


        List<IUnit> m_UnitList = new List<IUnit>();

        private void DeserializeUnits(BinaryReader reader, Collection<IUnit> units)
        {
            m_UnitList.Clear();
            int count = reader.ReadInt32();
            long[] positions = new long[count];
            for (int i = 0; i < count; i++)
            {
                var unitName = reader.ReadString();
                int len = reader.ReadInt32();
                positions[i] = (int)reader.BaseStream.Position;
                reader.BaseStream.Seek(len, SeekOrigin.Current);
                Unit unit = null;
                //unit = AutoBinaryUnits.GetUnit(unitName);
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
                catch (Exception e)
                {
                    Debug.LogError($"[LevelGraphBinaryError] type:{m_UnitList[i]?.GetType().Name} \n" + e.Message);
                    Debug.LogError(e.StackTrace);
                }
#endif
            }

            foreach (var unit in m_UnitList)
            {
                units.Add(unit);
            }
        }

        private void SerializeConnect(BinaryWriter writer, IEnumerable<IUnitConnection> connects)
        {
            writer.Write(connects.Count());
            foreach (var connect in connects)
            {
                var input = connect.destination;
                var output = connect.source;
                if (input == null || output == null)
                {
                    Debug.LogError($"[SerializeControlConnect] input or output is null. input : {input} output : {output}");
                    continue;
                }

                writer.Write(m_UnitList.IndexOf(input.unit));
                writer.Write(input.key);
                writer.Write(m_UnitList.IndexOf(output.unit));
                writer.Write(output.key);
            }
        }

        private void DeserializeControlConnect(BinaryReader reader,
            GraphConnectionCollection<ControlConnection, ControlOutput, ControlInput> connections)
        {
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var inputIndex = reader.ReadInt32();
                var inputKey = reader.ReadString();
                var outputIndex = reader.ReadInt32();
                var outputKey = reader.ReadString();
                if (inputIndex < 0 || inputIndex > m_UnitList.Count - 1 || outputIndex < 0 || outputIndex > m_UnitList.Count - 1)
                {
                    Debug.LogError($"[DeserializeControlConnect] Can't find unit. inputIndex : {inputIndex} outputIndex : {outputIndex}");
                    continue;
                }
                var inputUnit = m_UnitList[inputIndex];
                var outputUnit = m_UnitList[outputIndex];
                ControlInput inputPort = null;
                ControlOutput outputPort = null;
                foreach (var input in inputUnit.controlInputs)
                {
                    if (input.key == inputKey)
                    {
                        inputPort = input;
                    }
                }

                foreach (var output in outputUnit.controlOutputs)
                {
                    if (output.key == outputKey)
                    {
                        outputPort = output;
                    }
                }

                if (inputPort == null || outputPort == null)
                {
                    Debug.LogError($"[DeserializeControlConnect] Don't exist port. inputKey : {inputKey} outputKey : {outputKey}");
                    continue;
                }

                ControlConnection connect = new ControlConnection(outputPort, inputPort);
                connections.Add(connect);
            }
        }

        private void DeserializeValueConnect(BinaryReader reader,
            GraphConnectionCollection<ValueConnection, ValueOutput, ValueInput> connections)
        {
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var inputIndex = reader.ReadInt32();
                var inputKey = reader.ReadString();
                var outputIndex = reader.ReadInt32();
                var outputKey = reader.ReadString();
                if (inputIndex < 0 || inputIndex > m_UnitList.Count - 1 || outputIndex < 0 || outputIndex > m_UnitList.Count - 1)
                {
                    Debug.LogError($"[DeserializeValueConnect] Can't find unit. inputIndex : {inputIndex} outputIndex : {outputIndex}");
                    continue;
                }
                var inputUnit = m_UnitList[inputIndex];
                var outputUnit = m_UnitList[outputIndex];
                ValueInput inputPort = null;
                ValueOutput outputPort = null;
                foreach (var input in inputUnit.valueInputs)
                {
                    if (input.key == inputKey)
                    {
                        inputPort = input;
                    }
                }

                foreach (var output in outputUnit.valueOutputs)
                {
                    if (output.key == outputKey)
                    {
                        outputPort = output;
                    }
                }

                if (inputPort == null || outputPort == null)
                {
                    Debug.LogError($"[DeserializeValueConnect] Don't exist port. inputKey : {inputKey} outputKey : {outputKey}");
                    continue;
                }

                ValueConnection connect = new ValueConnection(outputPort, inputPort);
                connections.Add(connect);
            }
        }
    }
}
