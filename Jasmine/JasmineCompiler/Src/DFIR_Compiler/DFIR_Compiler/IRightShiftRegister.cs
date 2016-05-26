using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFIR_Compiler
{
    public class IRightShiftRegister:IBorderNode
    {
        private AssociatedLeftShiftRegister associatedLeftShiftRegister;

        public IRightShiftRegister(int nodeId, int parentId, List<ITerminal> inputTerminals, List<ITerminal> outputTerminals, AssociatedLeftShiftRegister _associatedLeftShiftRegister) : base(nodeId, parentId, inputTerminals, outputTerminals)
        {
            associatedLeftShiftRegister = _associatedLeftShiftRegister;
            INodeType = "IRightShiftRegister";
        }

        public virtual AssociatedLeftShiftRegister GetAssociatedLeftShiftRegister()
        {
            return associatedLeftShiftRegister;
        }

        public override void Print()
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine("IRightShiftRegister:");
            Console.WriteLine("NodeId: " + nodeId + ' ' + "ParentId: " + parentId);
            associatedLeftShiftRegister.Print();
            for (int i = 0; i < inputTerminals.Count; i++)
            {
                inputTerminals[i].Print();
            }
            for (int i = 0; i < outputTerminals.Count; i++)
            {
                outputTerminals[i].Print();
            }
        }

    }
}
