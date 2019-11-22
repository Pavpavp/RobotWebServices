# ABB Robot Web Services C# wrapper for RobotWare6
Many but not all calls are yet included
Planning to add support for RobotWare 7 & subscriptions



## Change log

2019 - 11 - 22  
Added support for subscribing to IOs. Try it as such:

    class TestProgram
    {
        static void Main(string[] args)
        {
            ControllerSession rwsCs1 = new ControllerSession("localhost");
            var ios = rwsCs1.RobotWareService.GetIOSignals().Embedded.State;

            var io0 = ios[0];

            io0.OnValueChanged += IOSignal_ValueChanged;
            Console.ReadKey();

            io0.OnValueChanged -= IOSignal_ValueChanged;
            Console.ReadKey();
    }
      
    private static void IOSignal_ValueChanged(object sender, IOEventArgs args)
    {
        var lvalue = args.LValue;
    }