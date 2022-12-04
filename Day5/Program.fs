open System.IO

[<EntryPoint>]
let main argv =
    let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    let filename = Array.tryItem 0 argv |> Option.defaultValue "./input.txt"
    let lines = File.ReadAllLines filename
    printfn $"Read {lines.Length} lines from {filename}\n"
    printfn $"Part 1:"
    printfn $"Part 2:"
    stopWatch.Stop()
    printfn $"Completed in {stopWatch.Elapsed.TotalMilliseconds} ms"
    0
