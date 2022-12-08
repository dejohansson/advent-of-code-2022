open System.IO
open System.Collections.Generic
open System

type Directory =
    { mutable Root: Directory
      mutable Parent: Directory
      mutable Directories: Dictionary<string, Directory>
      mutable Files: Dictionary<string, int> }

[<EntryPoint>]
let main argv =
    let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    let filename = Array.tryItem 0 argv |> Option.defaultValue "./input.txt"

    let commands =
        (File.ReadAllText filename).Split('$', StringSplitOptions.RemoveEmptyEntries)

    let rec rootDir =
        { Root = rootDir
          Parent = rootDir
          Directories = new Dictionary<string, Directory>()
          Files = new Dictionary<string, int>() }

    Seq.fold
        (fun (currentDir: Directory) (command: string) ->
            let a =
                match command.Trim() with
                | c when c.StartsWith("cd") ->
                    match c[3..] with
                    | "/" -> currentDir.Root
                    | ".." -> currentDir.Parent
                    | dir -> currentDir.Directories[dir]
                | c when c.StartsWith("ls") ->
                    Seq.fold
                        (fun (x: Directory) (z: string) ->
                            match z.Split() with
                            | o when o[0] = "dir" ->
                                (x.Directories.Add(
                                    o[1],
                                    { Root = currentDir.Root
                                      Parent = currentDir
                                      Directories = new Dictionary<string, Directory>()
                                      Files = new Dictionary<string, int>() }
                                ))
                            | o -> (x.Files.Add(o[1], o[0] |> int))

                            x)
                        currentDir
                        (c.Split("\r\n")[1..])
                | _ -> failwith $"Unknown command {command}"

            a)
        rootDir
        commands
    |> ignore

    let rec getSize (dir: Directory) =
        let fileSize = dir.Files.Values |> Seq.sum

        let (res, childTotalSize, childAllSizes) =
            dir.Directories.Values
            |> Seq.fold
                (fun (res, tot, allTot) (dir: Directory) ->
                    let (res2, tot2, allTot2) = getSize dir
                    (res + res2, tot + tot2, Array.concat [| allTot; allTot2 |]))
                (0, 0, Array.empty)

        let totalSize = fileSize + childTotalSize

        if totalSize > 100000 then
            (res, totalSize, Array.concat [| [| totalSize |]; childAllSizes |])
        else
            (totalSize + res, totalSize, Array.concat [| [| totalSize |]; childAllSizes |])

    let part1, totalSize, allSizes = getSize rootDir
    let minDeleted = totalSize - 40000000
    let part2 = allSizes |> Seq.filter (fun x -> x >= minDeleted) |> Seq.min
    printfn $"Part 1: {part1}"
    printfn $"Part 2: {part2}"
    stopWatch.Stop()
    printfn $"Completed in {stopWatch.Elapsed.TotalMilliseconds} ms"
    0
