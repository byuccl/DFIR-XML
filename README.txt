The DFIR-XML project focuses on a tool to export LabVIEW Comms
internal data structures to an open format (XML) to simplify the
creation of additional CAD tools which can operate on LabVIEW
Comms-created designs.

The main component located here is source code for a DFIR export tool
which is able to access the NI-internal data structure (DFIR) and
output an equivalent XML file.

In order to use this tool, you should contact National Instruments at 
DFIR.Export@ni.com as they provide a number of the required pieces:

- Documentation in the form of a getting started guide and
  documentation on the DFIR data structure.  These will be very
  helpful to have in order understand what the tools are doing and
  how to interpret the resulting XML files.
- A license and corresponding DLL files which will enable the DFIR
  access mechanism within LabVIEW Comms.

This repository also contains directories for two different example
XML compilers created by students during Winter 2016.  

DFIR Generator
-------------

The DFIR generator is located in the DFIR subdirectory and is a Visual
Studio project which will build a .DLL as described in the "Getting
Started With DfirExport.pdf" file (available from NI).  The project is
called XmlGenerator.  When it is run, the name of the folder it uses
to store the generated XML file is contained in a string variable in
the XmlGenerator.cs source code file.

Wayne's C++ Compiler
--------------------

The first example compiler is contained in the "Wayne" directory and
is written in Java.  It converts the XML generated above to an
equivalent C++ program.  For this compiler, you can run it on the
command line or inside Eclipse. There are two command arguments, one
is the input XML file while another is the schema file used to
validate this XML file. The schema file is found inside the DFIR
directory.  The C++ code produced by this compiler will be stored in a
new cpp file which have the same base name as the input XML file.

Jasmine's C Compiler (to be included)
--------------------

The second example compiler is contained in the "Jasmine" directory
and is written in C#.  It converts the generated XML to C code and is
a complete Visual Studio project.  The files to read from/write to are
hard coded into Jasmine's compiler.


Reports on the creation of both compilers are also contained in the
Wayne and Jasmine directories along with a variety of test case
designs.

