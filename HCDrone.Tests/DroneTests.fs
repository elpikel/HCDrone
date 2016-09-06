module DroneTests

open AR.Drone.Client
open Xunit
open FsUnit.Xunit
open System.Threading

[<Fact>]
let ``Should Take Off and Land`` () =
    use drone = new DroneClient("192.168.1.1")
    
    drone.Start()
    drone.Takeoff()
    
    Thread.Sleep(2000)
    
    drone.Land()
    drone.Stop()
    
    1 |> should equal 1

[<Fact>]
let ``Should fly to next room -> fly back``() =

    1 |> should equal 2