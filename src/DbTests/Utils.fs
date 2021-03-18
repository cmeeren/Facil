[<AutoOpen>]
module Utils

open System
open System.Data.SqlTypes
open System.Threading
open System.Threading.Tasks
open System.Reflection
open FSharp.Control
open Hedgehog



let rec getInnerEx (ex: exn) =
  if isNull ex.InnerException then ex else getInnerEx ex.InnerException


let (|InnerException|) ex =
  getInnerEx ex


/// Gets the type's static 'getPrimaryKey' method, if it exists.
let staticGetPrimaryKeyMethod<'a> =
  typeof<'a>.GetMethod("getPrimaryKey") |> Option.ofObj


module Seq =

  let tryHeadVoption xs =
    xs |> Seq.tryHead |> function Some x -> ValueSome x | None -> ValueNone


let inline allExecuteMethodsAsSingle< ^a, 'b when
                                    ^a: (member ExecuteSingle: unit -> 'b option)
                                    and ^a: (member AsyncExecuteSingle: unit -> Async<'b option>)
                                    and ^a: (member ExecuteSingleAsync: CancellationToken option -> Task<'b option>)
                                    and ^a: (member Execute: unit -> ResizeArray<'b>)
                                    and ^a: (member AsyncExecute: unit -> Async<ResizeArray<'b>>)
                                    and ^a: (member ExecuteAsync: CancellationToken option -> Task<ResizeArray<'b>>)
                                    and ^a: (member AsyncExecuteWithSyncRead: unit -> Async<ResizeArray<'b>>)
                                    and ^a: (member ExecuteAsyncWithSyncRead: CancellationToken option -> Task<ResizeArray<'b>>)
                                    and ^a: (member LazyExecute: unit -> seq<'b>)
                                    #if NET5_0
                                    and ^a: (member LazyExecuteAsync: CancellationToken option -> Collections.Generic.IAsyncEnumerable<'b>)
                                    and ^a: (member LazyExecuteAsyncWithSyncRead: CancellationToken option -> Collections.Generic.IAsyncEnumerable<'b>)
                                    #endif
                            > =
  [
    "ExecuteSingle", fun x -> (^a: (member ExecuteSingle: unit -> _) x)
    "AsyncExecuteSingle", fun x -> (^a: (member AsyncExecuteSingle: unit -> _) x) |> Async.RunSynchronously
    "ExecuteSingleAsync", fun x -> (^a: (member ExecuteSingleAsync: _ -> _) (x, None)) |> Async.AwaitTask |> Async.RunSynchronously
    "Execute", fun x -> (^a: (member Execute: unit -> _) x) |> Seq.tryHead
    "AsyncExecute", fun x -> (^a: (member AsyncExecute: unit -> _) x) |> Async.RunSynchronously |> Seq.tryHead
    "ExecuteAsync", fun x -> (^a: (member ExecuteAsync: _ -> _) (x, None)) |> Async.AwaitTask |> Async.RunSynchronously |> Seq.tryHead
    "AsyncExecuteWithSyncRead", fun x -> (^a: (member AsyncExecuteWithSyncRead: unit -> _) x) |> Async.RunSynchronously |> Seq.tryHead
    "ExecuteAsyncWithSyncRead", fun x -> (^a: (member ExecuteAsyncWithSyncRead: _ -> _) (x, None)) |> Async.AwaitTask |> Async.RunSynchronously |> Seq.tryHead
    "LazyExecute", fun x -> (^a: (member LazyExecute: unit -> _) x) |> Seq.tryHead
    #if NET5_0
    "LazyExecuteAsync", fun x -> (^a: (member LazyExecuteAsync: _ -> _) (x, None)) |> AsyncSeq.ofAsyncEnum |> AsyncSeq.toBlockingSeq |> Seq.tryHead
    "LazyExecuteAsyncWithSyncRead", fun x -> (^a: (member LazyExecuteAsyncWithSyncRead: _ -> _) (x, None)) |> AsyncSeq.ofAsyncEnum |> AsyncSeq.toBlockingSeq |> Seq.tryHead
    #endif
  ]


let inline allExecuteMethodsAsSingleVoption< ^a, 'b when
                                            ^a: (member ExecuteSingle: unit -> 'b voption)
                                            and ^a: (member AsyncExecuteSingle: unit -> Async<'b voption>)
                                            and ^a: (member ExecuteSingleAsync: CancellationToken option -> Task<'b voption>)
                                            and ^a: (member Execute: unit -> ResizeArray<'b>)
                                            and ^a: (member AsyncExecute: unit -> Async<ResizeArray<'b>>)
                                            and ^a: (member ExecuteAsync: CancellationToken option -> Task<ResizeArray<'b>>)
                                            and ^a: (member AsyncExecuteWithSyncRead: unit -> Async<ResizeArray<'b>>)
                                            and ^a: (member ExecuteAsyncWithSyncRead: CancellationToken option -> Task<ResizeArray<'b>>)
                                            and ^a: (member LazyExecute: unit -> seq<'b>)
                                            #if NET5_0
                                            and ^a: (member LazyExecuteAsync: CancellationToken option -> Collections.Generic.IAsyncEnumerable<'b>)
                                            and ^a: (member LazyExecuteAsyncWithSyncRead: CancellationToken option -> Collections.Generic.IAsyncEnumerable<'b>)
                                            #endif
                                         > =
  [
    "ExecuteSingle", fun x -> (^a: (member ExecuteSingle: unit -> _) x)
    "AsyncExecuteSingle", fun x -> (^a: (member AsyncExecuteSingle: unit -> _) x) |> Async.RunSynchronously
    "ExecuteSingleAsync", fun x -> (^a: (member ExecuteSingleAsync: _ -> _) (x, None)) |> Async.AwaitTask |> Async.RunSynchronously
    "Execute", fun x -> (^a: (member Execute: unit -> _) x) |> Seq.tryHeadVoption
    "AsyncExecute", fun x -> (^a: (member AsyncExecute: unit -> _) x) |> Async.RunSynchronously |> Seq.tryHeadVoption
    "ExecuteAsync", fun x -> (^a: (member ExecuteAsync: _ -> _) (x, None)) |> Async.AwaitTask |> Async.RunSynchronously |> Seq.tryHeadVoption
    "AsyncExecuteWithSyncRead", fun x -> (^a: (member AsyncExecuteWithSyncRead: unit -> _) x) |> Async.RunSynchronously |> Seq.tryHeadVoption
    "ExecuteAsyncWithSyncRead", fun x -> (^a: (member ExecuteAsyncWithSyncRead: _ -> _) (x, None)) |> Async.AwaitTask |> Async.RunSynchronously |> Seq.tryHeadVoption
    "LazyExecute", fun x -> (^a: (member LazyExecute: unit -> _) x) |> Seq.tryHeadVoption
    #if NET5_0
    "LazyExecuteAsync", fun x -> (^a: (member LazyExecuteAsync: _ -> _) (x, None)) |> AsyncSeq.ofAsyncEnum |> AsyncSeq.toBlockingSeq |> Seq.tryHeadVoption
    "LazyExecuteAsyncWithSyncRead", fun x -> (^a: (member LazyExecuteAsyncWithSyncRead: _ -> _) (x, None)) |> AsyncSeq.ofAsyncEnum |> AsyncSeq.toBlockingSeq |> Seq.tryHeadVoption
    #endif
  ]


let inline allEagerSingleExecuteMethods< ^a, 'b when
                                          ^a: (member ExecuteSingle: unit -> 'b)
                                          and ^a: (member AsyncExecuteSingle: unit -> Async<'b>)
                                          and ^a: (member ExecuteSingleAsync: CancellationToken option -> Task<'b>)
                                      > =
  [
    "ExecuteSingle", fun x -> (^a: (member ExecuteSingle: unit -> _) x)
    "AsyncExecuteSingle", fun x -> (^a: (member AsyncExecuteSingle: unit -> _) x) |> Async.RunSynchronously
    "ExecuteSingleAsync", fun x -> (^a: (member ExecuteSingleAsync: _ -> _) (x, None)) |> Async.AwaitTask |> Async.RunSynchronously
  ]


let inline allEagerSeqExecuteMethods< ^a, 'b when
                                    ^a: (member Execute: unit -> 'b)
                                    and ^a: (member AsyncExecute: unit -> Async<'b>)
                                    and ^a: (member ExecuteAsync: CancellationToken option -> Task<'b>)
                                    and ^a: (member AsyncExecuteWithSyncRead: unit -> Async<'b>)
                                    and ^a: (member ExecuteAsyncWithSyncRead: CancellationToken option -> Task<'b>)
                            > =
  [
    "Execute", fun x -> (^a: (member Execute: unit -> _) x)
    "AsyncExecute", fun x -> (^a: (member AsyncExecute: unit -> _) x) |> Async.RunSynchronously
    "ExecuteAsync", fun x -> (^a: (member ExecuteAsync: _ -> _) (x, None)) |> Async.AwaitTask |> Async.RunSynchronously
    "AsyncExecuteWithSyncRead", fun x -> (^a: (member AsyncExecuteWithSyncRead: unit -> _) x) |> Async.RunSynchronously
    "ExecuteAsyncWithSyncRead", fun x -> (^a: (member ExecuteAsyncWithSyncRead: _ -> _) (x, None)) |> Async.AwaitTask |> Async.RunSynchronously
  ]


let inline allSeqExecuteMethods< ^a, 'b when
                                ^a: (member Execute: unit -> ResizeArray<'b>)
                                and ^a: (member AsyncExecute: unit -> Async<ResizeArray<'b>>)
                                and ^a: (member ExecuteAsync: CancellationToken option -> Task<ResizeArray<'b>>)
                                and ^a: (member AsyncExecuteWithSyncRead: unit -> Async<ResizeArray<'b>>)
                                and ^a: (member ExecuteAsyncWithSyncRead: CancellationToken option -> Task<ResizeArray<'b>>)
                                and ^a: (member LazyExecute: unit -> seq<'b>)
                                #if NET5_0
                                and ^a: (member LazyExecuteAsync: CancellationToken option -> Collections.Generic.IAsyncEnumerable<'b>)
                                and ^a: (member LazyExecuteAsyncWithSyncRead: CancellationToken option -> Collections.Generic.IAsyncEnumerable<'b>)
                                #endif
                            > =
  [
    "Execute", fun x -> (^a: (member Execute: unit -> _) x)
    "AsyncExecute", fun x -> (^a: (member AsyncExecute: unit -> _) x) |> Async.RunSynchronously
    "ExecuteAsync", fun x -> (^a: (member ExecuteAsync: _ -> _) (x, None)) |> Async.AwaitTask |> Async.RunSynchronously
    "AsyncExecuteWithSyncRead", fun x -> (^a: (member AsyncExecuteWithSyncRead: unit -> _) x) |> Async.RunSynchronously
    "ExecuteAsyncWithSyncRead", fun x -> (^a: (member ExecuteAsyncWithSyncRead: _ -> _) (x, None)) |> Async.AwaitTask |> Async.RunSynchronously
    "LazyExecute", fun x -> (^a: (member LazyExecute: unit -> _) x) |> ResizeArray
    #if NET5_0
    "LazyExecuteAsync", fun x -> (^a: (member LazyExecuteAsync: _ -> _) (x, None)) |> AsyncSeq.ofAsyncEnum |> AsyncSeq.toBlockingSeq |> ResizeArray
    "LazyExecuteAsyncWithSyncRead", fun x -> (^a: (member LazyExecuteAsyncWithSyncRead: _ -> _) (x, None)) |> AsyncSeq.ofAsyncEnum |> AsyncSeq.toBlockingSeq |> ResizeArray
    #endif
  ]


let inline allLazyExecuteMethods< ^a, 'b when
                                ^a: (member LazyExecute: unit -> seq<'b>)
                                #if NET5_0
                                and ^a: (member LazyExecuteAsync: CancellationToken option -> Collections.Generic.IAsyncEnumerable<'b>)
                                and ^a: (member LazyExecuteAsyncWithSyncRead: CancellationToken option -> Collections.Generic.IAsyncEnumerable<'b>)
                                #endif
                            > =
  [
    "LazyExecute", fun x -> (^a: (member LazyExecute: unit -> _) x)
    #if NET5_0
    "LazyExecuteAsync", fun x -> (^a: (member LazyExecuteAsync: _ -> _) (x, None)) |> AsyncSeq.ofAsyncEnum |> AsyncSeq.toBlockingSeq
    "LazyExecuteAsyncWithSyncRead", fun x -> (^a: (member LazyExecuteAsyncWithSyncRead: _ -> _) (x, None)) |> AsyncSeq.ofAsyncEnum |> AsyncSeq.toBlockingSeq
    #endif
  ]


let inline allNonResultExecuteMethods< ^a, 'b when
                                      ^a: (member Execute: unit -> 'b)
                                      and ^a: (member AsyncExecute: unit -> Async<'b>)
                                      and ^a: (member ExecuteAsync: CancellationToken option -> Task<'b>)
                                    > =
  [
    "Execute", fun x -> (^a: (member Execute: unit -> _) x)
    "AsyncExecute", fun x -> (^a: (member AsyncExecute: unit -> _) x) |> Async.RunSynchronously
    "ExecuteAsync", fun x -> (^a: (member ExecuteAsync: _ -> _) (x, None)) |> Async.AwaitTask |> Async.RunSynchronously
  ]



module Gen =


  module Sql =


    let bigint = Gen.int64 (Range.exponentialBounded ())

    let binary n =
      if n < 1 || n > 8000 then invalidArg (nameof n) "Must be between 1 and 8000"
      Gen.byte (Range.exponentialBounded()) |> Gen.array (Range.constant n n)

    let bit = Gen.bool

    let char n =
      if n < 1 || n > 8000 then invalidArg (nameof n) "Must be between 1 and 8000"
      // Stick to low ASCII which is safe in any collation.
      // Don't include NULL because it seems to mess with test output.
      Gen.char '\001' '\127' |> Gen.string (Range.constant n n)

    let date =
      Range.exponentialFrom (DateTime(2000, 1, 1).Ticks) DateTime.MinValue.Ticks DateTime.MaxValue.Ticks
      |> Range.map DateTime
      |> Gen.dateTime
      |> Gen.map (fun dt -> dt.Date)

    let datetime =
      gen {
        let! ticks =
          Range.linearFrom
            (DateTime(2000, 1, 1)).Ticks
            SqlDateTime.MinValue.Value.Ticks
            SqlDateTime.MaxValue.Value.Ticks
          |> Gen.integral
        let ms = ticks / TimeSpan.TicksPerMillisecond
        let msAdjusted =
          match ms % 10L with
          | 0L -> ms
          | 1L -> ms - 1L
          | 2L -> ms + 1L
          | 3L -> ms
          | 4L -> ms - 1L
          | 5L -> ms - 2L  // Ambiguous, we could round both ways
          | 6L -> ms + 1L
          | 7L -> ms
          | 8L -> ms - 1L
          | 9L -> ms + 1L
          | _ -> failwith "Impossible modulo"
        return DateTime(msAdjusted * TimeSpan.TicksPerMillisecond)
      }


    let datetime2 (n: int) =
      if n < 0 || n > 7 then invalidArg (nameof n) "Must be between 0 and 7"
      let tickPrecision = TimeSpan.TicksPerSecond / int64 (10. ** float n)
      let adjustTickPrecision ticks = (ticks / tickPrecision) * tickPrecision
      Range.linearFrom
        (DateTime(2000, 1, 1)).Ticks
        DateTime.MinValue.Ticks
        DateTime.MaxValue.Ticks
      |> Gen.integral
      |> Gen.map (adjustTickPrecision >> DateTime)

    let datetimeoffset n =
      gen {
        if n < 0 || n > 7 then invalidArg (nameof n) "Must be between 0 and 7"

        let tickPrecision = TimeSpan.TicksPerSecond / int64 (10. ** float n)
        let adjustTickPrecision ticks = (ticks / tickPrecision) * tickPrecision

        let! ticks =
          Range.linearFrom
            (DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero)).Ticks
            DateTimeOffset.MinValue.Ticks
            DateTimeOffset.MaxValue.Ticks
          |> Gen.integral
          |> Gen.map adjustTickPrecision

        // Ensure there is no overflow near the edges when adding the offset
        let minOffsetMinutes =
          max 
            (-14L * 60L)
            ((DateTimeOffset.MaxValue.Ticks - ticks) / TimeSpan.TicksPerMinute * -1L)
        let maxOffsetMinutes =
          min 
            (14L * 60L)
            ((ticks - DateTimeOffset.MinValue.Ticks) / TimeSpan.TicksPerMinute)
        let! offsetMinutes = Gen.int (Range.exponentialFrom 0 (int minOffsetMinutes) (int maxOffsetMinutes))
        return DateTimeOffset(ticks, TimeSpan.FromMinutes (float offsetMinutes))
      }
      
    let decimal (p: int) (s: int) =
      gen {
        if p < 1 || p > int SqlDecimal.MaxPrecision then invalidArg (nameof p) $"Must be between 1 and %i{SqlDecimal.MaxPrecision}"
        if s < 0 || s > int SqlDecimal.MaxScale || s > p then invalidArg (nameof s) $"Must be between 0 and %i{SqlDecimal.MaxScale} and no larger than {nameof p}"

        // For simplicity, generate left/right sides as int64 and parse the result as a
        // decimal. This means that very high/low decimal values won't be generated, since
        // int64 can't represent 10^38 - 1. This is probably fine.
        let maxLeft = int64 (10. ** float (p - s)) - 1L
        let maxRight = int64 (10. ** float s) - 1L
        let! left = Gen.int64 (Range.exponential 0L maxLeft)
        let! right = Gen.int64 (Range.exponential 0L maxRight)
        let! negative = Gen.bool
        let decimalString =
          (if negative then "-" else "")
          + string<int64> left
          + (if s = 0 then "" else "." + string<int64> right)
        return Decimal.Parse(decimalString, Globalization.CultureInfo.InvariantCulture)
      }

    let float n =
      gen {
        let precision =
          if n >= 1 && n <= 24 then 7
          elif n >= 25 && n <= 53 then 15
          else invalidArg (nameof n) $"Must be between 1 and 53"
        let! scale = Gen.int (Range.linear 0 precision)
        return! decimal precision scale |> Gen.map float
      }

    let int = Gen.int (Range.exponentialBounded ())

    let money =
      // TODO: Shrink to no decimals?
      Gen.int64 (Range.exponentialBounded ())
      |> Gen.map (Operators.decimal >> fun d -> d / 10000M)

    let nchar n =
      if n < 1 || n > 4000 then invalidArg (nameof n) $"Must be between 1 and 4000"
      // Limit to UTC-2 code points to ensure that the storage size is equal to n.
      // Don't include NULL because it seems to mess with test output.
      Gen.choice [
        Gen.char '\u0001' '\uD7FF'
        Gen.char '\uE000' '\uFFFF'
      ]
      |> Gen.string (Range.constant n n)

    let numeric p s = decimal p s

    let nvarchar n =
      if n < 1 || n > 4000 then invalidArg (nameof n) $"Must be between 1 and 4000"
      // Limit to UTC-2 code points to ensure that the storage size is equal to n.
      // Don't include NULL because it seems to mess with test output.
      Gen.choice [
        Gen.char '\u0001' '\uD7FF'
        Gen.char '\uE000' '\uFFFF'
      ]
      |> Gen.string (Range.exponential 0 n)

    let real = float 24 |> Gen.map single

    let rowversion = binary 8

    let smalldatetime =
      let tickPrecision = TimeSpan.TicksPerMinute
      let adjustTickPrecision ticks = (ticks / tickPrecision) * tickPrecision
      Range.linearFrom
        (DateTime(2000, 1, 1)).Ticks
        (DateTime(1900, 1, 1)).Ticks
        (DateTime(2079, 6, 6)).Ticks
      |> Gen.integral
      |> Gen.map (adjustTickPrecision >> DateTime)

    let smallint = Gen.int16 (Range.exponentialBounded ())

    let smallmoney =
      Gen.int (Range.exponentialBounded ())
      |> Gen.map (Operators.decimal >> fun d -> d / 10000M)

    let time (n: int) =
      if n < 0 || n > 7 then invalidArg (nameof n) "Must be between 0 and 7"
      let tickPrecision = TimeSpan.TicksPerSecond / int64 (10. ** Operators.float n)
      let adjustTickPrecision ticks = (ticks / tickPrecision) * tickPrecision
      Range.linear 0L TimeSpan.TicksPerDay
      |> Gen.integral
      |> Gen.map (adjustTickPrecision >> TimeSpan.FromTicks)

    let timestamp = rowversion

    let tinyint = Gen.byte (Range.exponentialBounded ())

    let uniqueidentifier = Gen.guid

    let varbinary n =
      Gen.byte (Range.exponentialBounded()) |> Gen.array (Range.exponential 0 n)

    let varchar n =
      // Stick to low ASCII which is safe in any collation.
      // Don't include NULL because it seems to mess with test output.
      Gen.char '\001' '\127' |> Gen.string (Range.exponential 0 n)

    // TODO: Ideally generate random XML (not critical). Can probably use the BCL XML APIs
    // for this.
    let xml = Gen.constant "<tag attr=\"value\" />"

    let image = varbinary 50

    let ntext = nvarchar 50

    let text = varchar 50
