# sturdy-journey
This is a simple (in-progress) C# chat server built as a .NET Core console application.

## How to use
Install the .NET Core 2.2 SDK on your machine. Find your download [here](https://dotnet.microsoft.com/download).

Pull the repo onto your machine via fork or clone.

Navigate to the newly-created ```sturdy-journey``` directory. 

From the terminal and in the ```sturdy-journey``` directory, execute ```dotnet run```. 

You should see:

```bash
$ Server has started.
$ Server is waiting...
```
Open a second terminal window and, using your preferred LAN protocol (i.e. Telnet, netcat), open a connection to your localhost using port 44000:

Telnet example: ```$ o 127.0.0.1 44000```  
Netcat example: ```$ nc 127.0.0.1 44000```

You should see:

```bash
$ Enter name:
```
Enter a handle for yourself and start typing messages. Watch the original terminal window for feedback. 

For additional users, repeat the opening of an additional terminal window and connection.

## Notes
- Remaining TODOs:
  - Relaying messages back to client's window, rather than just on the server
  - Displaying to users the command they need to quit
  - More refactoring
  - Exception handling
  - Unit tests

## Credits

- I had never done this before, so I found [this code](https://rosettacode.org/wiki/Chat_server#C.23) as a jumping-off point, then started refactoring to my taste from there.

## License

MIT
