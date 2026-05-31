module CS220Game.Board

open System

type Board = int list list

type Direction =
    | Left
    | Right
    | Up
    | Down

let private rng = Random()

let create size : Board =
    List.init size (fun _ -> List.init size (fun _ -> 0))

let size (board: Board) =
    List.length board

let emptyCells (board: Board) =
    board
    |> List.mapi (fun rowIndex row ->
        row
        |> List.mapi (fun colIndex value ->
            if value = 0 then Some (rowIndex, colIndex)
            else None))
    |> List.concat
    |> List.choose id

let setCell row col value (board: Board) : Board =
    board
    |> List.mapi (fun rowIndex currentRow ->
        if rowIndex <> row then currentRow
        else
            currentRow
            |> List.mapi (fun colIndex cell ->
                if colIndex = col then value
                else cell))

let spawnTileWithValue value (board: Board) : Board =
    let empties = emptyCells board

    match empties with
    | [] -> board
    | _ ->
        let randomIndex = rng.Next(List.length empties)
        let row, col = empties.[randomIndex]
        setCell row col value board

let spawnTilesWithValues values (board: Board) : Board =
    values
    |> List.fold (fun currentBoard value -> spawnTileWithValue value currentBoard) board

let transpose (board: Board) : Board =
    List.transpose board

let private padRight length values =
    values @ List.replicate (length - List.length values) 0

let private moveRowLeft (mergeNonZeroRow: int list -> int list * int) (row: int list) =
    let originalLength = List.length row
    let nonZero = row |> List.filter ((<>) 0)
    let merged, gainedScore = mergeNonZeroRow nonZero
    padRight originalLength merged, gainedScore

let moveLeft (mergeNonZeroRow: int list -> int list * int) (board: Board) : Board * int =
    board
    |> List.map (moveRowLeft mergeNonZeroRow)
    |> List.fold (fun (rows, score) (row, gained) -> rows @ [row], score + gained) ([], 0)

let moveRight (mergeNonZeroRow: int list -> int list * int) (board: Board) : Board * int =
    board
    |> List.map List.rev
    |> moveLeft mergeNonZeroRow
    |> fun (moved, score) -> moved |> List.map List.rev, score

let moveUp (mergeNonZeroRow: int list -> int list * int) (board: Board) : Board * int =
    board
    |> transpose
    |> moveLeft mergeNonZeroRow
    |> fun (moved, score) -> transpose moved, score

let moveDown (mergeNonZeroRow: int list -> int list * int) (board: Board) : Board * int =
    board
    |> transpose
    |> moveRight mergeNonZeroRow
    |> fun (moved, score) -> transpose moved, score

let move direction mergeNonZeroRow board =
    match direction with
    | Left -> moveLeft mergeNonZeroRow board
    | Right -> moveRight mergeNonZeroRow board
    | Up -> moveUp mergeNonZeroRow board
    | Down -> moveDown mergeNonZeroRow board

let boardChanged oldBoard newBoard =
    oldBoard <> newBoard

let formatCell value =
    if value = 0 then sprintf "%6s" "."
    else sprintf "%6d" value

let toDisplayString (board: Board) =
    board
    |> List.map (fun row ->
        row
        |> List.map formatCell
        |> String.concat "")
    |> String.concat "\n"

let print (board: Board) =
    printfn "%s" (toDisplayString board)
