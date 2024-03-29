﻿[<AutoOpen>]
module internal Facil.Domain

open System
open System.Data


let isPrecisionAndScaleRelevantForSqlParameter =
    function
    | SqlDbType.Decimal -> true
    | _ -> false


let isSizeRelevantForSqlParameter =
    function
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


let isPrecisionAndScaleRelevantForSqlExpression =
    function
    | SqlDbType.Decimal -> true
    | _ -> false


let isSizeRelevantForSqlExpression =
    function
    | SqlDbType.Char
    | SqlDbType.VarChar
    | SqlDbType.NChar
    | SqlDbType.NVarChar
    | SqlDbType.Binary
    | SqlDbType.VarBinary -> true
    | _ -> false


let isScaleOnlyRelevantForSqlExpression =
    function
    | SqlDbType.Time
    | SqlDbType.DateTime2
    | SqlDbType.DateTimeOffset -> true
    | _ -> false


let isPrecisionOnlyRelevantForSqlExpression =
    function
    | SqlDbType.Float -> true
    | _ -> false


let isPrecisionAndScaleRelevantForSqlMetaData =
    function
    | SqlDbType.Decimal
    | SqlDbType.Time
    | SqlDbType.DateTime2
    | SqlDbType.DateTimeOffset -> true
    | _ -> false


let isSizeRelevantForSqlMetaData =
    function
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
    DefaultBuildValue: obj
}


let sqlDbTypeMap =
    [
        {
            SqlType = "bigint"
            FSharpTypeString = "int64"
            SqlDbType = SqlDbType.BigInt
            SqlDataReaderGetMethodName = "GetInt64"
            DefaultBuildValue = 1L |> box<int64>
        }
        {
            SqlType = "binary"
            FSharpTypeString = "byte []"
            SqlDbType = SqlDbType.Binary
            SqlDataReaderGetMethodName = "GetBytes"
            DefaultBuildValue = [| 1uy |] |> box<byte[]>
        }
        {
            SqlType = "bit"
            FSharpTypeString = "bool"
            SqlDbType = SqlDbType.Bit
            SqlDataReaderGetMethodName = "GetBoolean"
            DefaultBuildValue = true |> box<bool>
        }
        {
            SqlType = "char"
            FSharpTypeString = "string"
            SqlDbType = SqlDbType.Char
            SqlDataReaderGetMethodName = "GetString"
            DefaultBuildValue = "1" |> box<string>
        }
        {
            SqlType = "date"
            FSharpTypeString = "DateTime"
            SqlDbType = SqlDbType.Date
            SqlDataReaderGetMethodName = "GetDateTime"
            DefaultBuildValue = DateTime(2000, 1, 1) |> box<DateTime>
        }
        {
            SqlType = "datetime"
            FSharpTypeString = "DateTime"
            SqlDbType = SqlDbType.DateTime
            SqlDataReaderGetMethodName = "GetDateTime"
            DefaultBuildValue = DateTime(2000, 1, 1) |> box<DateTime>
        }
        {
            SqlType = "datetime2"
            FSharpTypeString = "DateTime"
            SqlDbType = SqlDbType.DateTime2
            SqlDataReaderGetMethodName = "GetDateTime"
            DefaultBuildValue = DateTime(2000, 1, 1) |> box<DateTime>
        }
        {
            SqlType = "datetimeoffset"
            FSharpTypeString = "DateTimeOffset"
            SqlDbType = SqlDbType.DateTimeOffset
            SqlDataReaderGetMethodName = "GetDateTimeOffset"
            DefaultBuildValue = DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero) |> box<DateTimeOffset>
        }
        {
            SqlType = "decimal"
            FSharpTypeString = "decimal"
            SqlDbType = SqlDbType.Decimal
            SqlDataReaderGetMethodName = "GetDecimal"
            DefaultBuildValue = 1M |> box<decimal>
        }
        {
            SqlType = "float"
            FSharpTypeString = "float"
            SqlDbType = SqlDbType.Float
            SqlDataReaderGetMethodName = "GetDouble"
            DefaultBuildValue = 1. |> box<float>
        }
        {
            SqlType = "image"
            FSharpTypeString = "byte []"
            SqlDbType = SqlDbType.Image
            SqlDataReaderGetMethodName = "GetBytes"
            DefaultBuildValue = [| 1uy |] |> box<byte[]>
        }
        {
            SqlType = "int"
            FSharpTypeString = "int"
            SqlDbType = SqlDbType.Int
            SqlDataReaderGetMethodName = "GetInt32"
            DefaultBuildValue = 1 |> box<int>
        }
        {
            SqlType = "money"
            FSharpTypeString = "decimal"
            SqlDbType = SqlDbType.Money
            SqlDataReaderGetMethodName = "GetDecimal"
            DefaultBuildValue = 1M |> box<decimal>
        }
        {
            SqlType = "nchar"
            FSharpTypeString = "string"
            SqlDbType = SqlDbType.NChar
            SqlDataReaderGetMethodName = "GetString"
            DefaultBuildValue = "1" |> box<string>
        }
        {
            SqlType = "ntext"
            FSharpTypeString = "string"
            SqlDbType = SqlDbType.NText
            SqlDataReaderGetMethodName = "GetString"
            DefaultBuildValue = "1" |> box<string>
        }
        {
            SqlType = "numeric"
            FSharpTypeString = "decimal"
            SqlDbType = SqlDbType.Decimal
            SqlDataReaderGetMethodName = "GetDecimal"
            DefaultBuildValue = 1M |> box<decimal>
        }
        {
            SqlType = "nvarchar"
            FSharpTypeString = "string"
            SqlDbType = SqlDbType.NVarChar
            SqlDataReaderGetMethodName = "GetString"
            DefaultBuildValue = "1" |> box<string>
        }
        {
            SqlType = "real"
            FSharpTypeString = "float32"
            SqlDbType = SqlDbType.Real
            SqlDataReaderGetMethodName = "GetFloat"
            DefaultBuildValue = 1.f |> box<float32>
        }
        {
            SqlType = "rowversion"
            FSharpTypeString = "byte []"
            SqlDbType = SqlDbType.Timestamp
            SqlDataReaderGetMethodName = "GetBytes"
            DefaultBuildValue = [| 1uy |] |> box<byte[]>
        }
        {
            SqlType = "smalldatetime"
            FSharpTypeString = "DateTime"
            SqlDbType = SqlDbType.SmallDateTime
            SqlDataReaderGetMethodName = "GetDateTime"
            DefaultBuildValue = DateTime(2000, 1, 1) |> box<DateTime>
        }
        {
            SqlType = "smallint"
            FSharpTypeString = "int16"
            SqlDbType = SqlDbType.SmallInt
            SqlDataReaderGetMethodName = "GetInt16"
            DefaultBuildValue = 1s |> box<int16>
        }
        {
            SqlType = "smallmoney"
            FSharpTypeString = "decimal"
            SqlDbType = SqlDbType.SmallMoney
            SqlDataReaderGetMethodName = "GetDecimal"
            DefaultBuildValue = 1M |> box<decimal>
        }
        {
            SqlType = "text"
            FSharpTypeString = "string"
            SqlDbType = SqlDbType.Text
            SqlDataReaderGetMethodName = "GetString"
            DefaultBuildValue = "1" |> box<string>
        }
        {
            SqlType = "time"
            FSharpTypeString = "TimeSpan"
            SqlDbType = SqlDbType.Time
            SqlDataReaderGetMethodName = "GetTimeSpan"
            DefaultBuildValue = TimeSpan.Zero |> box<TimeSpan>
        }
        {
            SqlType = "timestamp"
            FSharpTypeString = "byte []"
            SqlDbType = SqlDbType.Timestamp
            SqlDataReaderGetMethodName = "GetBytes"
            DefaultBuildValue = [| 1uy |] |> box<byte[]>
        }
        {
            SqlType = "tinyint"
            FSharpTypeString = "byte"
            SqlDbType = SqlDbType.TinyInt
            SqlDataReaderGetMethodName = "GetByte"
            DefaultBuildValue = 1uy |> box<byte>
        }
        {
            SqlType = "uniqueidentifier"
            FSharpTypeString = "Guid"
            SqlDbType = SqlDbType.UniqueIdentifier
            SqlDataReaderGetMethodName = "GetGuid"
            DefaultBuildValue = Guid.NewGuid() |> box<Guid>
        }
        {
            SqlType = "varbinary"
            FSharpTypeString = "byte []"
            SqlDbType = SqlDbType.VarBinary
            SqlDataReaderGetMethodName = "GetBytes"
            DefaultBuildValue = [| 1uy |] |> box<byte[]>
        }
        {
            SqlType = "varchar"
            FSharpTypeString = "string"
            SqlDbType = SqlDbType.VarChar
            SqlDataReaderGetMethodName = "GetString"
            DefaultBuildValue = "1" |> box<string>
        }
        {
            SqlType = "xml"
            FSharpTypeString = "string"
            SqlDbType = SqlDbType.Xml
            SqlDataReaderGetMethodName = "GetString"
            DefaultBuildValue = "1" |> box<string>
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
    Collation: string option
} with

    member this.StringEscapedName =
        this.Name |> Option.map (fun s -> s.Replace("\"", "\\\""))

    member this.PascalCaseName = this.Name |> Option.map String.firstUpper


type TableColumn = {
    Name: string
    /// A value that can be used for ordering columns.
    SortKey: int
    IsNullable: bool
    IsIdentity: bool
    IsComputed: bool
    IsGeneratedAlways: bool
    Size: int16
    Precision: byte
    Scale: byte
    TypeInfo: SqlTypeInfo
    Collation: string option
    /// Used only in Db.fs to distinguish between table DTO columns and table script columns. Code elsewhere does not need
    /// to take this into account.
    ShouldSkipInTableDto: bool
} with

    member this.StringEscapedName = this.Name.Replace("\"", "\\\"")
    member this.PascalCaseName = this.Name |> String.firstUpper

    member this.PrecisionForSqlMetaData =
        match this.TypeInfo.SqlDbType with
        | SqlDbType.DateTime2
        | SqlDbType.DateTimeOffset
        | SqlDbType.Time -> 0uy
        | _ -> this.Precision

    member this.SizeForSqlMetaData = if this.Size = 0s then -1L else int64 this.Size

    member this.SqlExpression =
        let expr =
            if isSizeRelevantForSqlExpression this.TypeInfo.SqlDbType then
                let size = if this.Size <= 0s then "MAX" else string this.Size
                $"{this.TypeInfo.SqlType}({size})"
            elif isScaleOnlyRelevantForSqlExpression this.TypeInfo.SqlDbType then
                $"{this.TypeInfo.SqlType}({this.Scale})"
            elif isPrecisionOnlyRelevantForSqlExpression this.TypeInfo.SqlDbType then
                $"{this.TypeInfo.SqlType}({this.Precision})"
            elif isPrecisionAndScaleRelevantForSqlExpression this.TypeInfo.SqlDbType then
                $"{this.TypeInfo.SqlType}({this.Precision}, {this.Scale})"
            else
                this.TypeInfo.SqlType

        expr |> String.toUpper


type TableType = {
    UserTypeId: int
    SchemaName: string
    Name: string
    Columns: TableColumn list
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

    member this.FSharpParamName = this.Name |> String.trimStart '@'


type TempTable = {
    Name: string
    Source: string
    Columns: OutputColumn list
} with

    member this.FSharpName = this.Name.TrimStart '#'


type StoredProcedure = {
    ObjectId: int
    SchemaName: string
    Name: string
    Definition: string
    Parameters: Parameter list
    ResultSet: OutputColumn list option
    TempTables: TempTable list
}


type Script = {
    GlobMatchOutput: string
    RelativePathSegments: string list
    NameWithoutExtension: string
    Source: string
    Parameters: Parameter list
    ResultSet: OutputColumn list option
    TempTables: TempTable list
    GeneratedByFacil: bool
}


type TableDto = {
    SchemaName: string
    Name: string
    Columns: TableColumn list
    PrimaryKeyColumns: TableColumn list
    IsView: bool
}


type Everything = {
    TableDtos: TableDto list
    TableTypes: TableType list
    StoredProcedures: StoredProcedure list
    Scripts: Script list
}


module TableColumn =


    let toOutputColumn (c: TableColumn) : OutputColumn = {
        Name = Some c.Name
        SortKey = c.SortKey
        IsNullable = c.IsNullable
        TypeInfo = c.TypeInfo
        Collation = c.Collation
    }


module TableDto =

    let canBeUsedBy
        (resultSet: OutputColumn list option)
        (procOrScriptRule: EffectiveProcedureOrScriptRule)
        cfg
        (dto: TableDto)
        =
        let dtoRule = RuleSet.getEffectiveTableDtoRuleFor dto.SchemaName dto.Name cfg

        match resultSet with
        | None -> false
        | Some resultSet ->
            let a = resultSet |> List.map (fun c -> { c with SortKey = 0; Collation = None })

            let b =
                (dto.Columns
                 |> List.map (fun c -> { c with SortKey = 0; Collation = None } |> TableColumn.toOutputColumn))

            // Must contain all columns, but ignore order
            a |> List.forall (fun a' -> b |> List.contains a')
            && b |> List.forall (fun b' -> a |> List.contains b')
            && procOrScriptRule.VoptionOut = dtoRule.Voption


open System.IO
open System.Collections.Generic
open Microsoft.SqlServer.TransactSql.ScriptDom


// From https://github.com/fsprojects/FSharp.Data.SqlClient/blob/c96a2e2445debd078aa7dea2779333f3e149ff84/src/SqlClient.DesignTime/SqlClientExtensions.fs#L79-L99
let parseDefaultValue (definition: string) (expr: ScalarExpression) =
    match expr with
    | :? Literal as x ->
        match x.LiteralType with
        | LiteralType.Default
        | LiteralType.Null -> Some null
        | LiteralType.Integer -> x.Value |> int |> box |> Some
        | LiteralType.Money
        | LiteralType.Numeric -> x.Value |> decimal |> box |> Some
        | LiteralType.Real -> x.Value |> float |> box |> Some
        | LiteralType.String -> x.Value |> string |> box |> Some
        | _ -> None
    | :? UnaryExpression as x when x.UnaryExpressionType <> UnaryExpressionType.BitwiseNot ->
        let fragment = definition.Substring(x.StartOffset, x.FragmentLength)

        match x.Expression with
        | :? Literal as x ->
            match x.LiteralType with
            | LiteralType.Integer -> fragment |> int |> box |> Some
            | LiteralType.Money
            | LiteralType.Numeric -> fragment |> decimal |> box |> Some
            | LiteralType.Real -> fragment |> float |> box |> Some
            | _ -> None
        | _ -> None
    | _ -> None


let getParameterDefaultValues (sproc: StoredProcedure) =
    let parser = TSql150Parser(true)
    let fragment, errs = parser.Parse(new StringReader(sproc.Definition))

    if errs.Count > 0 then
        let e = errs[0]

        failwith
            $"Parsing stored procedure failed with error %i{e.Number} on line %i{e.Line}, colum %i{e.Column}: %s{e.Message}"

    let paramDefaults = Dictionary()

    fragment.Accept
        { new TSqlFragmentVisitor() with
            member _.Visit(node: ProcedureParameter) =
                base.Visit node
                paramDefaults[node.VariableName.Value] <- parseDefaultValue sproc.Definition node.Value
        }

    paramDefaults
