
SELECT 
      [KnownByTipranks]
      ,[KnownByZacks]
	  ,[Id]
      ,[Code]
      ,[Name]
      ,[IndustryId]
      ,[ExchangeId]
      ,[CurrencyId]
  FROM [dbo].[Ticker]
  Order by [KnownByZacks]

GO

SELECT 
      COUNT(*),
	  [KnownByZacks]
  FROM [dbo].[Ticker]
  Group by [KnownByZacks]
  
GO

SELECT 
      COUNT(*),
	  [KnownByTipRanks]
  FROM [dbo].[Ticker]
  Group by [KnownByTipRanks]
  
GO