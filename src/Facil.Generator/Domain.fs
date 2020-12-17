﻿[<AutoOpen>]
module internal Facil.Domain

open System.Data


let isPrecisionAndScaleRelevantForSqlParameter = function
  | SqlDbType.Decimal -> true
  | _ -> false


let isSizeRelevantForSqlParameter = function
  | SqlDbType.Float
  | SqlDbType.Time
  | SqlDbType.DateTime2
  | SqlDbType.DateTimeOffset
  | SqlDbType.Char
  | SqlDbType.VarChar
  | SqlDbType.NChar
  | SqlDbType.NVarChar
  | SqlDbType.Binary
  | SqlDbType.VarBinary -> true
  | _ -> false


let isPrecisionAndScaleRelevantForSqlMetaData = function
  | SqlDbType.Decimal
  | SqlDbType.Time
  | SqlDbType.DateTime2
  | SqlDbType.DateTimeOffset -> true
  | _ -> false


let isSizeRelevantForSqlMetaData = function
  | SqlDbType.Char
  | SqlDbType.VarChar
  | SqlDbType.NChar
  | SqlDbType.NVarChar
  | SqlDbType.Binary
  | SqlDbType.VarBinary -> true
  | _ -> false


type SqlTypeInfo = {
  SqlType: string
  FSharpTypeString: string
  SqlDbType: SqlDbType
  SqlDataReaderGetMethodName: string
}


let sqlDbTypeMap =
  [
    {
      SqlType = "bigint"
      FSharpTypeString = "int64"
      SqlDbType = SqlDbType.BigInt
      SqlDataReaderGetMethodName = "GetInt64"
    }
    {
      SqlType = "binary"
      FSharpTypeString = "byte []"
      SqlDbType = SqlDbType.Binary
      SqlDataReaderGetMethodName = "GetBytes"
    }
    {
      SqlType = "bit"
      FSharpTypeString = "bool"
      SqlDbType = SqlDbType.Bit
      SqlDataReaderGetMethodName = "GetBoolean"
    }
    {
      SqlType = "char"
      FSharpTypeString = "string"
      SqlDbType = SqlDbType.Char
      SqlDataReaderGetMethodName = "GetString"
    }
    {
      SqlType = "date"
      FSharpTypeString = "DateTime"
      SqlDbType = SqlDbType.Date
      SqlDataReaderGetMethodName = "GetDateTime"
    }
    {
      SqlType = "datetime"
      FSharpTypeString = "DateTime"
      SqlDbType = SqlDbType.DateTime
      SqlDataReaderGetMethodName = "GetDateTime"
    }
    {
      SqlType = "datetime2"
      FSharpTypeString = "DateTime"
      SqlDbType = SqlDbType.DateTime2
      SqlDataReaderGetMethodName = "GetDateTime"
    }
    {
      SqlType = "datetimeoffset"
      FSharpTypeString = "DateTimeOffset"
      SqlDbType = SqlDbType.DateTimeOffset
      SqlDataReaderGetMethodName = "GetDateTimeOffset"
    }
    {
      SqlType = "decimal"
      FSharpTypeString = "decimal"
      SqlDbType = SqlDbType.Decimal
      SqlDataReaderGetMethodName = "GetDecimal"
    }
    {
      SqlType = "float"
      FSharpTypeString = "float"
      SqlDbType = SqlDbType.Float
      SqlDataReaderGetMethodName = "GetDouble"
    }
    {
      SqlType = "image"
      FSharpTypeString = "byte []"
      SqlDbType = SqlDbType.Image
      SqlDataReaderGetMethodName = "GetBytes"
    }
    {
      SqlType = "int"
      FSharpTypeString = "int"
      SqlDbType = SqlDbType.Int
      SqlDataReaderGetMethodName = "GetInt32"
    }
    {
      SqlType = "money"
      FSharpTypeString = "decimal"
      SqlDbType = SqlDbType.Money
      SqlDataReaderGetMethodName = "GetDecimal"
    }
    {
      SqlType = "nchar"
      FSharpTypeString = "string"
      SqlDbType = SqlDbType.NChar
      SqlDataReaderGetMethodName = "GetString"
    }
    {
      SqlType = "ntext"
      FSharpTypeString = "string"
      SqlDbType = SqlDbType.NText
      SqlDataReaderGetMethodName = "GetString"
    }
    {
      SqlType = "numeric"
      FSharpTypeString = "decimal"
      SqlDbType = SqlDbType.Decimal
      SqlDataReaderGetMethodName = "GetDecimal"
    }
    {
      SqlType = "nvarchar"
      FSharpTypeString = "string"
      SqlDbType = SqlDbType.NVarChar
      SqlDataReaderGetMethodName = "GetString"
    }
    {
      SqlType = "real"
      FSharpTypeString = "float32"
      SqlDbType = SqlDbType.Real
      SqlDataReaderGetMethodName = "GetFloat"
    }
    {
      SqlType = "rowversion"
      FSharpTypeString = "byte []"
      SqlDbType = SqlDbType.Timestamp
      SqlDataReaderGetMethodName = "GetBytes"
    }
    {
      SqlType = "smalldatetime"
      FSharpTypeString = "DateTime"
      SqlDbType = SqlDbType.SmallDateTime
      SqlDataReaderGetMethodName = "GetDateTime"
    }
    {
      SqlType = "smallint"
      FSharpTypeString = "int16"
      SqlDbType = SqlDbType.SmallInt
      SqlDataReaderGetMethodName = "GetInt16"
    }
    {
      SqlType = "smallmoney"
      FSharpTypeString = "decimal"
      SqlDbType = SqlDbType.SmallMoney
      SqlDataReaderGetMethodName = "GetDecimal"
    }
    {
      SqlType = "text"
      FSharpTypeString = "string"
      SqlDbType = SqlDbType.Text
      SqlDataReaderGetMethodName = "GetString"
    }
    {
      SqlType = "time"
      FSharpTypeString = "TimeSpan"
      SqlDbType = SqlDbType.Time
      SqlDataReaderGetMethodName = "GetTimeSpan"
    }
    {
      SqlType = "timestamp"
      FSharpTypeString = "byte []"
      SqlDbType = SqlDbType.Timestamp
      SqlDataReaderGetMethodName = "GetBytes"
    }
    {
      SqlType = "tinyint"
      FSharpTypeString = "byte"
      SqlDbType = SqlDbType.TinyInt
      SqlDataReaderGetMethodName = "GetByte"
    }
    {
      SqlType = "uniqueidentifier"
      FSharpTypeString = "Guid"
      SqlDbType = SqlDbType.UniqueIdentifier
      SqlDataReaderGetMethodName = "GetGuid"
    }
    {
      SqlType = "varbinary"
      FSharpTypeString = "byte []"
      SqlDbType = SqlDbType.VarBinary
      SqlDataReaderGetMethodName = "GetBytes"
    }
    {
      SqlType = "varchar"
      FSharpTypeString = "string"
      SqlDbType = SqlDbType.VarChar
      SqlDataReaderGetMethodName = "GetString"
    }
    {
      SqlType = "xml"
      FSharpTypeString = "string"
      SqlDbType = SqlDbType.Xml
      SqlDataReaderGetMethodName = "GetString"
    }
  ]
  |> List.map (fun ti -> ti.SqlType, ti)
  |> Map.ofList


type OutputColumn = {
  Name: string option
  /// A value that can be used for ordering columns.
  SortKey: int
  IsNullable: bool
  TypeInfo: SqlTypeInfo
} with
  member this.StringEscapedName = this.Name |> Option.map (fun s -> s.Replace("\"", "\\\""))


type TableTypeColumn = {
  Name: string
  /// A value that can be used for ordering columns.
  SortKey: int
  IsNullable: bool
  Size: int16
  Precision: byte
  Scale: byte
  TypeInfo: SqlTypeInfo
} with
  member this.StringEscapedName = this.Name.Replace("\"", "\\\"")
  member this.PrecisionForSqlMetaData =
    match this.TypeInfo.SqlDbType with
    | SqlDbType.DateTime2 | SqlDbType.DateTimeOffset | SqlDbType.Time -> 0uy
    | _ -> this.Precision


type TableType = {
  UserTypeId: int
  SchemaName: string
  Name: string
  Columns: TableTypeColumn list
}


type ParameterTypeInfo =
  | Scalar of SqlTypeInfo
  | Table of TableType


/// A parameter for a stored procedure, script, or similar.
type Parameter = {
  Name: string
  /// A value that can be used for ordering parameters.
  SortKey: int
  Size: int16
  Precision: byte
  Scale: byte
  FSharpDefaultValueString: string option
  TypeInfo: ParameterTypeInfo
  IsOutput: bool
  IsCursorRef: bool
} with
  member this.FSharpParamName =
    this.Name |> String.trimStart '@'


type StoredProcedure = {
  ObjectId: int
  SchemaName: string
  Name: string
  Definition: string
  Parameters: Parameter list
  ResultSet: OutputColumn list option
}

type TempTable = {
  Name : string
  Source : string
  Columns: OutputColumn list
}

type Script = {
  GlobMatchOutput: string
  RelativePathSegments: string list
  NameWithoutExtension: string
  Source: string
  Parameters: Parameter list
  ResultSet: OutputColumn list option
  TempTable : TempTable option
}


type TableDto = {
  SchemaName: string
  Name: string
  Columns: OutputColumn list
}


type Everything = {
  TableDtos: TableDto list
  TableTypes: TableType list
  StoredProcedures: StoredProcedure list
  Scripts: Script list
}


module TableDto =

  let canBeUsedBy (resultSet: OutputColumn list option) (procOrScriptRule: EffectiveProcedureOrScriptRule) cfg (dto: TableDto) =
    match procOrScriptRule.Result with
    | AnonymousRecord | Custom _ -> false
    | Auto ->
        let dtoRule = RuleSet.getEffectiveTableDtoRuleFor dto.SchemaName dto.Name cfg
        resultSet |> Option.map (List.map (fun c -> { c with SortKey = 0 }))
          = Some (dto.Columns |> List.map (fun c -> { c with SortKey = 0 }))
        && procOrScriptRule.VoptionOut = dtoRule.Voption


open System.IO
open System.Collections.Generic
open Microsoft.SqlServer.TransactSql.ScriptDom


// From https://github.com/fsprojects/FSharp.Data.SqlClient/blob/c96a2e2445debd078aa7dea2779333f3e149ff84/src/SqlClient.DesignTime/SqlClientExtensions.fs#L79-L99
let parseDefaultValue (definition: string) (expr: ScalarExpression) =
  match expr with
  | :? Literal as x ->
      match x.LiteralType with
      | LiteralType.Default | LiteralType.Null -> Some null
      | LiteralType.Integer -> x.Value |> int |> box |> Some
      | LiteralType.Money | LiteralType.Numeric -> x.Value |> decimal |> box |> Some
      | LiteralType.Real -> x.Value |> float |> box |> Some 
      | LiteralType.String -> x.Value |> string |> box |> Some 
      | _ -> None
  | :? UnaryExpression as x when x.UnaryExpressionType <> UnaryExpressionType.BitwiseNot ->
      let fragment = definition.Substring(x.StartOffset, x.FragmentLength)
      match x.Expression with
      | :? Literal as x ->
          match x.LiteralType with
          | LiteralType.Integer -> fragment |> int |> box |> Some
          | LiteralType.Money | LiteralType.Numeric -> fragment |> decimal |> box |> Some
          | LiteralType.Real -> fragment |> float |> box |> Some 
          | _ -> None
      | _  -> None 
  | _ -> None 


let getParameterDefaultValues (sproc: StoredProcedure) =
  let parser = TSql150Parser(true)
  let fragment, _ = parser.Parse(new StringReader(sproc.Definition))

  let paramDefaults = Dictionary()

  fragment.Accept
    { new TSqlFragmentVisitor() with
        member _.Visit (node: ProcedureParameter) =
          base.Visit node
          paramDefaults.[node.VariableName.Value] <- parseDefaultValue sproc.Definition node.Value }

  paramDefaults
