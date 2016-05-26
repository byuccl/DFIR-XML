package Dfir_representation;

import java.util.Vector;


public class IDataAccessor extends JNode{
	   
	   private String name;
	   private Direction direction; 
	   public IDataAccessor(int nodeId, int parentId, Vector<ITerminal> inputTerminals,Vector<ITerminal> outputTerminals, String name, Direction direction){
		   super(nodeId,parentId,inputTerminals,outputTerminals);
		   this.name = name;
		   this.direction = direction;
		   JNodeType = "IDataAccessor";
	   }
	  public String getName(){
		  return this.name;
	  }
	  public Direction getDirection(){
		  return this.direction;
	  }
	   public void print(){
		   System.out.println("IDataAccessor begin");
		   System.out.println("NodeId: " + this.nodeId + ' ' + "ParentId: " + this.parentId + "\nName: " + this.name + "\nDirection: " + this.direction);
		   if(this.direction == Direction.valueOf("OUTPUT")){
			   for(int i=0; i<this.inputTerminals.size(); i++){
				   this.inputTerminals.get(i).print();
			   }
		   }else if(this.direction == Direction.valueOf("INPUT")){
			   for(int i=0; i<this.outputTerminals.size(); i++){
				   this.outputTerminals.get(i).print();
			   }
		   }
		   System.out.println("IDataAccessor end\n");
	   }
}