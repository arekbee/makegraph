module GraphBuilderModule

open CmdSettingModule
open QuickGraph.Contracts
open QuickGraph.Collections
open QuickGraph
open System

[<AbstractClass>]
type AGraphBuilder() = 
    abstract Build : CmdSetting -> AdjacencyGraph<int,Edge<int>> 
    member  this.GetBaseGraph(cmdSetting :CmdSetting) =
        let g = new AdjacencyGraph<int,Edge<int>> ()
        for i =0 to cmdSetting.V do
                g.AddVertex(i) |>ignore
        g

// #region IGraphBuilder classes:
type RandomGraphBuilder() =
    inherit  AGraphBuilder()
    override this.Build(cmdSetting) =
        let g = this.GetBaseGraph(cmdSetting)
        let rdn  = new Random ()
        for i = 0  to cmdSetting.E do
            let fromV = i  % (cmdSetting.V + 1)
            let toV =  rdn .Next(0, cmdSetting.V)
            g.AddEdge(new Edge<int>(fromV,  toV)) |>ignore
        g

   
type OneLineGraphBuilder() = 
    inherit  AGraphBuilder()
    override this.Build(cmdSetting) =
        let g = this.GetBaseGraph(cmdSetting)
        for i = 0  to cmdSetting.E do
            let fromV = i  % cmdSetting.V 
            let toV =  (fromV  + 1 ) % (cmdSetting.V + 1) 
            g.AddEdge(new Edge<int>(fromV,  toV)) |>ignore
        g

type BiTreeGraphBuilder() = 
    inherit  AGraphBuilder()
    override this.Build(cmdSetting) =
        let g = this.GetBaseGraph(cmdSetting)
        let mutable fromCounter = 0
        let mutable toCounter = 1
        let mutable currentNodeAddedElements = 0

        for i = 0  to Math.Min(cmdSetting.E, cmdSetting.V - 1)  do
             if (currentNodeAddedElements >= 2 ) then
                currentNodeAddedElements <- 0
                fromCounter  <- fromCounter + 1

             g.AddEdge(new Edge<int>(fromCounter,  toCounter)) |>ignore 
             currentNodeAddedElements  <- currentNodeAddedElements  + 1
             toCounter <- toCounter + 1
        g


type CompleteGraphBuilder() = 
    inherit  AGraphBuilder()
        override this.Build(cmdSetting) =
        let g = this.GetBaseGraph(cmdSetting)
        for i = 0  to cmdSetting.V  do
            for j = 0  to cmdSetting.V  do
                if i<>j then
                    g.AddEdge(new Edge<int>(i,  j)) |>ignore 

        g

// #endregion 



let (|Contains |_|) (wildcard :string) (str :string) =
    if str.Trim().ToLowerInvariant().Contains(wildcard.Trim().ToLowerInvariant()) then Some(str)
    else None

let getGraphBuilder (className:string)  =    
   match className with
        | Contains "Random" str-> new RandomGraphBuilder() :>  AGraphBuilder
        | Contains "OneLine" str-> new OneLineGraphBuilder() :>  AGraphBuilder
        | Contains "BiTree" str-> new BiTreeGraphBuilder() :>  AGraphBuilder
        | Contains "Complete" str-> new CompleteGraphBuilder() :>  AGraphBuilder
        | _ -> new RandomGraphBuilder() :>  AGraphBuilder