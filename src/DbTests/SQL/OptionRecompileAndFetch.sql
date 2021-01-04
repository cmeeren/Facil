-- Use an extended sproc to force sp_describe_first_result_set to fail
-- and parser to fallback to SET FMTONLY ON
EXEC sp_getapplock 'TestLock', 'Update'

SELECT
  *
FROM
  Table1
ORDER BY
  TableCol1
OFFSET @offset ROWS
FETCH NEXT @limit ROWS ONLY

-- Together with FETCH above, this will for some reason cause SET FMTONLY ON to fail if --
-- @limit is NULL, meaning that the parameter will have to be added with an actual value even
-- for SET FMTONLY ON
OPTION (RECOMPILE)
