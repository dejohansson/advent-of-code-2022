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
        initState
        |> Array.map (fun (x: List<char>) -> x |> Seq.map (fun (y: char) -> y) |> List)

    let part1, part2 =
        Seq.fold
            (fun ((state1: List<char>[]), (state2: List<char>[])) (instr: string) ->
                let amount, src, dest =
                    match (instr.Split(' ')) with
                    | [| _; amount; _; src; _; dest |] ->
                        amount |> int, src |> int, dest |> int
                    | _ -> failwith ":("

                let crates = state1[ (src - 1) ].GetRange(0, amount)
                crates.Reverse()
                state1[ dest - 1 ].InsertRange(0, crates)
                state1[ src - 1 ].RemoveRange(0, amount)
                state2[ dest - 1 ].InsertRange(0, state2[ (src - 1) ].GetRange(0, amount))
                state2[ src - 1 ].RemoveRange(0, amount)
                (state1, state2))
            (initState, initState2)
            (strInstr.Split("\r\n"))
        |> (fun (s1, s2) ->
            (s1
             |> Seq.fold
                 (fun (res: string) (stack: List<char>) -> res + string stack[0])
                 ""),
            s2
            |> Seq.fold
                (fun (res: string) (stack: List<char>) -> res + string stack[0])
                "")

    printfn $"Part 1: {part1}"
    printfn $"Part 2: {part2}"
    stopWatch.Stop()
    printfn $"Completed in {stopWatch.Elapsed.TotalMilliseconds} ms"
    0
