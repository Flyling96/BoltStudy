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
                SerializeGraphFunctions(writer, graph.functions);
                SerializeGraph(writer, graph);
            }
        }

        private void SerializeGraph(BinaryWriter writer, FlowGraph graph)
        {
            SerializeGraphVarialbes(writer, graph.variables);
            SerializeGraphPorts(writer, graph.validPortDefinitions);
            SerializeUnits(writer, graph.units);
            SerializeConnect(writer, graph.controlConnections);
            SerializeConnect(writer, graph.valueConnections);
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
                DeserializeGraphFunctions(reader, flowGraph.functions);
                DeserializeGraph(reader, ref flowGraph);
            }

            return flowGraph;
        }

        public FlowGraph DeserializeGraph(TextAsset asset)
        {
            FlowGraph flowGraph = new FlowGraph();
            using (var stream = new MemoryStream(asset.bytes))
            {
                var reader = new BinaryReader(stream);
                DeserializeGraphFunctions(reader, flowGraph.functions);
                DeserializeGraph(reader, ref flowGraph);
            }

            return flowGraph;
        }

        private void DeserializeGraph(BinaryReader reader, ref FlowGraph graph)
        {
            DeserializeGraphVarialbes(reader, graph.variables);
            DeserializeGraphPorts(reader, graph);
            DeserializeUnits(reader, graph.units);
            DeserializeControlConnect(reader, graph.controlConnections);
            DeserializeValueConnect(reader, graph.valueConnections);
        }

        private void SerializeGraphPorts(BinaryWriter writer, IEnumerable<IUnitPortDefinition> ports)
        {
            writer.Write(ports.Count());
            foreach (var port in ports)
            {
                if (port is ControlInputDefinition controlInput)
                {
                    writer.Write(0);
                    SerializeGraphPort(writer, controlInput);
                }
                else if (port is ControlOutputDefinition controlOutput)
                {
                    writer.Write(1);
                    SerializeGraphPort(writer, controlOutput);
                }
                else if(port is ValueInputDefinition valueInput)
                {
                    writer.Write(2);
                    SerializeGraphPort(writer, valueInput);
                    writer.Write(RuntimeCodebase.SerializeType(valueInput.type));
                    BinaryManager.Instance.SerializeObject(writer, valueInput.defaultValue);
                }
                else if(port is ValueOutputDefinition valueOutput)
                {
                    writer.Write(3);
                    SerializeGraphPort(writer, valueOutput);
                    writer.Write(RuntimeCodebase.SerializeType(valueOutput.type));
                }
            }
        }

        private void SerializeGraphPort(BinaryWriter writer, UnitPortDefinition port)
        {
            if(port.key == null)
            {
                port.key = string.Empty;
            }
            if (port.label == null)
            {
                port.label = string.Empty;
            }
            if (port.summary == null)
            {
                port.summary = string.Empty;
            }
            writer.Write(port.key);
            writer.Write(port.label);
            writer.Write(port.summary);
            writer.Write(port.hideLabel);
        }

        private void DeserializeGraphPorts(BinaryReader reader, FlowGraph graph)
        {
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var index = reader.ReadInt32();
                if(index == 0)
                {
                    var port = new ControlInputDefinition();
                    DeserializeGraphPort(reader, port);
                    graph.controlInputDefinitions.Add(port);
                }
                else if(index == 1)
                {
                    var port = new ControlOutputDefinition();
                    DeserializeGraphPort(reader, port);
                    graph.controlOutputDefinitions.Add(port);
                }
                else if(index == 2)
                {
                    var port = new ValueInputDefinition();
                    DeserializeGraphPort(reader, port);
                    var typeString = reader.ReadString();
                    if(RuntimeCodebase.TryDeserializeType(typeString,out var type))
                    {
                        port.type = type;
                    }
                    object @object = null;
                    BinaryManager.Instance.DeserializeObject(reader, ref @object);
                    port.defaultValue = @object;
                    graph.valueInputDefinitions.Add(port);
                }
                else if(index == 3)
                {
                    var port = new ValueOutputDefinition();
                    DeserializeGraphPort(reader, port);
                    var typeString = reader.ReadString();
                    if (RuntimeCodebase.TryDeserializeType(typeString, out var type))
                    {
                        port.type = type;
                    }
                    graph.valueOutputDefinitions.Add(port);
                }
            }
        }

        private void DeserializeGraphPort(BinaryReader reader, UnitPortDefinition port)
        {
            port.key = reader.ReadString();
            port.label = reader.ReadString();
            port.summary = reader.ReadString();
            port.hideLabel = reader.ReadBoolean();
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
                unit = AutoBinaryUnits.GetUnit(unitName);
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

        private void SerializeGraphVarialbes(BinaryWriter writer, VariableDeclarations variables)
        {
            writer.Write(variables.Count());
            foreach(var variable in variables)
            {
                writer.Write(variable.name);
                BinaryManager.Instance.SerializeObject(writer, variable.value);
            }
        }

        private void DeserializeGraphVarialbes(BinaryReader reader, VariableDeclarations variables)
        {
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var name = reader.ReadString();
                object value = null;
                BinaryManager.Instance.DeserializeObject(reader, ref value);
                variables.Set(name, value);
            }
        }

        private void SerializeGraphFunctions(BinaryWriter writer, FlowFunctionDeclarations functions)
        {
            writer.Write(functions.Count());
            foreach(var function in functions)
            {
                writer.Write(function.name);
                writer.Write((int)function.source);
                if(function.source == GraphSource.Embed)
                {
                    SerializeGraph(writer, function.embed);
                }
                else
                {
                    //TODO
                }
            }
        }

        private void DeserializeGraphFunctions(BinaryReader reader, FlowFunctionDeclarations functions)
        {
            var count = reader.ReadInt32();
            for(int i =0; i< count;i++)
            {
                var name = reader.ReadString();
                var function = new FlowFunctionDeclaration(name);
                var source = (GraphSource)reader.ReadInt32();
                function.source = source;
                if(source == GraphSource.Embed)
                {
                    var graph = new FlowGraph();
                    DeserializeGraph(reader, ref graph);
                    function.embed = graph;
                }
                else
                {
                    //TODO
                }
                functions.Add(function);

            }
        }
    }
}
