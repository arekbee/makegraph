
open System
open FileDotEngineModule
open CmdSettingModule
open GraphBuilderModule

let buildGraph (cmdSetting :CmdSetting) =
    let graphBuilder =  getGraphBuilder cmdSetting.ClassName
    graphBuilder.Build cmdSetting


[<EntryPoint>]
let main argv = 

    let cmdSetting,_,_ = getCmdSetting argv
    if( not cmdSetting.help ) then
        let g = buildGraph  cmdSetting

        let result = exportToDot g (Some "graph.dot")
        printfn "%A"  result

        //printfGraph g
    
    0 // return an integer exit code
