﻿open System.IO

[<EntryPoint>]
let main argv =
    let filename = Array.tryItem 0 argv |> Option.defaultValue "./input.txt"
    let lines = File.ReadAllLines filename
    printfn $"Read {lines.Length} lines from {filename}\n"

    let sum = lines |> Seq.map System.Int32.Parse |> Seq.sum

    printfn $"Part 1: {sum}"

    //printfn ""
    //printfn $"Part 2:"

    0
