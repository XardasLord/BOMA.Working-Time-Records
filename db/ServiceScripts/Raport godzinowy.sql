SELECT 
	DATEPART(year, [Date]) as Rok,
	SUM(BaseNormativeHours) as [Godziny nominalne],
	SUM(FiftyPercentageBonusHours) as [Nadgodziny 50%],
	SUM(HundredPercentageBonusHours) as [Nadgodziny 100%],
	SUM(SaturdayHours) as [Soboty]
FROM [Boma].[dbo].[WorkingTimeRecordAggregatedHistories]
WHERE DATEPART(year, [Date]) IN (2023, 2024, 2025)
GROUP BY DATEPART(year, [Date])
ORDER BY Rok;



-- Z rozbiciem na działy
WITH Dept AS (
  SELECT v.Id, v.Name
  FROM (VALUES
    (0,'Wszyscy'),
    (1,'Magazyn'),
    (2,'Akcesoria'),
    (3,'Produkcja'),
    (4,'Pakowanie'),
    (6,'BOMA'),
    (7,'Zlecenia'),
    (8,'Agencja')
  ) AS v(Id, Name)
)
SELECT
  DATEPART(year, w.[Date])                                    AS Rok,
  d.Name                                                      AS [Dział],
  SUM(w.BaseNormativeHours)                                   AS [Godziny nominalne],
  SUM(w.FiftyPercentageBonusHours)                            AS [Nadgodziny 50%],
  SUM(w.HundredPercentageBonusHours)                          AS [Nadgodziny 100%],
  SUM(w.SaturdayHours)                                        AS [Soboty]
FROM [Boma].[dbo].[WorkingTimeRecordAggregatedHistories] w
JOIN Dept d
  ON d.Id = w.DepartmentId
WHERE DATEPART(year, w.[Date]) IN (2023, 2024)
  AND w.DepartmentId IN (0,1,2,3,4,6,7,8)
GROUP BY DATEPART(year, w.[Date]), d.Id, d.Name
ORDER BY Rok, d.Id;
