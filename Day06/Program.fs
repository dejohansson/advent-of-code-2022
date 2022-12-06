open System.IO

[<EntryPoint>]
let main argv =
    let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    let filename = Array.tryItem 0 argv |> Option.defaultValue "./input.txt"
    let datastream = File.ReadAllText filename

    let findUniqueSequence datastream length =
        (datastream
         |> Seq.windowed length
         |> Seq.tryFindIndex (fun x -> x |> Seq.distinct |> Seq.length = length))
            .Value
        + length

    printfn $"Part 1: {findUniqueSequence datastream 4}"
    printfn $"Part 2: {findUniqueSequence datastream 14}"
    stopWatch.Stop()
    printfn $"Completed in {stopWatch.Elapsed.TotalMilliseconds} ms"
    0
