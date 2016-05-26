package Dfir_representation;

import java.util.HashMap;
import java.util.Map;
import java.util.Vector;



public  class ITerminal{
	   
	   private int terminalId;
	   private int terminalIndex;
	   private IDataType iDataType;
	   private static Map<Integer, ITerminal> IdToITerminal = new HashMap<Integer, ITerminal>();
	   private Vector<Connection> connections = new Vector<Connection>();
	   
	   public ITerminal(int terminalId, int terminalIndex, IDataType iDataType){
		   this.terminalId = terminalId;
		   this.terminalIndex = terminalIndex;
		   this.iDataType = iDataType;
		   this.connections = new Vector<Connection>();
		   ITerminal.IdToITerminal.put(this.terminalId, this);
	   }
	   public IDataType getIDataType(){
		   return this.iDataType;
	   }
	   public int getTerminalId(){
		   return this.terminalId;
	   }
	   public int getTerminalIndex(){
		   return this.terminalIndex;
	   }
	   public static Map<Integer, ITerminal> getIdToITerminal(){
		   return ITerminal.IdToITerminal;
	   }
	   public Vector<Connection> getConnections(){
		   return this.connections;
	   }
	   public void AddConnection(Connection connection){
		   this.connections.addElement(connection);
	   }
	   public int getITerminalId(){
		   return this.terminalId;
	   }
	   public int getITerminalIndex(){
		   return this.terminalIndex;
	   }
	   public void print(){
		   System.out.println("ITerminal begin");
		   System.out.println("TerminalId: " + this.terminalId + ' ' + "TerminalIndex: " + this.terminalIndex);
		   iDataType.print();
		   for(int i=0; i<connections.size(); i++){
			   connections.get(i).print();
		   }
		   System.out.println("ITerminal end");
	   }
}
