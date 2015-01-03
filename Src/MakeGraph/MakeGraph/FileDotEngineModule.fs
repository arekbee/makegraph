module FileDotEngineModule

open QuickGraph.Graphviz
open System.IO
open System
open QuickGraph.Graphviz.Dot
open QuickGraph.Contracts
open QuickGraph.Collections
open QuickGraph
open System.Configuration
open FSharp.Configuration


let printfGraph (graph: AdjacencyGraph<_,_>)  = 
    for vertex in graph.Vertices do
        for edge in graph.OutEdges(vertex) do
            printfn "%A" edge 

type FileDotEngine()  =
    let pathToDotExeDir  = AppSettings<"app.config">.PathToDotExeDir

    interface IDotEngine with
        member x.Run(imageType: GraphvizImageType, dot:string , outputFileName:string ) : string = 
            File.WriteAllText(outputFileName, dot)
            let args = String.Format(@"{0} -Tjpg -O", outputFileName)
            let dorExePath = Path.Combine( pathToDotExeDir, "dot.exe")
            System.Diagnostics.Process.Start(dorExePath, args) |> ignore
            dot

let exportToDot (g: AdjacencyGraph<int,Edge<int>> )   (fileName: string option ) = 
    let graphviz = new GraphvizAlgorithm<int,Edge<int>>(g)
    match fileName  with 
    | Some(x) -> graphviz.Generate(new FileDotEngine(), x)
    | None -> graphviz.Generate()
        

