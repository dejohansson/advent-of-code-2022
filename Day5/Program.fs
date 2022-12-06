open System.IO
open System.Collections.Generic

[<EntryPoint>]
let main argv =
    let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    let filename = Array.tryItem 0 argv |> Option.defaultValue "./input.txt"

    let strState, strInstr =
        match (File.ReadAllText filename).Split "\r\n\r\n" with
        | [| strState; strInstr |] -> strState, strInstr
        | _ -> failwith ":("

    let initState =
        strState.Split("\r\n")[..^1]
        |> Array.map (fun r ->
            Seq.chunkBySize 4 r[1..] |> Seq.toArray |> Array.map (fun x -> x[0]))
        |> array2D
        |> (fun grid ->
            Array.init (Array2D.length2 grid) (fun column ->
                grid[*, column] |> Array.filter ((<>) ' ') |> List))

    let initState2 =
        strState.Split("\r\n")[..^1]
        |> Array.map (fun r ->
            Seq.chunkBySize 4 r[1..] |> Seq.toArray |> Array.map (fun x -> x[0]))
        |> array2D
        |> (fun grid ->
            Array.init (Array2D.length2 grid) (fun column ->
                grid[*, column] |> Array.filter ((<>) ' ') |> List))

    let part1 =
        Seq.fold
            (fun (state: List<char>[]) (instr: string) ->
                let amount, src, dest =
                    match (instr.Split(' ')) with
                    | [| _; amount; _; src; _; dest |] ->
                        amount |> int, src |> int, dest |> int
                    | _ -> failwith ":("

                let crates = state[ (src - 1) ].GetRange(0, amount)
                crates.Reverse()
                state[ dest - 1 ].InsertRange(0, crates)
                state[ src - 1 ].RemoveRange(0, amount)
                state)
            initState
            (strInstr.Split("\r\n"))
        |> Seq.fold (fun (res: string) (stack: List<char>) -> res + string stack[0]) ""

    let part2 =
        Seq.fold
            (fun (state: List<char>[]) (instr: string) ->
                let amount, src, dest =
                    match (instr.Split(' ')) with
                    | [| _; amount; _; src; _; dest |] ->
                        amount |> int, src |> int, dest |> int
                    | _ -> failwith ":("

                state[ dest - 1 ].InsertRange(0, state[ (src - 1) ].GetRange(0, amount))
                state[ src - 1 ].RemoveRange(0, amount)
                state)
            initState2
            (strInstr.Split("\r\n"))
        |> Seq.fold (fun (res: string) (stack: List<char>) -> res + string stack[0]) ""

    printfn $"Part 1: {part1}"
    printfn $"Part 2: {part2}"
    stopWatch.Stop()
    printfn $"Completed in {stopWatch.Elapsed.TotalMilliseconds} ms"
    0
