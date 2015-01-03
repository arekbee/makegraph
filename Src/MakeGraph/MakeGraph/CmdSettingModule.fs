module CmdSettingModule
open NDesk.Options //http://www.ndesk.org/doc/ndesk-options/NDesk.Options/OptionSet.html
open System


 // #region cmd parsing

 type CmdSetting() = 
    [<DefaultValue>]val mutable V  :int 
    [<DefaultValue>]val mutable E :int
    [<DefaultValue>]val mutable help :bool
    [<DefaultValue>]val mutable outputFile  :string
    [<DefaultValue>]val mutable LabelEdge :List<string>
    [<DefaultValue>]val mutable ClassName :string

let showHelp (os:OptionSet) =
    os.WriteOptionDescriptions(Console.Out)

let parseToList (str :string) :List<string>= 
    str.Replace("{","").Replace("}","").Split([|','; ':'; ';'|] , StringSplitOptions.RemoveEmptyEntries) 
        |> Array.toList
    
let getCmdSetting argv = 
    let cmdSetting = new CmdSetting()
    let p = OptionSet()
    [
        "V=", "Number of vertices in graph" ,  fun (x :string)-> cmdSetting.V <- int(x)  ; 
        "E=", "Number of edges in graph" , fun x-> cmdSetting.E <- int(x) ;
        "c=|class=", "Building class name", fun x-> cmdSetting.ClassName <- x;
        "h|help|?", "", fun x -> cmdSetting.help <- true ;
        "o=|output=", "Output file in dot format",  fun x -> cmdSetting.outputFile <- x ;
        "L=|LabelEdge=", "",  fun x -> cmdSetting.LabelEdge <- parseToList(x) ;
    ] 
    |> List.map (fun (x1,x2,x3) -> p.Add(x1 , x2, x3))  |> ignore
    
    if cmdSetting.help then 
        showHelp p
    
    (cmdSetting, p, p.Parse(argv))

// #endregion 