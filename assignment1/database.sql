USE [Decompressor];

-- DROP PROCEDURE [dbo].[brb_CRUD_user];
-- DROP TYPE [dbo].[UserType];

-- ************************* PART 1 - CRUD *************************

CREATE TYPE [dbo].[UserType] AS TABLE(
    [user_id] INT NULL,
    [user_type_id] INT NULL,
    [username] NVARCHAR(200) NULL,
    [password] NVARCHAR(MAX) NULL,
    [session] NVARCHAR(MAX) NULL,
    [date_of_birth] DATE NULL,
    [first_name] NVARCHAR(MAX) NULL,
    [last_name] NVARCHAR(MAX) NULL
)
GO

CREATE PROCEDURE [dbo].[brb_CRUD_user]
(
    @type TINYINT,
    @table UserType READONLY
)
AS
BEGIN

    IF (@type = 0) -- CREATE
    BEGIN

        DECLARE @output TABLE (id INT);

        INSERT INTO [dbo].[user]([user_type_id], [username], [password], [session], [date_of_birth], [first_name], [last_name])
        OUTPUT inserted.user_id INTO @output
        SELECT 1, [username], [password], [session], [date_of_birth], [first_name], [last_name] FROM @table

        SELECT * FROM @output;

    END
    ELSE IF (@type = 1) -- READ
    BEGIN

        SELECT  [user_id], [user_type_id], [username], [password], [session], [date_of_birth], [first_name], [last_name]
        FROM    [dbo].[user];

    END
    ELSE IF (@type = 2) -- UPDATE
    BEGIN

        UPDATE  base
        SET     base.[user_type_id] = t.[user_type_id],
                base.[password] = t.[password],
                base.[session] = t.[session],
                base.[date_of_birth] = t.[date_of_birth],
                base.[first_name] = t.[first_name],
                base.[last_name] = t.[last_name]
        FROM    [dbo].[user] base JOIN @table t ON base.user_id = t.user_id;

    END
    ELSE IF (@type = 3) -- DELETE
    BEGIN

        PRINT 'No can do amigo :c';
        -- No can do amigo :c
        -- DELETE c FROM client c JOIN @table t ON c.id = t.id

    END

END
GO

CREATE TYPE [dbo].[AuctionType] AS TABLE(
	[auction_id] [int] NULL,
	[user_id] [int] NULL,
	[auction_name] [nvarchar](max) NULL,
	[start_price] [decimal](8, 2) NULL,
	[buy_now_price] [decimal](8, 2) NULL,
	[start_date] [datetime2](0) NULL,
	[end_date] [datetime2](0) NULL,
	[comission] [decimal](8, 2) NULL,
	[tax] [decimal](8, 2) NULL,
	[discount_percentage] [decimal](8, 2) NULL
)
GO

CREATE PROCEDURE [dbo].[brb_CRUD_auction]
(
    @type TINYINT,
    @table AuctionType READONLY 
)
AS
BEGIN

    IF (@type = 0) -- CREATE
    BEGIN

        INSERT INTO auction(user_id, auction_name, start_price, buy_now_price, [start_date], end_date, comission, tax, discount_percentage)
        SELECT user_id, auction_name, start_price, buy_now_price, [start_date], end_date, comission, tax, discount_percentage FROM @table

    END
    ELSE IF (@type = 1) -- READ
    BEGIN

        SELECT  auction_id, user_id, auction_name, start_price, buy_now_price, [start_date], end_date, comission, tax, discount_percentage
        FROM    auction;

    END
    ELSE IF (@type = 2) -- UPDATE
    BEGIN

        UPDATE  base
        SET     base.auction_name = t.auction_name,
                base.start_price = t.start_price,
                base.buy_now_price = t.buy_now_price,
                base.[start_date] = t.[start_date],
                base.end_date = t.end_date,
                base.comission = t.comission,
                base.tax = t.tax,
                base.discount_percentage = t.discount_percentage
        FROM    auction base JOIN @table t ON base.auction_id = t.auction_id

    END
    ELSE IF (@type = 3) -- DELETE
    BEGIN

        PRINT 'No can do amigo :c';
        -- DELETE f FROM flight f JOIN @table t ON f.flight_number = t.flight_number

    END

END
GO

CREATE TYPE [dbo].[BidType] AS TABLE(
	[id] [int] NOT NULL,
	[auction_id] [int] NOT NULL,
	[user_id] [int] NOT NULL,
	[bid_date] [datetime2](0) NOT NULL,
	[bid_amount] [decimal](8, 2) NOT NULL
)
GO

CREATE PROCEDURE [dbo].[brb_CRUD_bid]
(
    @type TINYINT,
    @table BidType READONLY 
)
AS
BEGIN

    IF (@type = 0) -- CREATE
    BEGIN

        INSERT INTO bid(auction_id, user_id, bid_date, bid_amount)
        SELECT auction_id, user_id, bid_date, bid_amount FROM @table

    END
    ELSE IF (@type = 1) -- READ
    BEGIN

        SELECT  id, auction_id, [user_id], bid_date, bid_amount
        FROM    bid;

    END
    ELSE IF (@type = 2) -- UPDATE
    BEGIN

        PRINT 'No can do amigo :c
You should not be able to modify a bid :s';
        -- UPDATE  b
        -- SET     b.booking_date = t.booking_date,
        --         b.flight_number = t.flight_number,
        --         b.client_id = t.client_id
        -- FROM    booking b JOIN @table t ON b.id = t.id

    END
    ELSE IF (@type = 3) -- DELETE
    BEGIN

        PRINT 'No can do amigo :c';
        -- DELETE b FROM booking b JOIN @table t ON b.id = t.id

    END

END


-- ************************* PART 2 - Auth *************************
GO
CREATE PROCEDURE [dbo].[brb_login]
(
    @username NVARCHAR(200)
)
AS
BEGIN

    IF NOT EXISTS (SELECT * FROM [user] WHERE username = @username)
    BEGIN
        RETURN;
    END

    -- SELECT * INTO #us FROM [user] WHERE username = @username;

    -- DECLARE @session NVARCHAR(MAX);
    -- SELECT @session = [session] FROM #us;

    UPDATE  [user]
    SET     [session] = NULL
    WHERE   username = @username AND [session] IS NOT NULL

    SELECT * FROM [user] WHERE username = @username;

END

GO
CREATE PROCEDURE [dbo].[brb_recover_session]
(
    @session NVARCHAR(MAX)
)
AS
BEGIN

    SELECT * INTO #us FROM [user] WHERE [session] = @session

    IF ((SELECT COUNT(*) FROM #us) = 1)
    BEGIN
        SELECT * FROM #us;
    END


END

GO
CREATE PROCEDURE [dbo].[brb_set_session]
(
    @username NVARCHAR(MAX),
    @session NVARCHAR(MAX)
)
AS
BEGIN

    UPDATE  [dbo].[user]
    SET     [session] = @session
    WHERE   [username] = @username

END

-- ************************* PART 3 - Home *************************

GO
CREATE PROCEDURE [dbo].[brb_get_menus]
(
    @user_type_id INT
)
AS
BEGIN

    SELECT  * 
    FROM    menus 
    WHERE   menu_id IN (SELECT menu_id 
                        FROM menu_user_type 
                        WHERE user_type_id = @user_type_id);

END

GO 
CREATE PROCEDURE [dbo].[brb_get_last_auctions]
(
    @option TINYINT
)
AS
BEGIN

    IF (@option = 0)
    BEGIN

        SELECT  a.auction_id, COUNT(*) AS bids
            INTO #total_bids
        FROM    auction a LEFT JOIN bid b ON a.auction_id = b.auction_id
        WHERE   end_date > CAST(GETDATE() AS DATE)
        GROUP BY a.auction_id;

        SELECT TOP 50 * FROM auction WHERE auction_id IN (SELECT TOP 3 auction_id FROM #total_bids)

    END
    ELSE IF (@option = 1)
    BEGIN

        SELECT TOP 50 * FROM auction WHERE end_date > CAST(GETDATE() AS DATE) ORDER BY [start_date] DESC

    END
    ELSE IF (@option = 2)
    BEGIN

        SELECT TOP 50 * FROM auction WHERE end_date > CAST(GETDATE() AS DATE) ORDER BY [start_date] DESC

    END

END

GO
CREATE PROCEDURE [dbo].[brb_auction_search]
(
    @search NVARCHAR(MAX)
)
AS
BEGIN

    SELECT * FROM auction WHERE auction_name LIKE CONCAT('%', @search, '%')

END

-- ************************* PART 4 - Auctions *************************

GO
CREATE PROCEDURE [dbo].[brb_get_auction]
(
    @auction_id INT
)
AS
BEGIN

    SELECT * FROM auction WHERE auction_id = @auction_id;
    SELECT * FROM bid WHERE auction_id = @auction_id;

END