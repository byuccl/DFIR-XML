using System;
using System.Collections.Generic;

namespace DFIR_Compiler
{
    public class IDataAccessor:INode
    {
        private string name;
        private Direction direction;
        public IDataAccessor(int nodeId, int parentId, List<ITerminal> inputTerminals, List<ITerminal> outputTerminals, string Name, Direction direction) : base(nodeId, parentId, inputTerminals, outputTerminals)
        {
            name = Name;
            this.direction = direction;
            INodeType = "IDataAccessor";
        }


        public override string GetName()
        {
            return name;
        }
        public virtual Direction GetDirection()
        {
            return direction;
        }


        public override void Print()
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine("IDataAccessor:");
            Console.WriteLine("NodeId: " + nodeId + ' ' + "ParentId: " + parentId + "\nName: " + name + "\nDirection: " + direction);
            if (direction == Direction.OUTPUT)
            {
                for (int i = 0; i < inputTerminals.Count; i++)
                {
                    inputTerminals[i].Print();
                }
            }
            else if (direction == Direction.INPUT)
            {
                for (int i = 0; i < outputTerminals.Count; i++)
                {
                    outputTerminals[i].Print();
                }
            }
        }

    }
}
