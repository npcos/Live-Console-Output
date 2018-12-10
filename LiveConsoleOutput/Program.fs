// Learn more about F# at http://fsharp.org

open System
open System.Diagnostics
open System.IO
open System.Reactive.Linq
open RxFileSystemWatcher

let watchApp projectExe (args:string list) =
    Console.Clear()
    printf "Watching console output for %s\n" projectExe

    let argStr = String.Join (" ", args)
    let p = Process.Start (projectExe, argStr)

    let dir = Path.GetDirectoryName projectExe
    use fsw = new FileSystemWatcher(dir)
    use rxFsw = new ObservableFileSystemWatcher(fsw)
    fsw.EnableRaisingEvents <- true

    let onNext (lastRun:Process) _ =
        Console.Clear()
        printf "Detected change! Running again...\n"

        if not lastRun.HasExited then lastRun.Kill()
        Process.Start (projectExe, argStr)
    
    let toUnits (obs:IObservable<'a>) = obs.Select(fun _ -> ())

    (toUnits rxFsw.Changed)
        .Merge(toUnits rxFsw.Created)
        .Merge(toUnits rxFsw.Deleted)
        .Merge(toUnits rxFsw.Renamed)
        .Throttle(TimeSpan.FromMilliseconds 500.0)
        .Scan(p, Func<_,_,_> onNext)
        .Wait() |> ignore
    
[<EntryPoint>]
let main argv =
    match argv |> List.ofArray with
    | [] ->
        printf "Please provide an executable file to watch"
        1
    | exe::args ->
        watchApp exe args
        0