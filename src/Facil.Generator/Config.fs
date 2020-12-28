[<AutoOpen>]
module Facil.Config

open System.IO
open System.Text.RegularExpressions
open GlobExpressions


type IncludeOrFor =
  | Include of pattern: string
  | For of pattern: string


type ResultKind =
  | Auto
  | AnonymousRecord
  | Custom of string


[<CLIMutable>]
type ConfigSourceDto = {
  appSettings: string option
  userSecrets: string option
  envVars: string option
}


[<CLIMutable>]
type TableDtoColumnDto = {
  skip: bool option
}


type TableDtoColumn = {
  Skip: bool option
}


[<CLIMutable>]
type TableDtoRuleDto = {
  ``include``: string option
  ``for``: string option
  except: string option
  voption: bool option
  columns: Map<string, TableDtoColumnDto> option
}


type TableDtoRule = {
  IncludeOrFor: IncludeOrFor
  Except: string option
  Voption: bool option
  Columns: Map<string, TableDtoColumn>
}


type EffectiveTableDtoRule = {
  Voption: bool
  Columns: Map<string, TableDtoColumn>
}

[<CLIMutable>]
type TableTypeRuleDto = {
  ``for``: string option
  except: string option
  voption: bool option
  skipParamDto: bool option
}


type TableTypeRule = {
  For: string
  Except: string option
  Voption: bool option
  SkipParamDto: bool option
}


type EffectiveTableTypeRule = {
  Voption: bool
  SkipParamDto: bool
}


[<CLIMutable>]
type ProcedureParameterDto = {
  dtoName: string option
}


type ProcedureParameter = {
  DtoName: string option
}


[<CLIMutable>]
type ProcedureColumnDto = {
  skip: bool option
}


type ProcedureColumn = {
  Skip: bool option
}


[<CLIMutable>]
type ProcedureRuleDto = {
  ``include``: string option
  ``for``: string option
  except: string option
  result: string option
  voptionIn: bool option
  voptionOut: bool option
  recordIfSingleCol: bool option
  skipParamDto: bool option
  useReturnValue: bool option
  ``params``: Map<string, ProcedureParameterDto> option
  columns: Map<string, ProcedureColumnDto> option
}


type ProcedureRule = {
  IncludeOrFor: IncludeOrFor
  Except: string option
  Result: ResultKind option
  VoptionIn: bool option
  VoptionOut: bool option
  RecordIfSingleCol: bool option
  SkipParamDto: bool option
  UseReturnValue: bool option
  Parameters: Map<string, ProcedureParameter>
  Columns: Map<string, ProcedureColumn>
}


type EffectiveProcedureRule = {
  Result: ResultKind
  VoptionIn: bool
  VoptionOut: bool
  RecordIfSingleCol: bool
  SkipParamDto: bool
  UseReturnValue: bool
  Parameters: Map<string, ProcedureParameter>
  Columns: Map<string, ProcedureColumn>
}


[<CLIMutable>]
type ScriptParameterDto = {
  nullable: bool option
  ``type``: string option
  dtoName: string option
}


type ScriptParameter = {
  Nullable: bool option
  Type: string option
  DtoName: string option
}

[<CLIMutable>]
type ScriptTempTableRuleDto = {
  definition: string
}


type ScriptTempTableRule = {
  Definition: string
}


[<CLIMutable>]
type ScriptColumnDto = {
  skip: bool option
}


type ScriptColumn = {
  Skip: bool option
}


[<CLIMutable>]
type ScriptRuleDto = {
  ``include``: string option
  ``for``: string option
  except: string option
  result: string option
  voptionIn: bool option
  voptionOut: bool option
  recordIfSingleCol: bool option
  skipParamDto: bool option
  ``params``: Map<string, ScriptParameterDto> option
  tempTables: ScriptTempTableRuleDto list option
  columns: Map<string, ScriptColumnDto> option
}


type ScriptRule = {
  IncludeOrFor: IncludeOrFor
  Except: string option
  IncludeMatches: Set<string>
  ForMatches: Set<string>
  Result: ResultKind option
  VoptionIn: bool option
  VoptionOut: bool option
  RecordIfSingleCol: bool option
  SkipParamDto: bool option
  Parameters: Map<string, ScriptParameter>
  TempTables: ScriptTempTableRule list option
  Columns: Map<string, ScriptColumn>
}


type EffectiveScriptRule = {
  Result: ResultKind
  VoptionIn: bool
  VoptionOut: bool
  RecordIfSingleCol: bool
  SkipParamDto: bool
  Parameters: Map<string, ScriptParameter>
  TempTables: ScriptTempTableRule list
  Columns: Map<string, ScriptColumn>
}


type ProcedureOrScriptParameter = {
  DtoName: string option
}


type ProcedureOrScriptColumn = {
  Skip: bool option
}


type EffectiveProcedureOrScriptRule = {
  Result: ResultKind
  VoptionIn: bool
  VoptionOut: bool
  RecordIfSingleCol: bool
  SkipParamDto: bool
  UseReturnValue: bool
  Parameters: Map<string, ProcedureOrScriptParameter>
  Columns: Map<string, ProcedureOrScriptColumn>
}


[<CLIMutable>]
type RuleSetDto = {
  connectionString: string option
  filename: string option
  namespaceOrModuleDeclaration: string option
  scriptBasePath: string option
  prelude: string option
  tableDtos: TableDtoRuleDto list option
  tableTypes: TableTypeRuleDto list option
  procedures: ProcedureRuleDto list option
  scripts: ScriptRuleDto list option
}


type RuleSet = {
  ConnectionString: string
  Filename: string
  NamespaceOrModuleDeclaration: string
  ScriptBasePath: string
  Prelude: string list option
  TableDtos: TableDtoRule list
  TableTypes: TableTypeRule list
  Procedures: ProcedureRule list
  Scripts: ScriptRule list
}


[<CLIMutable>]
type FacilConfigDto = {
  configs: ConfigSourceDto list option
  rulesets: RuleSetDto list option
}


module TableDtoColumn =


  let fromDto (dto: TableDtoColumnDto) : TableDtoColumn =
    {
      Skip = dto.skip
    }


  let merge (c1: TableDtoColumn) (c2: TableDtoColumn) : TableDtoColumn = {
    Skip = c2.Skip |> Option.orElse c1.Skip
  }


module TableDtoRule =


  let defaultEffectiveRule : EffectiveTableDtoRule = {
    Voption = false
    Columns = Map.empty
  }


  let fromDto fullYamlPath (dto: TableDtoRuleDto) : TableDtoRule = {
    IncludeOrFor =
      match dto.``include``, dto.``for`` with
      | None, None -> failwithYamlError fullYamlPath 0 0 "Either 'include' or 'for' is required in a tableDto rule"
      | Some inc, None -> Include inc
      | None, Some f -> For f
      | Some _, Some _ -> failwithYamlError fullYamlPath 0 0 "'include' and 'for' may not be combined in a tableDto rule"
    Except = dto.except
    Voption = dto.voption
    Columns =
      dto.columns
      |> Option.defaultValue Map.empty
      |> Map.map (fun _ -> TableDtoColumn.fromDto)
  }


  let matches schemaName tableName (rule: TableDtoRule) =
    let qualifiedName = $"%s{schemaName}.%s{tableName}"
    let pattern =
      match rule.IncludeOrFor with
      | For x -> x
      | Include x -> x
    match rule.Except with
    | None -> Regex.IsMatch(qualifiedName, pattern)
    | Some exPattern -> Regex.IsMatch(qualifiedName, pattern) && not <| Regex.IsMatch(qualifiedName, exPattern)


  let merge (eff: EffectiveTableDtoRule) (rule: TableDtoRule) : EffectiveTableDtoRule =
    {
      Voption = rule.Voption |> Option.defaultValue eff.Voption
      Columns =
        let colsToMerge =
          match rule.Columns.TryFind "" with
          | None -> rule.Columns
          | Some baseParam -> rule.Columns |> Map.map (fun _ param -> TableDtoColumn.merge baseParam param)
        Map.merge TableDtoColumn.merge eff.Columns colsToMerge
    }


module TableTypeRule =


  let defaultEffectiveRule : EffectiveTableTypeRule = {
    Voption = false
    SkipParamDto = false
  }


  let fromDto fullYamlPath (dto: TableTypeRuleDto) : TableTypeRule = {
    For =
      dto.``for``
      |> Option.defaultWith (fun () -> failwithYamlError fullYamlPath 0 0 "Either 'include' or 'for' is required in a tableType rule")
    Except = dto.except
    Voption = dto.voption
    SkipParamDto = dto.skipParamDto
  }


  let matches schemaName tableName (rule: TableTypeRule) =
    let qualifiedName = $"%s{schemaName}.%s{tableName}"
    match rule.Except with
    | None -> Regex.IsMatch(qualifiedName, rule.For)
    | Some exPattern -> Regex.IsMatch(qualifiedName, rule.For) && not <| Regex.IsMatch(qualifiedName, exPattern)


  let merge (eff: EffectiveTableTypeRule) (rule: TableTypeRule) : EffectiveTableTypeRule =
    {
      Voption = rule.Voption |> Option.defaultValue eff.Voption
      SkipParamDto = rule.SkipParamDto |> Option.defaultValue eff.SkipParamDto
    }


module ProcedureParameter =


  let fromDto (dto: ProcedureParameterDto) : ProcedureParameter = {
    DtoName = dto.dtoName
  }


  let merge (p1: ProcedureParameter) (p2: ProcedureParameter) : ProcedureParameter = {
    DtoName = p2.DtoName |> Option.orElse p1.DtoName
  }


module ProcedureColumn =


  let fromDto (dto: ProcedureColumnDto) : ProcedureColumn =
    {
      Skip = dto.skip
    }


  let merge (c1: ProcedureColumn) (c2: ProcedureColumn) : ProcedureColumn = {
    Skip = c2.Skip |> Option.orElse c1.Skip
  }


module ProcedureRule =


  let fromDto fullYamlPath (dto: ProcedureRuleDto) : ProcedureRule = {
    IncludeOrFor =
      match dto.``include``, dto.``for`` with
      | None, None -> failwithYamlError fullYamlPath 0 0 "Either 'include' or 'for' is required in a procedure rule"
      | Some inc, None -> Include inc
      | None, Some f -> For f
      | Some _, Some _ -> failwithYamlError fullYamlPath 0 0 "'include' and 'for' may not be combined in a procedure rule"
    Except = dto.except
    Result =
      dto.result
      |> Option.map (function
          | "auto" -> Auto
          | "anonymous" -> AnonymousRecord
          | name -> Custom name
      )
    VoptionIn = dto.voptionIn
    VoptionOut = dto.voptionOut
    RecordIfSingleCol = dto.recordIfSingleCol
    SkipParamDto = dto.skipParamDto
    UseReturnValue = dto.useReturnValue
    Parameters =
      dto.``params``
      |> Option.defaultValue Map.empty
      |> Map.map (fun _ -> ProcedureParameter.fromDto)
    Columns =
      dto.columns
      |> Option.defaultValue Map.empty
      |> Map.map (fun _ -> ProcedureColumn.fromDto)
  }


  let defaultEffectiveRule : EffectiveProcedureRule = {
    Result = Auto
    VoptionIn = false
    VoptionOut = false
    RecordIfSingleCol = false
    SkipParamDto = false
    UseReturnValue = false
    Parameters = Map.empty
    Columns = Map.empty
  }


  let matches schemaName procName (rule: ProcedureRule) =
    let qualifiedName = $"%s{schemaName}.%s{procName}"
    let pattern =
      match rule.IncludeOrFor with
      | For x -> x
      | Include x -> x
    match rule.Except with
    | None -> Regex.IsMatch(qualifiedName, pattern)
    | Some exPattern -> Regex.IsMatch(qualifiedName, pattern) && not <| Regex.IsMatch(qualifiedName, exPattern)


  let merge (eff: EffectiveProcedureRule) (rule: ProcedureRule) : EffectiveProcedureRule =
    {
      Result = rule.Result |> Option.defaultValue eff.Result
      VoptionIn = rule.VoptionIn |> Option.defaultValue eff.VoptionIn
      VoptionOut = rule.VoptionOut |> Option.defaultValue eff.VoptionOut
      RecordIfSingleCol = rule.RecordIfSingleCol |> Option.defaultValue eff.RecordIfSingleCol
      SkipParamDto = rule.SkipParamDto |> Option.defaultValue eff.SkipParamDto
      UseReturnValue = rule.UseReturnValue |> Option.defaultValue eff.UseReturnValue
      Parameters =
        let paramsToMerge =
          match rule.Parameters.TryFind "" with
          | None -> rule.Parameters
          | Some baseParam -> rule.Parameters |> Map.map (fun _ param -> ProcedureParameter.merge baseParam param)
        Map.merge ProcedureParameter.merge eff.Parameters paramsToMerge
      Columns =
        let colsToMerge =
          match rule.Columns.TryFind "" with
          | None -> rule.Columns
          | Some baseParam -> rule.Columns |> Map.map (fun _ param -> ProcedureColumn.merge baseParam param)
        Map.merge ProcedureColumn.merge eff.Columns colsToMerge
    }


module ProcedureOrScriptParameter =


  let fromProcedureParameter (p: ProcedureParameter) : ProcedureOrScriptParameter = {
      DtoName = p.DtoName
  }


  let fromScriptParameter (p: ScriptParameter) : ProcedureOrScriptParameter = {
      DtoName = p.DtoName
  }


module ProcedureOrScriptColumn =


  let fromProcedureColumn (p: ProcedureColumn) : ProcedureOrScriptColumn = {
      Skip = p.Skip
  }


  let fromScriptColumn (p: ScriptColumn) : ProcedureOrScriptColumn = {
      Skip = p.Skip
  }


module EffectiveProcedureOrScriptRule =


  let fromEffectiveProcedureRule (rule: EffectiveProcedureRule) : EffectiveProcedureOrScriptRule = {
      Result = rule.Result
      VoptionIn = rule.VoptionIn
      VoptionOut = rule.VoptionOut
      RecordIfSingleCol = rule.RecordIfSingleCol
      SkipParamDto = rule.SkipParamDto
      UseReturnValue = rule.UseReturnValue
      Parameters = rule.Parameters |> Map.map (fun _ -> ProcedureOrScriptParameter.fromProcedureParameter)
      Columns = rule.Columns |> Map.map (fun _ -> ProcedureOrScriptColumn.fromProcedureColumn)
  }


  let fromEffectiveScriptRule (rule: EffectiveScriptRule) : EffectiveProcedureOrScriptRule = {
      Result = rule.Result
      VoptionIn = rule.VoptionIn
      VoptionOut = rule.VoptionOut
      RecordIfSingleCol = rule.RecordIfSingleCol
      SkipParamDto = rule.SkipParamDto
      UseReturnValue = false
      Parameters = rule.Parameters |> Map.map (fun _ -> ProcedureOrScriptParameter.fromScriptParameter)
      Columns = rule.Columns |> Map.map (fun _ -> ProcedureOrScriptColumn.fromScriptColumn)
  }


module ScriptParameter =


  let fromDto (dto: ScriptParameterDto) : ScriptParameter = {
    Nullable = dto.nullable
    Type = dto.``type``
    DtoName = dto.dtoName
  }


  let merge (p1: ScriptParameter) (p2: ScriptParameter) = {
    Nullable = p2.Nullable |> Option.orElse p1.Nullable
    Type = p2.Type |> Option.orElse p1.Type
    DtoName = p2.DtoName |> Option.orElse p1.DtoName
  }


module ScriptTempTableRule =


  let fromDto basePath (dto: ScriptTempTableRuleDto) =
    {
      Definition =
        if dto.definition.EndsWith ".sql" && File.Exists(Path.Combine(basePath, dto.definition)) then
          File.ReadAllText(Path.Combine(basePath, dto.definition))
        else
          dto.definition
    }


module ScriptColumn =


  let fromDto (dto: ScriptColumnDto) : ScriptColumn =
    {
      Skip = dto.skip
    }


  let merge (c1: ScriptColumn) (c2: ScriptColumn) : ScriptColumn = {
    Skip = c2.Skip |> Option.orElse c1.Skip
  }


module ScriptRule =


  let fromDto (basePath: string) fullYamlPath (dto: ScriptRuleDto) : ScriptRule =
    let exceptMatches =
      match dto.except with
      | Some pattern -> Glob.Files(basePath, pattern) |> set
      | None -> Set.empty
    {
      IncludeOrFor =
        match dto.``include``, dto.``for`` with
        | None, None -> failwithYamlError fullYamlPath 0 0 "Either 'include' or 'for' is required in a script rule"
        | Some inc, None -> Include inc
        | None, Some f -> For f
        | Some _, Some _ -> failwithYamlError fullYamlPath 0 0 "'include' and 'for' may not be combined in a tableDto rule"
      Except = dto.except
      IncludeMatches =
        match dto.``include`` with
        | None -> Set.empty
        | Some pattern -> (Glob.Files(basePath, pattern) |> set) - exceptMatches
      ForMatches =
        match dto.``for`` with
        | None -> Set.empty
        | Some pattern -> (Glob.Files(basePath, pattern) |> set) - exceptMatches
      Result =
        dto.result
        |> Option.map (function
            | "auto" -> Auto
            | "anonymous" -> AnonymousRecord
            | name -> Custom name
        )
      VoptionIn = dto.voptionIn
      VoptionOut = dto.voptionOut
      RecordIfSingleCol = dto.recordIfSingleCol
      SkipParamDto = dto.skipParamDto
      Parameters =
        dto.``params``
        |> Option.defaultValue Map.empty
        |> Map.map (fun _ -> ScriptParameter.fromDto)
      TempTables = dto.tempTables |> Option.map (List.map (ScriptTempTableRule.fromDto basePath))
      Columns =
        dto.columns
        |> Option.defaultValue Map.empty
        |> Map.map (fun _ -> ScriptColumn.fromDto)
    }


  let defaultEffectiveRule : EffectiveScriptRule = {
    Result = Auto
    VoptionIn = false
    VoptionOut = false
    RecordIfSingleCol = false
    SkipParamDto = false
    Parameters = Map.empty
    TempTables = []
    Columns = Map.empty
  }


  let matches (globMatchOutput: string) (rule: ScriptRule) =
    rule.IncludeMatches.Contains globMatchOutput
    || rule.ForMatches.Contains globMatchOutput


  let merge (eff: EffectiveScriptRule) (rule: ScriptRule) : EffectiveScriptRule =
    {
      Result = rule.Result |> Option.defaultValue eff.Result
      VoptionIn = rule.VoptionIn |> Option.defaultValue eff.VoptionIn
      VoptionOut = rule.VoptionOut |> Option.defaultValue eff.VoptionOut
      RecordIfSingleCol = rule.RecordIfSingleCol |> Option.defaultValue eff.RecordIfSingleCol
      SkipParamDto = rule.SkipParamDto |> Option.defaultValue eff.SkipParamDto
      Parameters =
        let paramsToMerge =
          match rule.Parameters.TryFind "" with
          | None -> rule.Parameters
          | Some baseParam -> rule.Parameters |> Map.map (fun _ param -> ScriptParameter.merge baseParam param)
        Map.merge ScriptParameter.merge eff.Parameters paramsToMerge
      TempTables = rule.TempTables |> Option.defaultValue eff.TempTables
      Columns =
        let colsToMerge =
          match rule.Columns.TryFind "" with
          | None -> rule.Columns
          | Some baseParam -> rule.Columns |> Map.map (fun _ param -> ScriptColumn.merge baseParam param)
        Map.merge ScriptColumn.merge eff.Columns colsToMerge
    }



module RuleSet =


  let fromDto projectDir resolveVariable fullYamlPath (dto: RuleSetDto) =
    let scriptBasePath = dto.scriptBasePath |> Option.defaultValue projectDir |> Path.GetFullPath
    {
      ConnectionString =
        dto.connectionString
        |> Option.defaultWith (fun () -> failwithYamlError fullYamlPath 0 0 "All array items in the 'rulesets' section must have a 'connectionString' property")
        |> resolveVariable
      Filename = dto.filename |> Option.defaultValue "DbGen.fs"
      NamespaceOrModuleDeclaration = dto.namespaceOrModuleDeclaration |> Option.defaultValue "module internal DbGen"
      ScriptBasePath = scriptBasePath
      Prelude = dto.prelude |> Option.map String.getDeindentedLines
      TableDtos =
        dto.tableDtos
        |> Option.defaultValue []
        |> List.map (TableDtoRule.fromDto fullYamlPath)
      TableTypes =
        dto.tableTypes
        |> Option.defaultValue []
        |> List.map (TableTypeRule.fromDto fullYamlPath)
      Procedures =
        dto.procedures
        |> Option.defaultValue []
        |> List.map (ProcedureRule.fromDto fullYamlPath)
      Scripts =
        dto.scripts
        |> Option.defaultValue []
        |> List.map (ScriptRule.fromDto scriptBasePath fullYamlPath)
    }


  let shouldIncludeProcedure schemaName procedureName (cfg: RuleSet) =
    let qualifiedName = $"{schemaName}.{procedureName}"
    cfg.Procedures
    |> List.exists (fun rule ->
        match rule.IncludeOrFor, rule.Except with
        | For _, _ -> false
        | Include pattern, None -> Regex.IsMatch(qualifiedName, pattern)
        | Include incPattern, Some exPattern ->
            Regex.IsMatch(qualifiedName, incPattern) && not <| Regex.IsMatch(qualifiedName, exPattern)
    )


  let shouldIncludeTableDto schemaName tableName (cfg: RuleSet) =
    let qualifiedName = $"{schemaName}.{tableName}"
    cfg.TableDtos
    |> List.exists (fun rule ->
        match rule.IncludeOrFor, rule.Except with
        | For _, _ -> false
        | Include pattern, None -> Regex.IsMatch(qualifiedName, pattern)
        | Include incPattern, Some exPattern ->
            Regex.IsMatch(qualifiedName, incPattern) && not <| Regex.IsMatch(qualifiedName, exPattern)
    )


  let getEffectiveProcedureRuleFor schemaName procName (cfg: RuleSet) =
    cfg.Procedures
    |> List.filter (ProcedureRule.matches schemaName procName)
    |> List.fold ProcedureRule.merge ProcedureRule.defaultEffectiveRule


  let getEffectiveScriptRuleFor globMatchOutput (cfg: RuleSet) =
    cfg.Scripts
    |> List.filter (ScriptRule.matches globMatchOutput)
    |> List.fold ScriptRule.merge ScriptRule.defaultEffectiveRule


  let getEffectiveTableDtoRuleFor schemaName tableName (cfg: RuleSet) =
    cfg.TableDtos
    |> List.filter (TableDtoRule.matches schemaName tableName)
    |> List.fold TableDtoRule.merge TableDtoRule.defaultEffectiveRule


  let getEffectiveTableTypeRuleFor schemaName typeName (cfg: RuleSet) =
    cfg.TableTypes
    |> List.filter (TableTypeRule.matches schemaName typeName)
    |> List.fold TableTypeRule.merge TableTypeRule.defaultEffectiveRule



module FacilConfig =

  open Microsoft.Extensions.Configuration
  open Legivel.Serialization


  let getRuleSets projectDir fullYamlPath =

    let yaml = File.ReadAllText fullYamlPath

    let facilConfig =
      match DeserializeWithOptions<FacilConfigDto> [MappingMode MapYaml.AndRequireFullProjection] yaml with
      | [] -> failwithError "Unexpected empty YAML deserialization result"
      | [Success res] ->
          res.Warn
          |> List.iter (fun x ->
              logYamlWarning fullYamlPath x.Location.Line x.Location.Column x.Message
          )
          res.Data
      | results ->
          results
          |> List.choose (function Error err -> Some err | _ -> None)
          |> List.collect (fun ei ->
              [
                yield! ei.Error |> List.map (fun x -> x, logYamlError)
                yield! ei.Warn |> List.map (fun x -> x, logYamlWarning)
              ]
          )
          |> List.iter (fun (x, log) ->
              log fullYamlPath x.Location.Line x.Location.Column x.Message
          )
          failwithError "YAML deserialization failed"

    let configBuilder = ConfigurationBuilder()

    facilConfig.configs
    |> Option.iter (List.iter (fun (cfg: ConfigSourceDto) ->
        match cfg.appSettings, cfg.userSecrets, cfg.envVars with
        | Some path, None, None -> configBuilder.AddJsonFile (Path.Combine(projectDir, path)) |> ignore
        | None, Some secretsId, None -> configBuilder.AddUserSecrets secretsId |> ignore
        | None, None, Some (null | "") -> configBuilder.AddEnvironmentVariables() |> ignore
        | None, None, Some prefix -> configBuilder.AddEnvironmentVariables(prefix) |> ignore
        | _ -> failwithYamlError fullYamlPath 0 0 "Each array item in the 'configs' section of the YAML may only contain one property (either 'appSettings', 'userSecrets', or 'envVars')"
    ))

    let config = configBuilder.Build()

    let resolveVariable (str: string) =
      let m = Regex.Match(str, "\$\((.+)\)")
      if not m.Success then str
      else
        match facilConfig.configs with
        | None | Some [] -> failwithYamlError fullYamlPath 0 0 $"Cannot use variable {str} since no configuration sources has been specified"
        | _ -> ()
        let varName = m.Groups.[1].Value
        match config.GetValue varName with
        | null -> failwithYamlError fullYamlPath 0 0 $"The variable {str} could not be found in the specified configuration sources"
        | str -> str

    match facilConfig.rulesets with
    | None -> failwithYamlError fullYamlPath 0 0 "The 'rulesets' section is required"
    | Some [] -> failwithYamlError fullYamlPath 0 0 "At least one item must be specified in the 'rulesets' section"
    | Some rulesets -> rulesets |> List.map (RuleSet.fromDto projectDir resolveVariable fullYamlPath)
