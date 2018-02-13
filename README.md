# Rapid 85-1753 Programmable power supply library

## Description

This Repo contains a class for controling the Rapid 85-1753 or manson XXXX(need to check models) programmable power supply. the main class is `PowerSupply` this takes the required information to connect to the power supply and contains methods to implement all the remote functions.

To connect to  this power supply a RS232 to RS485 or USB serial to RS485 converter. this current version uses a custom closed source converter that appends addiotional line endings to commands but the hope is that later versions will use a standard off the shelf converter.

this current vertion does not implement any functions relating to the timed program mode. hopefully a later version these will be added.

## Example Program

The repo also includes a simple Console example program that demontrates some of the verious functionality.

