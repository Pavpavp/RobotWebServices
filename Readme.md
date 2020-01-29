# ABB Robot Web Services C# wrapper

 - Many but not all calls are yet included 
 - Planning to add support for RobotWare 7 & subscriptions
 
[Robot Web Services API reference](http://developercenter.robotstudio.com/blobproxy/devcenter/Robot_Web_Services/html/index.html)
## Change log

*2020 - 01 - 29*  
The project is now targetting .NET Core and .NET 4.8 so the PCSDK can be used for discovery. The OmniCore controller is now also supported but so far you can only get IOs and subscribe to valueChanged, see below example.

    //This example works if you have 2 virtual controllers running, one with RW6 and one with 	 
    //RW7 and both having a DO signal called "doSigTest"
    var scanner = new NetworkScanner();
    scanner.Scan();
    ControllerInfoCollection controllers = scanner.Controllers;
    var vc7 = controllers.FirstOrDefault(c => c.IsVirtual && c.VersionName.Contains("7."));
    var vc6 = controllers.FirstOrDefault(c => c.IsVirtual && c.VersionName.Contains("6."));
    
    var rwsCs7 = new OmniCoreSession(new Address($"{vc7.IPAddress}:{vc7.WebServicesPort}"));
    var ios7 = await rwsCs7.RobotWareService.GetIOSignalsAsync();
    var io7 = ios7.Embedded.Resources.FirstOrDefault(io => io.Name.Contains("doSigTest"));
    
    var rwsCs6 = new IRC5Session(new Address($"{vc6.IPAddress}:{vc6.WebServicesPort}"));
    var ios6 = await rwsCs6.RobotWareService.GetIOSignalsAsync();
    var io6 = ios6.Embedded.State.FirstOrDefault(io => io.Name.Contains("doSigTest"));
    
    io6.OnValueChanged += IOSignal_ValueChanged;
    io7.OnValueChanged += IOSignal_ValueChanged;


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