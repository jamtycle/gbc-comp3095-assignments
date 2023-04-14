USE [Decompressor];

-- DROP PROCEDURE [dbo].[brb_CRUD_user];
-- DROP TYPE [dbo].[UserType];

-- ************************* PART 1 - CRUD *************************
-- DROP TYPE [dbo].[UserType];

CREATE TYPE [dbo].[UserType] AS TABLE(
    [user_id] INT NULL,
    [user_type_id] INT NULL,
    [username] NVARCHAR(200) NULL,
    [password] NVARCHAR(MAX) NULL,
    [session] NVARCHAR(MAX) NULL,
    [date_of_birth] DATE NULL,
    [first_name] NVARCHAR(200) NULL,
    [last_name] NVARCHAR(200) NULL,
    [validation_key] NVARCHAR(MAX) NULL, 
    [email] NVARCHAR(200) NULL, 
    [profile_pic] IMAGE
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

        INSERT INTO [dbo].[user]([user_type_id], [username], [password], [session], [date_of_birth], [first_name], [last_name], [validation_key], [email], [profile_pic])
        OUTPUT inserted.user_id INTO @output
        SELECT 1, [username], [password], [session], [date_of_birth], [first_name], [last_name], [validation_key], [email], [profile_pic] FROM @table

        SELECT * FROM @output;

    END
    ELSE IF (@type = 1) -- READ
    BEGIN

        SELECT  [user_id], [user_type_id], [username], [password], [session], [date_of_birth], [first_name], [last_name]
        FROM    [dbo].[user]
        WHERE   [user_id] = -1;

    END
    ELSE IF (@type = 2) -- UPDATE
    BEGIN

        UPDATE  base
        SET     base.[user_type_id] = t.[user_type_id],
                base.[password] = t.[password],
                base.[session] = t.[session],
                base.[date_of_birth] = t.[date_of_birth],
                base.[first_name] = t.[first_name],
                base.[last_name] = t.[last_name],
                base.[validation_key] = t.[validation_key],
                base.[email] = t.[email],
                base.[profile_pic] = t.[profile_pic]
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

-- SELECT * FROM auction

-- DROP TYPE [AuctionType]

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
	[discount_percentage] [decimal](8, 2) NULL,
    [condition] NVARCHAR(MAX) NULL,
    [description] NVARCHAR(MAX) NULL,
    [image] IMAGE NULL
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

        DECLARE @output TABLE (id INT);

        INSERT INTO auction(user_id, auction_name, start_price, buy_now_price, [start_date], end_date, condition, [description], [image])
        OUTPUT inserted.auction_id INTO @output
        SELECT TOP 1 user_id, auction_name, start_price, buy_now_price, [start_date], end_date, condition, [description], [image] FROM @table

        SELECT * FROM @output;
        -- comission, tax, discount_percentage
        -- comission, tax, discount_percentage

    END
    ELSE IF (@type = 1) -- READ
    BEGIN

        SELECT  *
        FROM    auction
        WHERE   auction_id = -1;

    END
    ELSE IF (@type = 2) -- UPDATE
    BEGIN

        UPDATE  base
        SET     base.auction_name = t.auction_name,
                base.start_price = t.start_price,
                base.buy_now_price = t.buy_now_price,
                base.[start_date] = t.[start_date],
                base.end_date = t.end_date,
                -- base.comission = t.comission,
                -- base.tax = t.tax,
                -- base.discount_percentage = t.discount_percentage,
                base.condition = t.condition,
                base.[description] = t.[description],
                base.[image] = t.[image]
        FROM    auction base JOIN @table t ON base.auction_id = t.auction_id

    END
    ELSE IF (@type = 3) -- DELETE
    BEGIN

        DELETE base FROM auction base JOIN @table t ON base.auction_id = t.auction_id
        -- PRINT 'No can do amigo :c';
        -- DELETE f FROM flight f JOIN @table t ON f.flight_number = t.flight_number

    END

END
GO

CREATE TYPE [dbo].[BidType] AS TABLE(
	[id] [int] NOT NULL,
	[auction_id] [int] NOT NULL,
	[user_id] [int] NOT NULL,
	[bid_date] [datetime2](0) NOT NULL,
	[bid_amount] [decimal](8, 2) NOT NULL,
    [buyed_now] [bit] NOT NULL
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

        INSERT INTO bid(auction_id, user_id, bid_date, bid_amount, buyed_now)
        SELECT auction_id, user_id, bid_date, bid_amount, buyed_now FROM @table

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


GO
CREATE TYPE [ReviewType] AS TABLE (
	[id] [int] NULL,
	[auction_id] [int] NULL,
	[user_id] [int] NULL,
    [user_rating_id] [int] NOT NULL,
	[rating] [int] NULL
)

GO
CREATE PROCEDURE [brb_CRUD_review] 
(
    @type TINYINT,
    @table ReviewType READONLY
)
AS
BEGIN
    
    IF (@type = 0) -- CREATE
    BEGIN

        INSERT INTO [dbo].[review]([auction_id], [user_id], [user_rating_id], [rating])
        SELECT auction_id, user_id, user_rating_id, rating FROM @table

    END
    ELSE IF (@type = 1) -- READ
    BEGIN

        SELECT  id, auction_id, user_id, user_rating_id, rating
        FROM    [dbo].[review]

    END
    ELSE IF (@type = 2) -- UPDATE
    BEGIN
        PRINT 'No can do amigo :c';
    END
    ELSE IF (@type = 3) -- DELETE
    BEGIN
        PRINT 'No can do amigo :c';
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

    SELECT * FROM [user] WHERE username = @username AND [validation_key] IS NULL;

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
        WHERE   end_date >= CAST(GETDATE() AS DATE)
        GROUP BY a.auction_id;

        SELECT TOP 50 * FROM auction WHERE auction_id IN (SELECT TOP 3 auction_id FROM #total_bids WHERE [image] IS NOT NULL)

    END
    ELSE IF (@option = 1)
    BEGIN

        SELECT  TOP 50 a.*,
                CAST((SELECT COUNT(*) FROM bid b WHERE a.auction_id = b.auction_id AND b.buyed_now = 1) AS BIT) AS [has_been_buyed]
        FROM    auction a
        WHERE   end_date >= CAST(GETDATE() AS DATE) ORDER BY [start_date] DESC

    END
    ELSE IF (@option = 2)
    BEGIN

        SELECT  TOP 100 a.*,
                CAST((SELECT COUNT(*) FROM bid b WHERE a.auction_id = b.auction_id AND b.buyed_now = 1) AS BIT) AS [has_been_buyed]
        FROM    auction a
        WHERE   end_date >= CAST(GETDATE() AS DATE) ORDER BY [start_date] DESC

    END
    ELSE IF (@option = 3)
    BEGIN
        SELECT  TOP 50 * 
        FROM    auction 
        WHERE   end_date <= CAST(GETDATE() AS DATE) ORDER BY [start_date] DESC
    END

END

GO
CREATE PROCEDURE [dbo].[brb_auction_search]
(
    @search NVARCHAR(MAX),
    @condition NVARCHAR(MAX) = 'All',
    @min_price FLOAT = 0,
    @max_price FLOAT = 1000,
    @status NVARCHAR(MAX) = 'all'
)
AS
BEGIN

    SELECT  a.*,
            CAST((SELECT COUNT(*) FROM bid b WHERE a.auction_id = b.auction_id AND b.buyed_now = 1) AS BIT) AS [has_been_buyed]
        INTO #pre_search
    FROM    auction a
    WHERE   auction_name LIKE CONCAT('%', @search, '%') 
            AND end_date >= CAST(GETDATE() AS DATE)
            AND condition = IIF(@condition = 'All', condition, @condition)
            AND (buy_now_price BETWEEN @min_price AND @max_price)
    ORDER BY [start_price] 

    DECLARE @has_been_buyed BIT = IIF(@status = 'finished', 1, 0)

    SELECT * FROM #pre_search WHERE has_been_buyed = IIF(@status = 'all', has_been_buyed, @has_been_buyed)

END

-- ************************* PART 4 - Auctions *************************

GO
CREATE PROCEDURE [dbo].[brb_get_auction]
(
    @auction_id INT
)
AS
BEGIN

    SELECT  auction_id, user_id, auction_name, start_price, buy_now_price, [start_date], 
            end_date, comission, tax, discount_percentage, condition, [description]
    FROM    auction a
    WHERE   auction_id = @auction_id;

    SELECT  b.id, b.auction_id, b.user_id, u.username, b.bid_date, CAST(b.bid_amount AS FLOAT) AS bid_amount, b.buyed_now 
    FROM    bid b JOIN [user] u ON b.user_id = u.user_id 
    WHERE   auction_id = @auction_id;

END

GO
CREATE PROCEDURE [dbo].[brb_get_auction_image]
(
    @auction_id INT
)
AS
BEGIN

    SELECT [image] FROM auction WHERE auction_id = @auction_id

END

-- ************************* PART 5 - Users *************************

GO
CREATE PROCEDURE [dbo].[brb_get_user]
(
    @user_id INT
)
AS
BEGIN

    SELECT  user_id, user_type_id, username, [session], date_of_birth, first_name, last_name, email, two_factor_auth
    FROM    [user] 
    WHERE   user_id = @user_id;

END

GO
CREATE PROCEDURE [dbo].[brb_get_user_by_email]
(
    @email NVARCHAR(MAX)
)
AS
BEGIN

    SELECT  user_id, user_type_id, username, [session], first_name, last_name
    FROM    [user] 
    WHERE   email = @email;

END

GO
CREATE PROCEDURE [dbo].[brb_get_user_pic]
(
    @user_id INT
)
AS
BEGIN

    SELECT  profile_pic
    FROM    [user] 
    WHERE   user_id = @user_id;

END

GO
CREATE PROCEDURE [dbo].[brb_set_password_reset]
(
    @user_id INT,
    @reset_code NVARCHAR(MAX)
)
AS
BEGIN
    
    UPDATE  [user]
    SET     reset_code = @reset_code
    WHERE   [user_id] = @user_id

END

GO
CREATE PROCEDURE [dbo].[brb_validate_password_reset]
(
    @user_id INT,
    @reset_code NVARCHAR(MAX)
)
AS
BEGIN
    
    SELECT user_id FROM [user] WHERE user_id = @user_id AND reset_code = @reset_code

END

GO
CREATE PROCEDURE [dbo].[brb_consume_password_reset]
(
    @user_id INT,
    @reset_code NVARCHAR(MAX),
    @password NVARCHAR(MAX)
)
AS
BEGIN

    UPDATE  [user]
    SET     [password] = @password,
            reset_code = NULL
    WHERE   user_id = @user_id
            AND reset_code = @reset_code

END

GO
CREATE PROCEDURE [dbo].[brb_consume_password_reset]
(
    @user_id INT,
    @reset_code NVARCHAR(MAX),
    @password NVARCHAR(MAX)
)
AS
BEGIN

    UPDATE  [user]
    SET     [password] = @password,
            reset_code = NULL
    WHERE   user_id = @user_id
            AND reset_code = @reset_code

END

GO
CREATE PROCEDURE [dbo].[brb_consume_password_reset]
(
    @user_id INT,
    @reset_code NVARCHAR(MAX),
    @password NVARCHAR(MAX)
)
AS
BEGIN

    UPDATE  [user]
    SET     [password] = @password,
            reset_code = NULL
    WHERE   user_id = @user_id
            AND reset_code = @reset_code

END

GO
CREATE PROCEDURE [dbo].[brb_tf_set_code]
(
    @user_id INT,
    @tf_code NVARCHAR(MAX)
)
AS
BEGIN

    IF ((SELECT two_factor_auth FROM [user] WHERE [user_id] = @user_id) = 0)
    BEGIN
        RETURN;
    END

    UPDATE  [user]
    SET     two_factor_code = @tf_code
    WHERE   user_id = @user_id

END

GO
ALTER PROCEDURE [dbo].[brb_tf_get_code]
(
    @user_id INT
)
AS
BEGIN

    IF (SELECT two_factor_auth FROM [user] WHERE [user_id] = @user_id) = 0
    BEGIN
        RETURN;
    END

    SELECT two_factor_code FROM [user] WHERE user_id = @user_id

END

GO
CREATE PROCEDURE [dbo].[brb_get_user_types]
AS
BEGIN
    SELECT * FROM user_type;
END

GO
CREATE PROCEDURE [dbo].[brb_user_validation_key]
(
    @key NVARCHAR(MAX)
)
AS
BEGIN

    UPDATE  [user]
    SET     [validation_key] = NULL
    WHERE   [validation_key] = @key

END

GO
CREATE PROCEDURE [dbo].[brb_get_all_users]
AS
BEGIN

    SELECT user_id, username, date_of_birth, email, first_name, last_name, user_type_id FROM [user] WHERE user_type_id IN (1, 2)

END


-- ************************* PART 6 - Reviews *************************

GO
CREATE PROCEDURE [brb_get_reviews_by_user]
(
    @user_id INT
)
AS
BEGIN

    SELECT  r.auction_id, u.user_id, r.user_rating_id, ur.username, r.rating 
    FROM    review r JOIN [user] u ON r.user_id = u.user_id
                     JOIN [user] ur ON r.user_rating_id = ur.user_id
    WHERE   r.user_id = @user_id

END