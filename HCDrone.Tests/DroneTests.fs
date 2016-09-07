module DroneTests

open AR.Drone.Client
open AR.Drone.Client.Command
open AR.Drone.Data
open AR.Drone.Data.Navigation
open Xunit
open FsUnit.Xunit
open System
open System.Threading
open System.IO
open Microsoft.FSharp.Core
open System.Windows.Input

let waitFor (navigationState) (droneClient:DroneClient) =
    while droneClient.NavigationData.State = navigationState |> not do
        Thread.Sleep(1000)

let runCommand command (state:NavigationState) (drone:DroneClient) =
    command()
    (state, drone) ||> waitFor
    drone

let start (drone:DroneClient) = 
    runCommand drone.Start (NavigationState.Landed ||| NavigationState.Command) drone

let takeOff (drone:DroneClient) =
    runCommand drone.Takeoff (NavigationState.Flying ||| NavigationState.Hovering ||| NavigationState.Command) drone

let landDrone (drone:DroneClient) =
    runCommand drone.Land (NavigationState.Landed ||| NavigationState.Command) drone

let stop (drone:DroneClient) =
    runCommand drone.Stop (NavigationState.Landed ||| NavigationState.Command) drone
    
let hover (drone:DroneClient) =
    runCommand drone.Hover (NavigationState.Flying ||| NavigationState.Hovering ||| NavigationState.Command) drone

let rotate (drone:DroneClient) =
    let droneTurn () = drone.Progress(FlightMode.Progressive, 0.00F, 0.0F, -0.25F, 0.0F)
        
    runCommand droneTurn (NavigationState.Flying ||| NavigationState.Hovering ||| NavigationState.Command) drone

let wait (time:int) drone = 
    Thread.Sleep(time)
    drone

let onKeyDown (key, command) =
    if Keyboard.GetKeyStates(key) = KeyStates.Down then command()

let bindDroneFlightControlls drone =
    (Key.PageUp, (fun () -> drone |> landDrone |> stop |> ignore)) |> onKeyDown 
    (Key.PageDown, (fun () -> drone |> start |> takeOff |> ignore)) |> onKeyDown 
    

[<Fact>]
let ``Should Take Off and Land`` () =
    let drone = new DroneClient("192.168.1.1") 
                    |> start
                    |> takeOff
                    |> wait 5000
                    |> landDrone
                    |> stop
    
    drone.Dispose ()

[<Fact>]
let ``Should be able to rotate``() =
    let drone = new DroneClient("192.168.1.1") 
                    |> start
                    |> takeOff
                    |> wait 2000
                    |> rotate
                    |> wait 3000
                    |> hover
                    |> wait 2000
                    |> landDrone
                    |> stop
    
    drone.Dispose ()

[<WpfFact>]
let ``Should read command from keyboard``() =
    let drone = new DroneClient("192.168.1.1")

    while Keyboard.GetKeyStates(Key.Escape) = KeyStates.None do
        if Keyboard.GetKeyStates(Key.PageUp) = KeyStates.Down then
            drone |> start |> takeOff |> ignore
        if Keyboard.GetKeyStates(Key.PageDown) = KeyStates.Down then
            drone |> landDrone |> stop |> ignore

    drone.Dispose()