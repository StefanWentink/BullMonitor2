USE [TickerDb]
GO

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
  WHERE [Code] = 'MSFT'
GO

Update  [dbo].[Ticker]
SET [KnownByZacks] = null
WHERE [Id] = 'D1213670-AF23-461E-BCF4-08DBB84D9919'

GO

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

--update [dbo].[Ticker] SET [KnownByZacks] = null

SELECT 
      COUNT(*),
	  [KnownByZacks]
  FROM [dbo].[Ticker]
  Group by [KnownByZacks]

