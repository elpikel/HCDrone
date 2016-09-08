using System.Threading;
using AR.Drone.Client;
using AR.Drone.Client.Command;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var droneClient = new DroneClient("192.168.1.1");

            SleepAndPaint(droneClient);

            droneClient.Start();

            SleepAndPaint(droneClient);

            droneClient.Takeoff();

            SleepAndPaint(droneClient);

            
            droneClient.Progress(FlightMode.Progressive, yaw: -0.05f);

            System.Console.WriteLine("Yaw -0.05");
            SleepAndPaint(droneClient);

            droneClient.Hover();
            SleepAndPaint(droneClient);

            droneClient.Land();

            SleepAndPaint(droneClient);

            droneClient.Stop();

            SleepAndPaint(droneClient);

            System.Console.ReadLine();

            droneClient.Dispose();
        }

        private static void SleepAndPaint(DroneClient droneClient)
        {
            System.Console.WriteLine(droneClient.NavigationData.State);
            Thread.Sleep(10000);
            System.Console.WriteLine(droneClient.NavigationData.State);
        }
    }
}
