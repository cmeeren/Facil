SELECT
  t1.Col1,
  t1.Col2,
  t2.Col3
FROM
  #tempTable1 t1
INNER JOIN
  #tempTable2 t2
    ON t2.Col1 = t1.Col1
