
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/04/2016 16:55:14
-- Generated from EDMX file: C:\SDKProjects\BiggWhaleWebAppDemo\BiggWhaleWebAppDemo\BiggWhaleWebAppDemo\Funds.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [CEF_db];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FundAssets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FundAssets];
GO
IF OBJECT_ID(N'[dbo].[FundDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FundDetails];
GO
IF OBJECT_ID(N'[dbo].[Funds]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Funds];
GO
IF OBJECT_ID(N'[dbo].[Sites]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sites];
GO
IF OBJECT_ID(N'[dbo].[sysdiagrams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sysdiagrams];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'FundAssets'
CREATE TABLE [dbo].[FundAssets] (
    [id] varchar(64)  NOT NULL,
    [fund_id] varchar(64)  NOT NULL,
    [Total_Net_Assets_Date] datetime  NULL,
    [Total_Net_Assets] varchar(512)  NULL,
    [Quality] varchar(512)  NULL,
    [Crawl_Date] datetime  NULL,
    [Top_Funds_Date1] datetime  NULL,
    [Top_Funds1] varchar(512)  NULL,
    [Total_Net_Assets1] varchar(512)  NULL,
    [Quality_Date1] datetime  NULL,
    [Crawl_Date1] datetime  NULL
);
GO

-- Creating table 'FundDetails'
CREATE TABLE [dbo].[FundDetails] (
    [id] varchar(64)  NOT NULL,
    [fund_id] varchar(64)  NOT NULL,
    [Create_Date] datetime  NULL,
    [Created_By] varchar(50)  NULL,
    [Crawl_Date] datetime  NULL,
    [OneYearLipperAvg] decimal(18,3)  NULL,
    [TenYearMarketReturn] decimal(18,3)  NULL,
    [FiveYearMarketReturn] decimal(18,3)  NULL,
    [OneYearMarketReturn] decimal(18,3)  NULL,
    [YTDMarketReturn] decimal(18,3)  NULL,
    [TenYearMarketReturnRank] int  NULL,
    [FiveYearMarketReturnRank] int  NULL,
    [OneYearMarketReturnRank] int  NULL,
    [YTDMarketReturnRank] int  NULL,
    [TenYearNAVReturn] decimal(18,3)  NULL,
    [FiveYearNAVReturn] decimal(18,3)  NULL,
    [OneYearNAVReturn] decimal(18,3)  NULL,
    [YTDNAVReturn] decimal(18,3)  NULL,
    [TenYearNAVReturnRank] int  NULL,
    [FiveYearNAVReturnRank] int  NULL,
    [OneYearNAVReturnRank] int  NULL,
    [YTDNAVReturnRank] int  NULL,
    [TenYearPremiumDiscountAvg] decimal(18,3)  NULL,
    [FiveYearPremiumDiscountAvg] decimal(18,3)  NULL,
    [YTDPremiumDiscountAvg] decimal(18,3)  NULL,
    [NAV] decimal(18,3)  NULL,
    [MarketPrice] decimal(18,3)  NULL,
    [NetChange] decimal(18,3)  NULL,
    [MarketChange] decimal(18,3)  NULL,
    [OneYearNAVReturnAct] decimal(18,3)  NULL,
    [TwelveMonthYieldDate] datetime  NULL,
    [DefinedIncomeOnlyYield] decimal(18,3)  NULL,
    [DistributionYield] decimal(18,3)  NULL,
    [MostRecentIncimeDividend] decimal(18,3)  NULL,
    [MostRecentIncomeDividendDate] decimal(18,3)  NULL,
    [MostRecentCapGainDiviednd] decimal(18,3)  NULL,
    [MostRecentCapGainDividendDate] decimal(18,3)  NULL,
    [MonthlyYTDDividends] decimal(18,3)  NULL,
    [YTDCapGains] decimal(18,3)  NULL,
    [PremiumDiscount] decimal(18,3)  NULL,
    [Create_Date1] datetime  NULL,
    [Created_By1] varchar(50)  NULL,
    [Crawl_Date1] datetime  NULL
);
GO

-- Creating table 'Funds'
CREATE TABLE [dbo].[Funds] (
    [id] varchar(64)  NOT NULL,
    [Name] varchar(100)  NULL,
    [Fund_Type] varchar(50)  NULL,
    [Ticker_Symbol] varchar(25)  NULL,
    [Asset_Class] varchar(50)  NULL,
    [Inception_Date] datetime  NULL,
    [Advisor] varchar(100)  NULL,
    [Manager_And_Tenure] varchar(100)  NULL,
    [Phone] varchar(50)  NULL,
    [Website] varchar(256)  NULL,
    [Total_Net_Assets] decimal(18,3)  NULL,
    [Total_Net_Assets_Date] datetime  NULL,
    [Percent_Leveraged_Assets] decimal(18,3)  NULL,
    [Percent_Leveraged_Assets_Date] datetime  NULL,
    [Portfolio_Turnover] decimal(18,3)  NULL,
    [Management_Fees] decimal(18,3)  NULL,
    [Expense_Ratio] decimal(18,3)  NULL,
    [Alternative_Minimum_Tax] decimal(18,3)  NULL,
    [Fund_Objective] varchar(1024)  NULL,
    [Yield] decimal(18,3)  NULL,
    [Fund_Type1] varchar(50)  NULL,
    [Ticker_Symbol1] varchar(25)  NULL,
    [Asset_Class1] varchar(50)  NULL,
    [Inception_Date1] datetime  NULL,
    [Manager_And_Tenure1] varchar(100)  NULL,
    [Total_Net_Assets1] decimal(18,3)  NULL,
    [Total_Net_Assets_Date1] datetime  NULL,
    [Percent_Leveraged_Assets1] decimal(18,3)  NULL,
    [Percent_Leveraged_Assets_Date1] datetime  NULL,
    [Portfolio_Turnover1] decimal(18,3)  NULL,
    [Management_Fees1] decimal(18,3)  NULL,
    [Expense_Ratio1] decimal(18,3)  NULL,
    [Alternative_Minimum_Tax1] decimal(18,3)  NULL,
    [Fund_Objective1] varchar(1024)  NULL
);
GO

-- Creating table 'Sites'
CREATE TABLE [dbo].[Sites] (
    [id] varchar(64)  NOT NULL,
    [Name] varchar(256)  NOT NULL,
    [Created] datetime  NULL,
    [Modified] datetime  NULL,
    [Created_By] varchar(50)  NULL,
    [Last_Crawl] datetime  NULL,
    [Url] varchar(256)  NULL,
    [Created_By1] varchar(50)  NULL,
    [Last_Crawl1] datetime  NULL
);
GO

-- Creating table 'sysdiagrams'
CREATE TABLE [dbo].[sysdiagrams] (
    [name] nvarchar(128)  NOT NULL,
    [principal_id] int  NOT NULL,
    [diagram_id] int IDENTITY(1,1) NOT NULL,
    [version] int  NULL,
    [definition] varbinary(max)  NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [id] varchar(64)  NULL,
    [username] varchar(128)  NULL,
    [password] varchar(128)  NULL,
    [firstname] varchar(128)  NULL,
    [lastname] varchar(128)  NULL,
    [email] varchar(256)  NULL,
    [createdate] datetime  NULL,
    [lastmodified] datetime  NULL,
    [enabled] bit  NULL,
    [passwordexpired] bit  NULL,
    [passwordchangedate] datetime  NULL,
    [passwordminlength] smallint  NULL,
    [passwordneverexpires] bit  NULL,
    [UserId] uniqueidentifier  NOT NULL,
    [ApplicationId] uniqueidentifier  NOT NULL,
    [IsAnonymous] bit  NULL,
    [LastActivityDate] datetime  NULL
);
GO

-- Creating table 'SummaryDatas'
CREATE TABLE [dbo].[SummaryDatas] (
    [id] varchar(64)  NOT NULL,
    [Fund_Name] varchar(100)  NULL,
    [Ticker_Symbol] varchar(25)  NULL,
    [Summary_Date] datetime  NULL,
    [Calculated_Stat] varchar(100)  NULL,
    [Calculated_Value] decimal(18,3)  NULL,
    [RankOrder] int  NULL,
    [Data_Source] varchar(512)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [id] in table 'FundAssets'
ALTER TABLE [dbo].[FundAssets]
ADD CONSTRAINT [PK_FundAssets]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'FundDetails'
ALTER TABLE [dbo].[FundDetails]
ADD CONSTRAINT [PK_FundDetails]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Funds'
ALTER TABLE [dbo].[Funds]
ADD CONSTRAINT [PK_Funds]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Sites'
ALTER TABLE [dbo].[Sites]
ADD CONSTRAINT [PK_Sites]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([diagram_id] ASC);
GO

-- Creating primary key on [UserId] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([UserId] ASC);
GO

-- Creating primary key on [id] in table 'SummaryDatas'
ALTER TABLE [dbo].[SummaryDatas]
ADD CONSTRAINT [PK_SummaryDatas]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [fund_id] in table 'FundAssets'
ALTER TABLE [dbo].[FundAssets]
ADD CONSTRAINT [FK_FundFundAsset]
    FOREIGN KEY ([fund_id])
    REFERENCES [dbo].[Funds]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FundFundAsset'
CREATE INDEX [IX_FK_FundFundAsset]
ON [dbo].[FundAssets]
    ([fund_id]);
GO

-- Creating foreign key on [fund_id] in table 'FundDetails'
ALTER TABLE [dbo].[FundDetails]
ADD CONSTRAINT [FK_FundFundDetail]
    FOREIGN KEY ([fund_id])
    REFERENCES [dbo].[Funds]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FundFundDetail'
CREATE INDEX [IX_FK_FundFundDetail]
ON [dbo].[FundDetails]
    ([fund_id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------