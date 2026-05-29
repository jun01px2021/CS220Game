module CS220Game.Program

open System
open CS220Game.Board

type GameMode =
    | Twos
    | Threes
    | Fibonacci
    | Prime

[<EntryPoint>]
let main _ =
    printfn "Multi-Mode 2048"
    printfn "1. 2's Mode"
    printfn "2. 3's Mode"
    printfn "3. Fibonacci Mode"
    printfn "4. Prime Mode"

    let choice = Console.ReadLine()

    let mode =
        match choice with
        | "1" -> Twos
        | "2" -> Threes
        | "3" -> Fibonacci
        | "4" -> Prime
        | _ -> Twos

    printfn "Selected mode: %A" mode
    printfn "Game implementation coming next milestone."
    0
