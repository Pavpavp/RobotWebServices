# ABB Robot Web Services C# wrapper

 - Only RobotWare 6 is currently supported
 - Many but not all calls are yet included 
 - Planning to add support for RobotWare 7 & subscriptions
 
 [Robot Web Services API reference](http://developercenter.robotstudio.com/blobproxy/devcenter/Robot_Web_Services/html/index.html)

## Change log

*2019 - 11 - 22*  
Added support for IO subscriptions. Try it as such:

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
