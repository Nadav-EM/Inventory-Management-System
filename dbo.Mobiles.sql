CREATE TABLE [dbo].[Mobiles] (
    [First]    VARCHAR (50) NULL,
    [Last]     VARCHAR (50) NULL,
    [Mobile]   VARCHAR (50) NOT NULL,
    [Email]    VARCHAR (50) NULL,
    [Catagory] VARCHAR (50) NULL,
    [Adddress] VARCHAR (50) NULL,
    [City] VARCHAR(50) NULL, 
    [UserID] CHAR(10) NULL, 
    PRIMARY KEY CLUSTERED ([Mobile])
);

