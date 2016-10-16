CREATE DATABASE Theater
GO

ALTER DATABASE Theater SET RECOVERY SIMPLE
GO

USE Theater
	CREATE TABLE dbo.authors (
    Id   INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    name NVARCHAR (50) NOT NULL
);

CREATE TABLE dbo.genres (
    Id   INT           IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    name NVARCHAR (50) NOT NULL
);

CREATE TABLE dbo.logins (
    Id       INT           IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    name     NVARCHAR (50) NOT NULL,
    password NVARCHAR (50) NOT NULL,
    roleId   INT           NOT NULL,
    email    NVARCHAR (50) NOT NULL,
    phone    NVARCHAR (50) NOT NULL
);

CREATE TABLE dbo.plays (
    Id          INT            IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    name        NVARCHAR (50)  NOT NULL,
    authorId    INT            NOT NULL,
    genreId     INT            NOT NULL,
    description NVARCHAR (MAX) NULL,
    CONSTRAINT [FK_plays_authors] FOREIGN KEY ([authorId]) REFERENCES [dbo].[authors] ([Id]),
    CONSTRAINT [FK_plays_genres] FOREIGN KEY ([genreId]) REFERENCES [dbo].[genres] ([Id])
);

CREATE TABLE dbo.dates (
    Id      INT  IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    playsId INT  NOT NULL,
    date    DATE NOT NULL,
	CONSTRAINT [FK_dates_plays] FOREIGN KEY ([playsId]) REFERENCES [dbo].[plays] ([Id])

);

CREATE TABLE dbo.orders (
    Id            INT   IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    dateId        INT   NOT NULL,
    loginId       INT   NOT NULL,
    categoryId    INT   NOT NULL,
    quantity      INT   NOT NULL,
    price         MONEY NOT NULL,
    statusOrderId INT   NOT NULL,
    CONSTRAINT [FK_orders_dates] FOREIGN KEY ([dateId]) REFERENCES [dbo].[dates] ([Id]),
    CONSTRAINT [FK_orders_logins] FOREIGN KEY ([loginId]) REFERENCES [dbo].[logins] ([Id])

);

Declare @i int = 0, @j int = 0, @NameLimit int, @Position int
DECLARE @Symbol CHAR(52)= 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz'
DECLARE @CountForSmallTable INT = 10
DECLARE @CountForBigTable INT = 15
DECLARE @CountAuthors INT = 0
DECLARE @CountGenres INT = 0
DECLARE @Countlogins INT = 0
DECLARE @CountOrders INT = 0
DECLARE @CountDates INT = 0
DECLARE @CountPlays INT = 0

---------Fill Authors table////////////////////////////////////////

DECLARE @AuthorName VARCHAR(50) = ''
WHILE @i < @CountForSmallTable
BEGIN
	SET @NameLimit = 5 + RAND() * 5
	WHILE @j < @NameLimit
	BEGIN
		SET @Position = RAND() * 52
		SET @AuthorName += SUBSTRING(@Symbol, @Position, 1)
		SET @j += 1
	END
	INSERT INTO authors (name)
	VALUES (@AuthorName)
  SET @CountAuthors += 1
	SET @i += 1
	SET @j = 0	
	SET @AuthorName = ''
END
SET @i = 0

----------Fill genres table//////////////////////////////////////////////

DECLARE @GenreName VARCHAR(50) = ''
WHILE @i < @CountForSmallTable
BEGIN
	SET @NameLimit = 5 + RAND() * 5
	WHILE @j < @NameLimit
	BEGIN
		SET @Position = RAND() * 52
		SET @GenreName += SUBSTRING(@Symbol, @Position, 1)
		SET @j += 1
	END
	INSERT INTO genres(name)
	VALUES (@GenreName)
	SET @CountGenres += 1
	SET @i += 1
	SET @j = 0	
	SET @GenreName = ''
END
SET @i = 0

----------Fill logins table///////////////////////////////////////////////

DECLARE @UserName VARCHAR(50) = ''
DECLARE @UserPassword VARCHAR(50)
DECLARE @UserRole INT 
DECLARE @UserEmail VARCHAR(15)
DECLARE @UserPhone VARCHAR(15) = '+'

DECLARE @FirstPartEmail VARCHAR(15)=''

DECLARE @SecondPartEmail VARCHAR(15)=''

DECLARE @PasswordLimit INT
DECLARE @StringLimit INT

WHILE @i < @CountForBigTable
BEGIN
	SET @PasswordLimit = 5 + RAND() * 5
	SET @UserPassword = SUBSTRING(convert(varchar(36),newid()),1,@PasswordLimit)	
	SET @UserRole = RAND()*2
	DECLARE @number INT = 10000000 + RAND()*999999999
	SET @UserPhone += CONVERT(VARCHAR(15),@number)

	SET @StringLimit = 5 + RAND() * 5
	
	WHILE @j < @StringLimit
	BEGIN
		SET @Position = 1 + RAND() * 51

		SET @UserName += SUBSTRING(@Symbol, @Position, 1)
		SET @FirstPartEmail += SUBSTRING(@Symbol, @Position - 1, 1)
		SET @SecondPartEmail += SUBSTRING(@Symbol, @Position + 1, 1)
		SET @j += 1
	END
	SET @UserEmail = @FirstPartEmail+'@'+@SecondPartEmail
	INSERT INTO logins(name, password, roleId, email, phone)

	VALUES (@UserName, @UserPassword, @UserRole,
	 @UserEmail, @UserPhone)
	SET @CountLogins += 1
	SET @i += 1
	SET @j = 0	
	SET @UserName = ''
	SET @UserPassword = ''
	SET @UserRole = ''
	SET @UserEmail = ''
	SET @FirstPartEmail = ''
	SET @SecondPartEmail = ''
	SET @UserPhone = '+'
END
SET @i = 0

------------Fill plays table ///////////////////////////////////

DECLARE @PlayName VARCHAR(15) = ''
DECLARE @AuthorId INT
DECLARE @GenreId INT
DECLARE @Description VARCHAR(MAX) = ''
DECLARE @DescriptionLimit INT

WHILE @i < @CountForBigTable
BEGIN
	
	SET @AuthorId = 1 + RAND() * (@CountAuthors - 1)
	SET @GenreId = 1 + RAND() * (@CountGenres - 1)
	
	SET @NameLimit = 5 + RAND() * 5
	SET @DescriptionLimit = 20 + RAND() * 250
	WHILE @j < @NameLimit
	BEGIN
		SET @Position = RAND() * 52
		SET @PlayName += SUBSTRING(@Symbol, @Position, 1)
		SET @j += 1
	END

	WHILE @j < @DescriptionLimit
	BEGIN
		SET @Position = RAND() * 52
		SET @Description += SUBSTRING(@Symbol, @Position, 1)
		if(@j % 10 = 0)
		BEGIN
			SET @Description += ' '
		END
		SET @j += 1
	END

	INSERT INTO plays(name, authorId, genreId, description)
	VALUES(@PlayName, @AuthorId, @GenreId, @Description)
	SET @i += 1
	SET @CountPlays += 1
	SET @j = 0	
	SET @PlayName = ''
	SET @Description = ''	
END
SET @i = 0

------Fill dates table ////////////////////////////////////
DECLARE @Date smalldatetime = '2016-10-19'
DECLARE @PlayId INT 

WHILE @i < @CountForBigTable
BEGIN
	SET @Date = DATEADD(day, 1, @Date)
	SET @PlayId = 1 + RAND() * (@CountPlays - 1)

	INSERT INTO dates(playsId, date)
	VALUES(@PlayId, @Date)
	SET @i += 1
	SET @CountDates += 1
END
SET @i = 0


----------Fill orders table ///////////////////////////////////

DECLARE @CategoryId INT
DECLARE @DateId INT
DECLARE @LoginId INT
DECLARE @Quantity INT
DECLARE @Price money
DECLARE @StatusOrderId INT

WHILE @i < @CountForBigTable
BEGIN
	SET @DateId = 1 + RAND()*(@CountDates - 1)
	SET @LoginId = 1 + RAND()*(@Countlogins - 1)

	SET @CategoryId = RAND() * 2
	SET @Quantity = 1 + RAND() * 3
	
	SET @Price = ((40000 + RAND() * 60000) * (@CategoryId + 1) * @Quantity)
	SET @StatusOrderId = RAND()*3
	
	INSERT INTO orders(dateId, loginId, categoryId, quantity, price, statusOrderId)
	VALUES(@DateId, @LoginId, @CategoryId, @Quantity, @Price, @StatusOrderId)
	SET @i += 1
	SET @CountOrders += 1
END
SET @i = 0

