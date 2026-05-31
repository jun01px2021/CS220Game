module CS220Game.Program

open System
open CS220Game.Board

type GameMode =
    | Twos
    | Threes
    | Fibonacci
    | Prime

type ModeSpec =
    { Name: string
      BoardSize: int
      Goal: int
      InitialSpawnCount: int
      MergeNonZeroRow: int list -> int list * int
      SpawnValues: Board -> int list }

let rng = Random()

let chooseWeighted (items: (int * int) list) =
    let totalWeight = items |> List.sumBy snd
    let pick = rng.Next(totalWeight)

    let rec loop acc remaining =
        match remaining with
        | [] -> fst items.[0]
        | (value, weight) :: rest ->
            let next = acc + weight
            if pick < next then value
            else loop next rest

    loop 0 items

let rec mergePairs canMerge combine row =
    match row with
    | a :: b :: rest when canMerge a b ->
        let mergedValue = combine a b
        let mergedRest, scoreRest = mergePairs canMerge combine rest
        mergedValue :: mergedRest, mergedValue + scoreRest
    | x :: rest ->
        let mergedRest, scoreRest = mergePairs canMerge combine rest
        x :: mergedRest, scoreRest
    | [] -> [], 0

let rec mergeTriples row =
    match row with
    | a :: b :: c :: rest when a = b && b = c ->
        let mergedValue = a * 3
        let mergedRest, scoreRest = mergeTriples rest
        mergedValue :: mergedRest, mergedValue + scoreRest
    | x :: rest ->
        let mergedRest, scoreRest = mergeTriples rest
        x :: mergedRest, scoreRest
    | [] -> [], 0

let isPrime n =
    if n < 2 then false
    else
        let bound = int (sqrt (float n))
        seq { 2 .. bound }
        |> Seq.forall (fun divisor -> n % divisor <> 0)

let fibonacciNumbers =
    let rec build a b acc =
        if a > 3000 then List.rev acc
        else build b (a + b) (a :: acc)

    build 1 1 [] |> List.distinct

let isFibonacciPair a b =
    (a = 1 && b = 1)
    || (fibonacciNumbers |> List.pairwise |> List.exists (fun (x, y) -> (a = x && b = y) || (a = y && b = x)))

let containsAtLeast value board =
    board |> List.exists (List.exists (fun cell -> cell >= value))

let containsGreaterThan value board =
    board |> List.exists (List.exists (fun cell -> cell > value))

let spawnOne weightedItems =
    [ chooseWeighted weightedItems ]

let spawnOneOrTwo chanceForTwo weightedItems =
    let count = if rng.NextDouble() < chanceForTwo then 2 else 1
    List.init count (fun _ -> chooseWeighted weightedItems)

let getModeSpec mode =
    match mode with
    | Twos ->
        { Name = "2048 (2's)"
          BoardSize = 4
          Goal = 2048
          InitialSpawnCount = 2
          MergeNonZeroRow = mergePairs (=) (+)
          SpawnValues =
            fun board ->
                if containsAtLeast 32 board then spawnOne [ 2, 60; 4, 40 ]
                else spawnOne [ 2, 90; 4, 10 ] }
    | Threes ->
        { Name = "2187 (3's)"
          BoardSize = 6
          Goal = 2187
          InitialSpawnCount = 2
          MergeNonZeroRow = mergeTriples
          SpawnValues =
            fun board ->
                if containsAtLeast 81 board then spawnOneOrTwo 0.40 [ 1, 50; 3, 30; 9, 20 ]
                else spawnOne [ 1, 70; 3, 30 ] }
    | Fibonacci ->
        { Name = "2584 (Fibonacci)"
          BoardSize = 4
          Goal = 2584
          InitialSpawnCount = 2
          MergeNonZeroRow = mergePairs isFibonacciPair (+)
          SpawnValues =
            fun board ->
                if containsAtLeast 89 board then spawnOne [ 1, 70; 2, 30 ]
                else spawnOne [ 1, 90; 2, 10 ] }
    | Prime ->
        { Name = "199 (Prime)"
          BoardSize = 5
          Goal = 199
          InitialSpawnCount = 2
          MergeNonZeroRow = mergePairs (fun a b -> a = b || isPrime (a + b)) (+)
          SpawnValues =
            fun board ->
                if containsGreaterThan 20 board then spawnOne [ 2, 50; 3, 30; 5, 20 ]
                else spawnOne [ 2, 70; 3, 30 ] }

let initializeBoard spec =
    let initialValues =
        List.init spec.InitialSpawnCount (fun _ ->
            spec.SpawnValues (create spec.BoardSize) |> List.head)

    create spec.BoardSize
    |> spawnTilesWithValues initialValues

let hasWon spec board =
    board |> List.exists (List.exists (fun cell -> cell >= spec.Goal))

let canMove spec board =
    [ Left; Right; Up; Down ]
    |> List.exists (fun direction ->
        let movedBoard, _ = move direction spec.MergeNonZeroRow board
        boardChanged board movedBoard)

let draw spec board score =
    Console.Clear()
    printfn "%s" spec.Name
    printfn "Score: %d    Goal: %d" score spec.Goal
    printfn "Use arrow keys or WASD to move. Press Q to quit."
    printfn ""
    print board
    printfn ""

let rec readDirection () =
    let key = Console.ReadKey(true).Key

    match key with
    | ConsoleKey.LeftArrow | ConsoleKey.A -> Some Left
    | ConsoleKey.RightArrow | ConsoleKey.D -> Some Right
    | ConsoleKey.UpArrow | ConsoleKey.W -> Some Up
    | ConsoleKey.DownArrow | ConsoleKey.S -> Some Down
    | ConsoleKey.Q -> None
    | _ -> readDirection ()

let rec play spec board score =
    draw spec board score

    if hasWon spec board then
        printfn "You win! You reached %d." spec.Goal
        0
    elif not (canMove spec board) then
        printfn "Game over! No valid moves remain."
        0
    else
        match readDirection () with
        | None ->
            printfn "Quit game."
            0
        | Some direction ->
            let movedBoard, gainedScore = move direction spec.MergeNonZeroRow board

            if not (boardChanged board movedBoard) then
                play spec board score
            else
                let spawnedBoard =
                    movedBoard
                    |> spawnTilesWithValues (spec.SpawnValues movedBoard)

                play spec spawnedBoard (score + gainedScore)

let selectMode () =
    Console.Clear()
    printfn "Multi-Mode 2048"
    printfn "Choose a game mode:"
    printfn "1. 2's Mode"
    printfn "2. 3's Mode"
    printfn "3. Fibonacci Mode"
    printfn "4. Prime Mode"
    printf "Enter 1, 2, 3, or 4: "

    match Console.ReadLine() with
    | "1" -> Twos
    | "2" -> Threes
    | "3" -> Fibonacci
    | "4" -> Prime
    | _ -> Twos

[<EntryPoint>]
let main _ =
    let mode = selectMode ()
    let spec = getModeSpec mode
    let board = initializeBoard spec
    play spec board 0
