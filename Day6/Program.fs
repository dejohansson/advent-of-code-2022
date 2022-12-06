open System.IO

[<EntryPoint>]
let main argv =
    let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    let filename = Array.tryItem 0 argv |> Option.defaultValue "./input.txt"
    let datastream = File.ReadAllText filename

    let part1 =
        (datastream
         |> Seq.windowed 4
         |> Seq.tryFindIndex (fun x -> x |> Seq.distinct |> Seq.length = 4))
            .Value
        + 4

    let part2 =
        (datastream
         |> Seq.windowed 14
         |> Seq.tryFindIndex (fun x -> x |> Seq.distinct |> Seq.length = 14))
            .Value
        + 14

    printfn $"Part 1: {part1}"
    printfn $"Part 2: {part2}"
    stopWatch.Stop()
    printfn $"Completed in {stopWatch.Elapsed.TotalMilliseconds} ms"
    0
