using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ludiq;
using System;

namespace Bolt.Extend
{
    public class FunctionSuperUnit : Unit
    {
        [Inspectable, Serialize]
        public string functionName { get; set; }

        private FlowFunctionDeclaration _function;
        [DoNotSerialize]
        private FlowFunctionDeclaration function
        {
            get
            {
                return _function;
            }
            set
            {
                if(_function != value)
                {
                    BeforeFunctionChange();
                    _function = value;
                    AfterFunctionChange();
                }
            }
        }

        protected override void Definition()
        {
            if (string.IsNullOrEmpty(functionName) || graph == null)
            {
                return;
            }

            FlowFunctionDeclaration tempFunction = null;

            foreach (var func in graph.functions)
            {
                if (func.name == functionName)
                {
                    tempFunction = func;
                    break;
                }
            }

            function = tempFunction;

            if (function == null || function.graph == null )
            {
                return;
            }

            isControlRoot = true;

            foreach (var definition in function.graph.validPortDefinitions)
            {
                if (definition is ControlInputDefinition)
                {
                    var controlInputDefinition = (ControlInputDefinition)definition;
                    var key = controlInputDefinition.key;

                    ControlInput(key, (flow) =>
                    {
                        function.executeElement = this;
                        foreach (var unit in function.graph.units)
                        {
                            if (unit is GraphInput)
                            {
                                var inputUnit = (GraphInput)unit;

                                flow.stack.EnterFunctionElement(function);

                                return inputUnit.controlOutputs[key];
                            }
                        }
                        return null;
                    });
                }
                else if (definition is ValueInputDefinition)
                {
                    var valueInputDefinition = (ValueInputDefinition)definition;
                    var key = valueInputDefinition.key;
                    var type = valueInputDefinition.type;
                    var hasDefaultValue = valueInputDefinition.hasDefaultValue;
                    var defaultValue = valueInputDefinition.defaultValue;

                    var port = ValueInput(type, key);

                    if (hasDefaultValue)
                    {
                        port.SetDefaultValue(defaultValue);
                    }
                }
                else if (definition is ControlOutputDefinition)
                {
                    var controlOutputDefinition = (ControlOutputDefinition)definition;
                    var key = controlOutputDefinition.key;

                    ControlOutput(key);
                }
                else if (definition is ValueOutputDefinition)
                {
                    var valueOutputDefinition = (ValueOutputDefinition)definition;
                    var key = valueOutputDefinition.key;
                    var type = valueOutputDefinition.type;

                    ValueOutput(type, key, (flow) =>
                    {
                        flow.stack.EnterFunctionElement(function);

                        foreach (var unit in function.graph.units)
                        {
                            if (unit is GraphOutput)
                            {
                                var outputUnit = (GraphOutput)unit;

                                var value = flow.GetValue(outputUnit.valueInputs[key]);

                                flow.stack.ExitFunctionElement();

                                return value;
                            }
                        }

                        flow.stack.ExitFunctionElement();

                        throw new InvalidOperationException("Missing output unit when to get value.");
                    });
                }
            }
        }

        #region Editing

        public override void BeforeRemove()
        {
            function = null;
            base.BeforeRemove();
        }

        public void AfterFunctionChange()
        {
            if(function == null)
            {
                return;
            }

            function.beforeGraphChange += StopWatchingPortDefinitions;
            function.afterGraphChange += StartWatchingPortDefinitions;

            StartWatchingPortDefinitions();
        }

        public void BeforeFunctionChange()
        {
            if (function == null)
            {
                return;
            }

            StopWatchingPortDefinitions();

            function.beforeGraphChange -= StopWatchingPortDefinitions;
            function.afterGraphChange -= StartWatchingPortDefinitions;
        }

        private void StopWatchingPortDefinitions()
        {
            if (function.graph != null)
            {
                function.graph.onPortDefinitionsChanged -= Define;
            }
        }

        private void StartWatchingPortDefinitions()
        {
            if (function.graph != null)
            {
                function.graph.onPortDefinitionsChanged += Define;
            }
        }

        #endregion
    }
}