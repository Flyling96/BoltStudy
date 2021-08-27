using Ludiq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bolt.Extend
{

    [SerializationVersion("A")]
    public class FlowFunctionDeclaration : FunctionDeclaration<FlowGraph, FlowMacro>
    {
        public override IMachine machine
        {
            get
            {
                if(self != null)
                {
                    return self.GetComponent<FlowMachine>();
                }

                return null;
            }
        }

        public FlowFunctionDeclaration(string name) : base(name) { }

        public override IGraph DefaultGraph()
        {
            return FlowGraph.WithInputOutput();
        }
    }


    public class FlowFunctionDeclarations : FunctionDeclarations<FlowFunctionDeclaration>
    {
        public override IGraphFunctionElement CreateFunction(string key)
        {
            return new FlowFunctionDeclaration(key);
        }
    }
}
