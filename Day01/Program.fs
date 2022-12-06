open System.IO

[<EntryPoint>]
let main argv =
    let filename = Array.tryItem 0 argv |> Option.defaultValue "./input.txt"
    let elves: string[] = (File.ReadAllText filename).Split("\r\n\r\n")
    printfn $"Read {elves.Length} lines from {filename}\n"

    let cals =
        elves
        |> Array.map (fun e -> e.Split("\r\n") |> Array.map (fun r -> r |> int) |> Array.sum)

    let maxCals = cals |> Array.max

    printfn $"Part 1: {maxCals}"

    let topThreeCals = cals |> Array.sortDescending |> Array.take 3 |> Array.sum

    printfn ""
    printfn $"Part 2: {topThreeCals}"

    0
