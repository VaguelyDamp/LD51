using System.Collections.Generic;

namespace qASIC.Console.Commands
{
    public class ExampleCommand : GameConsoleCommand
    {
        public override bool Active { get => GameConsoleController.GetConfig().clearCommand; }
        public override string CommandName { get; } = "example";
        public override string Description { get; } = "says 'hello world'";
        public override string Help { get; } = "Prints 'hello world' to console, if an argument is given greets that instead";
        public override string[] Aliases { get; } = new string[] { "eg", "test" };

        public override void Run(List<string> args)
        {
            if (args.Count == 1)
            {
                Log("Hello world!", "default");
            }
            else if (args.Count == 2)
            {
                Log("Hello, " + args[1], "default");
            }
            else 
            {
                LogError("Too many arguments!");
            }
        }
    }
}
