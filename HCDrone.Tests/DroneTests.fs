﻿module DroneTests

open AR.Drone.Client
open Xunit
open System.Threading

[<Fact>]
let ``Should Take Off and Land`` () =
    use drone = new DroneClient("192.168.1.1")
    drone.Start()
    drone.Takeoff()
    Thread.Sleep(2000)
    drone.Land()
    drone.Stop()
    ()