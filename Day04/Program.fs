open System.IO
let add a b = fst a + fst b, snd a + snd b

[<EntryPoint>]
let main argv =
    let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    let filename = Array.tryItem 0 argv |> Option.defaultValue "./input.txt"
    let pairs = File.ReadAllLines filename
    printfn $"Read {pairs.Length} lines from {filename}\n"

    let res =
        Seq.fold
            (fun res (pair: string) ->
                let s1, s2, s3, s4 =
                    match
                        pair.Split ','
                        |> Array.map (fun elf -> elf.Split '-' |> Array.map int)
                    with
                    | [| [| s1; s2 |]; [| s3; s4 |] |] -> s1, s2, s3, s4
                    | _ -> failwith ":("

                (if (s1 <= s3 && s2 >= s4) || (s3 <= s1 && s4 >= s2) then
                     (1, 1)
                 else if (s1 <= s3 && s2 >= s3) || (s1 <= s4 && s2 >= s4) then
                     (0, 1)
                 else
                     (0, 0))
                |> add res)
            (0, 0)
            pairs

    printfn $"Part 1: {fst res}"
    printfn $"Part 2: {snd res}"
    stopWatch.Stop()
    printfn $"Completed in {stopWatch.Elapsed.TotalMilliseconds} ms"
    0
