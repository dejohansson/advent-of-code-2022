open System.IO

[<EntryPoint>]
let main argv =
    let filename = Array.tryItem 0 argv |> Option.defaultValue "./input.txt"
    let lines = File.ReadAllLines filename
    printfn $"Read {lines.Length} lines from {filename}\n"

    printfn $"Part 1:"

    //printfn ""
    //printfn $"Part 2:"

    0
