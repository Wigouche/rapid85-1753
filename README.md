# Rapid 85-1753 Programmable power supply library

## Description

This Repo contains a class for controlling the Rapid 85-1753 or Manson XXXX(need to check models) programmable power supply. the main class is `PowerSupply` this takes the required information to connect to the power supply and contains methods to implement all the remote functions.

To connect to  this power supply a RS232 to RS485 or USB serial to RS485 converter. this current version uses a custom closed source converter that appends additional line endings to responses but the hope is that later versions will use a standard off the shelf converter.

this current version does not implement any functions relating to the timed program mode. hopefully a later version these will be added.

## Example Program

The repo also includes a simple Console example program that demonstrates some of the various functionality.

## future work

the following list is posable features/fixes to work on this project

- add timed program functions 
  - Recall timed program
  - Stop timed program
  - Get timed programs
  - Get specific timed program
  - Set Timed Program

- use single com port for multiple supplies (pass in a serial port object ref and addres rather than a port name)

- change converter to a off the shelf model need to change line endings for this.