USE [pypdb]
GO
/****** Object:  UserDefinedFunction [dbo].[udf_Split]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[udf_Split] ( @String VARCHAR(MAX), @Delimiter VARCHAR(5)) 
RETURNS @Tokens table 
(
Token NVARCHAR(MAX)
) 
AS 
BEGIN
	WHILE (CHARINDEX(@Delimiter,@String)>0)
	BEGIN 
		INSERT INTO @Tokens (Token) VALUES (LTRIM(RTRIM(SUBSTRING(@String,1,CHARINDEX(@Delimiter,@String)-1))))
		SET @String = SUBSTRING(@String, CHARINDEX(@Delimiter,@String)+LEN(@Delimiter),LEN(@String))
	END
	INSERT INTO @Tokens (Token) VALUES (LTRIM(RTRIM(@String)))
	DELETE FROM @Tokens where LEN((LTRIM(RTRIM(Token)))) = 0
RETURN
END
GO
/****** Object:  Table [dbo].[tb_log]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tb_log](
	[content] [varchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tb_QueueAction]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tb_QueueAction](
	[QueueActionID] [bigint] IDENTITY(7700000000000000000,1) NOT NULL,
	[UserID] [bigint] NOT NULL,
	[GUID] [varchar](128) NOT NULL,
	[CreatedWhen] [datetime] NOT NULL CONSTRAINT [DF_tb_QueueAction_CreatedWhen]  DEFAULT (getutcdate()),
 CONSTRAINT [PK_tb_QueueAction] PRIMARY KEY CLUSTERED 
(
	[QueueActionID] ASC,
	[UserID] ASC,
	[GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tb_used_nonce]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tb_used_nonce](
	[LoginType] [varchar](15) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[RequestTime] [datetime] NOT NULL,
	[Nonce] [int] NOT NULL,
 CONSTRAINT [PK_tb_used_request_nonce_1] PRIMARY KEY CLUSTERED 
(
	[LoginType] ASC,
	[UserName] ASC,
	[RequestTime] ASC,
	[Nonce] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tb_user]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tb_user](
	[ID] [bigint] IDENTITY(7700000000000000000,1) NOT NULL,
	[LoginType] [varchar](15) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[SocialEmail] [varchar](50) NULL,
	[DisplayName] [nvarchar](200) NOT NULL,
	[ProfilePictureURL] [varchar](200) NULL CONSTRAINT [DF_tb_user_ProfilePictureURL]  DEFAULT (''),
	[Password] [varchar](50) NOT NULL CONSTRAINT [DF_tb_user_Password]  DEFAULT (''),
	[MobilePlatform] [varchar](10) NOT NULL CONSTRAINT [DF_tb_user_MobilePlatform]  DEFAULT (''),
	[PushNotificationID] [varchar](200) NOT NULL CONSTRAINT [DF_tb_user_PushNotificationID]  DEFAULT (''),
	[CreatedWhen] [datetime] NOT NULL CONSTRAINT [DF_tb_user_CreatedWhen]  DEFAULT (getutcdate()),
	[TouchedWhen] [datetime] NOT NULL CONSTRAINT [DF_tb_user_TouchedWhen]  DEFAULT (getutcdate()),
	[HMACHashKey] [varchar](128) NOT NULL,
	[EmailVerification] [bit] NOT NULL CONSTRAINT [DF_tb_user_EmailVerification]  DEFAULT ((0)),
	[Country] [varchar](50) NULL,
	[Region] [varchar](100) NULL,
	[City] [varchar](100) NULL,
 CONSTRAINT [PK_tb_user_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tb_user_friends]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_user_friends](
	[UserID] [bigint] NOT NULL,
	[FriendID] [bigint] NOT NULL,
 CONSTRAINT [PK_tb_user_friends] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC,
	[FriendID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tb_user_onetimecode]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tb_user_onetimecode](
	[UserID] [bigint] NOT NULL,
	[Purpose] [varchar](50) NOT NULL,
	[RequestID] [bigint] IDENTITY(7700000000000000000,1) NOT NULL,
	[ActivationCode] [varchar](100) NOT NULL,
	[CreatedWhen] [datetime] NULL,
	[Expired] [bit] NULL,
 CONSTRAINT [PK_tb_user_onetimecode] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC,
	[Purpose] ASC,
	[RequestID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tb_user_prayer]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tb_user_prayer](
	[TouchedWhen] [bigint] NOT NULL CONSTRAINT [DF_tb_user_prayer_TouchedWhen]  DEFAULT (datediff(second,'1970-01-01 00:00:00',getutcdate())),
	[CreatedWhen] [bigint] NOT NULL CONSTRAINT [DF_tb_user_prayer_CreatedWhen]  DEFAULT (datediff(second,'1970-01-01 00:00:00',getutcdate())),
	[UserID] [bigint] NOT NULL,
	[PrayerID] [bigint] IDENTITY(7700000000000000000,1) NOT NULL,
	[PrayerContent] [nvarchar](max) NOT NULL,
	[PublicView] [bit] NOT NULL CONSTRAINT [DF_tb_user_prayer_PublicView]  DEFAULT ((0)),
	[Deleted] [bit] NOT NULL CONSTRAINT [DF_tb_user_prayer_Deleted]  DEFAULT ((0)),
	[QueueActionGUID] [varchar](128) NULL,
 CONSTRAINT [PK_tb_user_prayer] PRIMARY KEY CLUSTERED 
(
	[PrayerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tb_user_prayer_amen]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_user_prayer_amen](
	[AmenID] [bigint] IDENTITY(7700000000000000000,1) NOT NULL,
	[PrayerID] [bigint] NOT NULL,
	[UserID] [bigint] NOT NULL,
	[CreatedWhen] [bigint] NOT NULL,
	[TouchedWhen] [bigint] NOT NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_tb_user_prayer_amen] PRIMARY KEY CLUSTERED 
(
	[AmenID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tb_user_prayer_answered]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tb_user_prayer_answered](
	[AnsweredID] [bigint] IDENTITY(7700000000000000000,1) NOT NULL,
	[PrayerID] [bigint] NOT NULL,
	[UserID] [bigint] NOT NULL,
	[Answered] [nvarchar](max) NOT NULL,
	[CreatedWhen] [bigint] NOT NULL,
	[TouchedWhen] [bigint] NOT NULL,
	[Deleted] [bit] NOT NULL,
	[QueueActionGUID] [varchar](128) NOT NULL,
 CONSTRAINT [PK_tb_user_prayer_answered] PRIMARY KEY CLUSTERED 
(
	[AnsweredID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tb_user_prayer_attachment]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tb_user_prayer_attachment](
	[AttachmentID] [bigint] IDENTITY(7700000000000000000,1) NOT NULL,
	[PrayerID] [bigint] NOT NULL,
	[UserID] [bigint] NOT NULL,
	[FileName] [varchar](128) NOT NULL,
	[OriginalFilePath] [varchar](max) NOT NULL,
 CONSTRAINT [PK_tb_user_prayer_attachment_1] PRIMARY KEY CLUSTERED 
(
	[AttachmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tb_user_prayer_comment]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tb_user_prayer_comment](
	[CommentID] [bigint] IDENTITY(7700000000000000000,1) NOT NULL,
	[PrayerID] [bigint] NOT NULL,
	[UserID] [bigint] NOT NULL,
	[Comment] [nvarchar](max) NOT NULL,
	[CreatedWhen] [bigint] NOT NULL,
	[TouchedWhen] [bigint] NOT NULL,
	[Deleted] [bit] NOT NULL,
	[QueueActionGUID] [varchar](128) NOT NULL,
 CONSTRAINT [PK_tb_user_prayer_comment] PRIMARY KEY CLUSTERED 
(
	[CommentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tb_user_prayer_request]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tb_user_prayer_request](
	[TouchedWhen] [bigint] NOT NULL CONSTRAINT [DF_tb_prayer_request_TouchedWhen]  DEFAULT (datediff(second,'1970-01-01 00:00:00',getutcdate())),
	[CreatedWhen] [bigint] NOT NULL CONSTRAINT [DF_tb_prayer_request_CreatedWhen]  DEFAULT (datediff(second,'1970-01-01 00:00:00',getutcdate())),
	[UserID] [bigint] NOT NULL,
	[PrayerRequestID] [bigint] IDENTITY(7700000000000000000,1) NOT NULL,
	[Subject] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](4000) NULL,
	[Answered] [bit] NOT NULL CONSTRAINT [DF_tb_prayer_request_Answered]  DEFAULT ((0)),
	[AnswerComment] [nvarchar](4000) NULL,
	[AnsweredWhen] [bigint] NULL,
	[QueueActionGUID] [varchar](128) NULL,
	[Deleted] [bit] NOT NULL CONSTRAINT [DF_tb_user_prayer_request_Deleted]  DEFAULT ((0)),
 CONSTRAINT [PK_tb_user_prayer_request] PRIMARY KEY CLUSTERED 
(
	[PrayerRequestID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tb_user_prayer_request_attachment]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tb_user_prayer_request_attachment](
	[AttachmentID] [bigint] IDENTITY(7700000000000000000,1) NOT NULL,
	[PrayerRequestID] [bigint] NOT NULL,
	[UserID] [bigint] NOT NULL,
	[FileName] [varchar](128) NOT NULL,
	[OriginalFilePath] [varchar](max) NOT NULL,
 CONSTRAINT [PK_tb_user_prayer_request_attachment] PRIMARY KEY CLUSTERED 
(
	[AttachmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tb_user_prayer_tag_friends]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_user_prayer_tag_friends](
	[PrayerID] [bigint] NOT NULL,
	[UserID] [bigint] NOT NULL,
 CONSTRAINT [PK_tb_user_prayer_tag_friends] PRIMARY KEY CLUSTERED 
(
	[PrayerID] ASC,
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tb_user_testimony]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_user_testimony](
	[TouchedWhen] [datetime] NOT NULL,
	[CreatedWhen] [datetime] NOT NULL,
	[UserID] [bigint] NOT NULL,
	[TestimonyID] [bigint] IDENTITY(7700000000000000000,1) NOT NULL,
	[Testimony] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_tb_user_testimony_1] PRIMARY KEY CLUSTERED 
(
	[TestimonyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[tb_used_nonce] ADD  CONSTRAINT [DF_tb_used_nonce_UserID]  DEFAULT ('') FOR [LoginType]
GO
ALTER TABLE [dbo].[tb_user_onetimecode] ADD  CONSTRAINT [DF_tb_AccountActivationCode_ActivationCode]  DEFAULT (newid()) FOR [ActivationCode]
GO
ALTER TABLE [dbo].[tb_user_onetimecode] ADD  CONSTRAINT [DF_tb_AccountActivationCode_CreatedWhen]  DEFAULT (getutcdate()) FOR [CreatedWhen]
GO
ALTER TABLE [dbo].[tb_user_onetimecode] ADD  CONSTRAINT [DF_tb_user_onetimecode_Expired]  DEFAULT ((0)) FOR [Expired]
GO
ALTER TABLE [dbo].[tb_user_prayer_amen] ADD  CONSTRAINT [DF_tb_user_prayer_amen_CreatedWhen]  DEFAULT (datediff(second,'1970-01-01 00:00:00',getutcdate())) FOR [CreatedWhen]
GO
ALTER TABLE [dbo].[tb_user_prayer_amen] ADD  CONSTRAINT [DF_tb_user_prayer_amen_TouchedWhen]  DEFAULT (datediff(second,'1970-01-01 00:00:00',getutcdate())) FOR [TouchedWhen]
GO
ALTER TABLE [dbo].[tb_user_prayer_amen] ADD  CONSTRAINT [DF_tb_user_prayer_amen_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[tb_user_prayer_answered] ADD  CONSTRAINT [DF_tb_user_prayer_answered_CreatedWhen]  DEFAULT (datediff(second,'1970-01-01 00:00:00',getutcdate())) FOR [CreatedWhen]
GO
ALTER TABLE [dbo].[tb_user_prayer_answered] ADD  CONSTRAINT [DF_tb_user_prayer_answered_TouchedWhen]  DEFAULT (datediff(second,'1970-01-01 00:00:00',getutcdate())) FOR [TouchedWhen]
GO
ALTER TABLE [dbo].[tb_user_prayer_answered] ADD  CONSTRAINT [DF_tb_user_prayer_answered_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[tb_user_prayer_comment] ADD  CONSTRAINT [DF_tb_user_prayer_comment_CreatedWhen]  DEFAULT (datediff(second,'1970-01-01 00:00:00',getutcdate())) FOR [CreatedWhen]
GO
ALTER TABLE [dbo].[tb_user_prayer_comment] ADD  CONSTRAINT [DF_tb_user_prayer_comment_TouchedWhen]  DEFAULT (datediff(second,'1970-01-01 00:00:00',getutcdate())) FOR [TouchedWhen]
GO
ALTER TABLE [dbo].[tb_user_prayer_comment] ADD  CONSTRAINT [DF_tb_user_prayer_comment_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[tb_user_testimony] ADD  CONSTRAINT [DF_tb_user_testimony_TouchedWhen]  DEFAULT (getutcdate()) FOR [TouchedWhen]
GO
ALTER TABLE [dbo].[tb_user_testimony] ADD  CONSTRAINT [DF_tb_user_testimony_CreatedWhen]  DEFAULT (getutcdate()) FOR [CreatedWhen]
GO
ALTER TABLE [dbo].[tb_QueueAction]  WITH CHECK ADD  CONSTRAINT [FK_tb_QueueAction_tb_user] FOREIGN KEY([UserID])
REFERENCES [dbo].[tb_user] ([ID])
GO
ALTER TABLE [dbo].[tb_QueueAction] CHECK CONSTRAINT [FK_tb_QueueAction_tb_user]
GO
ALTER TABLE [dbo].[tb_user_friends]  WITH CHECK ADD  CONSTRAINT [FK_tb_user_friends_tb_user] FOREIGN KEY([UserID])
REFERENCES [dbo].[tb_user] ([ID])
GO
ALTER TABLE [dbo].[tb_user_friends] CHECK CONSTRAINT [FK_tb_user_friends_tb_user]
GO
ALTER TABLE [dbo].[tb_user_friends]  WITH CHECK ADD  CONSTRAINT [FK_tb_user_friends_tb_user1] FOREIGN KEY([FriendID])
REFERENCES [dbo].[tb_user] ([ID])
GO
ALTER TABLE [dbo].[tb_user_friends] CHECK CONSTRAINT [FK_tb_user_friends_tb_user1]
GO
ALTER TABLE [dbo].[tb_user_onetimecode]  WITH CHECK ADD  CONSTRAINT [FK_tb_AccountActivationCode_tb_user] FOREIGN KEY([UserID])
REFERENCES [dbo].[tb_user] ([ID])
GO
ALTER TABLE [dbo].[tb_user_onetimecode] CHECK CONSTRAINT [FK_tb_AccountActivationCode_tb_user]
GO
ALTER TABLE [dbo].[tb_user_prayer]  WITH CHECK ADD  CONSTRAINT [FK_tb_user_prayer_tb_user] FOREIGN KEY([UserID])
REFERENCES [dbo].[tb_user] ([ID])
GO
ALTER TABLE [dbo].[tb_user_prayer] CHECK CONSTRAINT [FK_tb_user_prayer_tb_user]
GO
ALTER TABLE [dbo].[tb_user_prayer_answered]  WITH CHECK ADD  CONSTRAINT [FK_tb_user_prayer_answered_tb_user] FOREIGN KEY([UserID])
REFERENCES [dbo].[tb_user] ([ID])
GO
ALTER TABLE [dbo].[tb_user_prayer_answered] CHECK CONSTRAINT [FK_tb_user_prayer_answered_tb_user]
GO
ALTER TABLE [dbo].[tb_user_prayer_answered]  WITH CHECK ADD  CONSTRAINT [FK_tb_user_prayer_answered_tb_user_prayer] FOREIGN KEY([PrayerID])
REFERENCES [dbo].[tb_user_prayer] ([PrayerID])
GO
ALTER TABLE [dbo].[tb_user_prayer_answered] CHECK CONSTRAINT [FK_tb_user_prayer_answered_tb_user_prayer]
GO
ALTER TABLE [dbo].[tb_user_prayer_attachment]  WITH CHECK ADD  CONSTRAINT [FK_tb_user_prayer_attachment_tb_user] FOREIGN KEY([UserID])
REFERENCES [dbo].[tb_user] ([ID])
GO
ALTER TABLE [dbo].[tb_user_prayer_attachment] CHECK CONSTRAINT [FK_tb_user_prayer_attachment_tb_user]
GO
ALTER TABLE [dbo].[tb_user_prayer_attachment]  WITH CHECK ADD  CONSTRAINT [FK_tb_user_prayer_attachment_tb_user_prayer] FOREIGN KEY([PrayerID])
REFERENCES [dbo].[tb_user_prayer] ([PrayerID])
GO
ALTER TABLE [dbo].[tb_user_prayer_attachment] CHECK CONSTRAINT [FK_tb_user_prayer_attachment_tb_user_prayer]
GO
ALTER TABLE [dbo].[tb_user_prayer_comment]  WITH CHECK ADD  CONSTRAINT [FK_tb_user_prayer_comment_tb_user] FOREIGN KEY([UserID])
REFERENCES [dbo].[tb_user] ([ID])
GO
ALTER TABLE [dbo].[tb_user_prayer_comment] CHECK CONSTRAINT [FK_tb_user_prayer_comment_tb_user]
GO
ALTER TABLE [dbo].[tb_user_prayer_comment]  WITH CHECK ADD  CONSTRAINT [FK_tb_user_prayer_comment_tb_user_prayer] FOREIGN KEY([PrayerID])
REFERENCES [dbo].[tb_user_prayer] ([PrayerID])
GO
ALTER TABLE [dbo].[tb_user_prayer_comment] CHECK CONSTRAINT [FK_tb_user_prayer_comment_tb_user_prayer]
GO
ALTER TABLE [dbo].[tb_user_prayer_request]  WITH CHECK ADD  CONSTRAINT [FK_tb_user_prayer_request_tb_user] FOREIGN KEY([UserID])
REFERENCES [dbo].[tb_user] ([ID])
GO
ALTER TABLE [dbo].[tb_user_prayer_request] CHECK CONSTRAINT [FK_tb_user_prayer_request_tb_user]
GO
ALTER TABLE [dbo].[tb_user_prayer_request_attachment]  WITH CHECK ADD  CONSTRAINT [FK_tb_user_prayer_request_attachment_tb_user] FOREIGN KEY([UserID])
REFERENCES [dbo].[tb_user] ([ID])
GO
ALTER TABLE [dbo].[tb_user_prayer_request_attachment] CHECK CONSTRAINT [FK_tb_user_prayer_request_attachment_tb_user]
GO
ALTER TABLE [dbo].[tb_user_prayer_request_attachment]  WITH CHECK ADD  CONSTRAINT [FK_tb_user_prayer_request_attachment_tb_user_prayer_request] FOREIGN KEY([PrayerRequestID])
REFERENCES [dbo].[tb_user_prayer_request] ([PrayerRequestID])
GO
ALTER TABLE [dbo].[tb_user_prayer_request_attachment] CHECK CONSTRAINT [FK_tb_user_prayer_request_attachment_tb_user_prayer_request]
GO
ALTER TABLE [dbo].[tb_user_prayer_tag_friends]  WITH CHECK ADD  CONSTRAINT [FK_tb_user_prayer_tag_friends_tb_user] FOREIGN KEY([UserID])
REFERENCES [dbo].[tb_user] ([ID])
GO
ALTER TABLE [dbo].[tb_user_prayer_tag_friends] CHECK CONSTRAINT [FK_tb_user_prayer_tag_friends_tb_user]
GO
ALTER TABLE [dbo].[tb_user_prayer_tag_friends]  WITH CHECK ADD  CONSTRAINT [FK_tb_user_prayer_tag_friends_tb_user_prayer] FOREIGN KEY([PrayerID])
REFERENCES [dbo].[tb_user_prayer] ([PrayerID])
GO
ALTER TABLE [dbo].[tb_user_prayer_tag_friends] CHECK CONSTRAINT [FK_tb_user_prayer_tag_friends_tb_user_prayer]
GO
ALTER TABLE [dbo].[tb_user_testimony]  WITH CHECK ADD  CONSTRAINT [FK_tb_user_testimony_tb_user] FOREIGN KEY([UserID])
REFERENCES [dbo].[tb_user] ([ID])
GO
ALTER TABLE [dbo].[tb_user_testimony] CHECK CONSTRAINT [FK_tb_user_testimony_tb_user]
GO
/****** Object:  StoredProcedure [dbo].[usp_aaaaa_mustdeleteTemporaryTesting]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_aaaaa_mustdeleteTemporaryTesting] 
AS
BEGIN
	SELECT GETUTCDATE();
END

GO
/****** Object:  StoredProcedure [dbo].[usp_ActivateUserAccount]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_ActivateUserAccount] 
(@ID BIGINT, @ActivationCode VARCHAR(100), @Result VARCHAR(200) OUTPUT, @DisplayName NVARCHAR(200) OUTPUT)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SET @DisplayName = NULL;

	SELECT @DisplayName = B.DisplayName FROM [dbo].[tb_user_onetimecode] (NOLOCK) AS A
	INNER JOIN [dbo].[tb_user] (NOLOCK) AS B ON A.UserID = B.ID AND B.EmailVerification = 0
	WHERE A.UserID = @ID AND A.ActivationCode = @ActivationCode AND A.Purpose = 'Account Activation'

	IF (@DisplayName IS NOT NULL)
	BEGIN
		SET @Result = 'OK';
		UPDATE [dbo].[tb_user] SET EmailVerification = 1 WHERE ID = @ID;
		UPDATE [dbo].[tb_user_onetimecode] SET Expired = 1 WHERE [UserID] = @ID AND ActivationCode = @ActivationCode AND Purpose = 'Account Activation'	
	END
	ELSE
	BEGIN
		SET @DisplayName = null;
		SET @Result = 'NOT EXISTS';
	END


END

GO
/****** Object:  StoredProcedure [dbo].[usp_AddLog]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_AddLog] 
(@LogText VARCHAR(MAx))
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [dbo].[tb_log](content)
	select @LogText
END

GO
/****** Object:  StoredProcedure [dbo].[usp_AddNewPrayer]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_AddNewPrayer] 
(@UserID BIGINT, @Prayer NVARCHAR(MAX), @CreatedWhen BIGINT, @TouchedWhen BIGINT, @PublicView BIT, @Friends XML, @QueueActionGUID VARCHAR(128), @Result VARCHAR(100) OUTPUT, @PrayerID BIGINT OUTPUT)
AS
BEGIN
	
	SET NOCOUNT ON;

	IF EXISTS(SELECT 1 FROM [dbo].[tb_QueueAction] (NOLOCK) WHERE [UserID] = @UserID AND [GUID] = @QueueActionGUID)
	BEGIN
		SELECT @Result = 'EXISTS-' + CONVERT(VARCHAR(20), PrayerID) FROM [dbo].[tb_user_prayer] WHERE QueueActionGUID = @QueueActionGUID
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[tb_user_prayer]([UserID], [PrayerContent], PublicView, CreatedWhen, TouchedWhen, QueueActionGUID)
		SELECT @UserID, @Prayer, @PublicView, @CreatedWhen, @TouchedWhen, @QueueActionGUID;

		SET @PrayerID = Scope_Identity();
	
		INSERT INTO [dbo].[tb_user_prayer_tag_friends](PrayerID, UserID)
		select @PrayerID, T.N.value('UserID[1]', 'bigint') as [Key]
		FROM @Friends.nodes('/Friends/*') as T(N);

		SET @Result = 'OK';
	END




    
END

GO
/****** Object:  StoredProcedure [dbo].[usp_AddNewPrayerAmen]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_AddNewPrayerAmen] 
(@QueueActionGUID VARCHAR(128), @UserID BIGINT, @PrayerID BIGINT, @Result VARCHAR(100) OUTPUT, @AmenID BIGINT OUTPUT)
AS
BEGIN
	IF EXISTS(SELECT 1 FROM [dbo].[tb_QueueAction] (NOLOCK) WHERE [UserID] = @UserID AND [GUID] = @QueueActionGUID) OR
	EXISTS(SELECT 1 FROM [dbo].[tb_user_prayer_amen] (NOLOCK) WHERE UserID = @UserID AND PrayerID = @PrayerID AND Deleted = 0)
	BEGIN
		SET @Result = 'EXISTS';
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[tb_QueueAction](UserID, [GUID])
		SELECT @UserID, @QueueActionGUID;

		INSERT INTO [dbo].[tb_user_prayer_amen](PrayerID, UserID)
		SELECT @PrayerID, @UserID

		SET @AmenID = Scope_Identity();

		SET @Result = 'OK';
	END
END


GO
/****** Object:  StoredProcedure [dbo].[usp_AddNewPrayerAnswered]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_AddNewPrayerAnswered] 
(@QueueActionGUID VARCHAR(128), @UserID BIGINT, @PrayerID BIGINT, @Answered NVARCHAR(MAX), @CreatedWhen BIGINT, @TouchedWhen BIGINT, @Result VARCHAR(100) OUTPUT, @AnsweredID BIGINT OUTPUT)
AS
BEGIN
	IF EXISTS(SELECT 1 FROM [dbo].[tb_QueueAction] (NOLOCK) WHERE [UserID] = @UserID AND [GUID] = @QueueActionGUID)
	BEGIN
		SELECT @Result = 'EXISTS-' + CONVERT(VARCHAR(20), AnsweredID) FROM [dbo].[tb_user_prayer_answered] WHERE QueueActionGUID = @QueueActionGUID
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[tb_QueueAction](UserID, [GUID])
		SELECT @UserID, @QueueActionGUID;

		INSERT INTO [dbo].[tb_user_prayer_answered](PrayerID, UserID, Answered, CreatedWhen, TouchedWhen, QueueActionGUID)
		SELECT @PrayerID, @UserID, @Answered, @CreatedWhen, @TouchedWhen, @QueueActionGUID

		SET @AnsweredID = Scope_Identity();

		SET @Result = 'OK';
	END
END


GO
/****** Object:  StoredProcedure [dbo].[usp_AddNewPrayerAttachment]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_AddNewPrayerAttachment]
(@PrayerID BIGINT, @Filename VARCHAR(128), @OriginalFilePath VARCHAR(MAX), @UserID BIGINT, @AttachmentID BIGINT OUTPUT)
AS
BEGIN
	IF NOT EXISTS(SELECT 1 FROM [dbo].[tb_user_prayer_attachment] (NOLOCK) WHERE PrayerID = @PrayerID AND [Filename] = @Filename)
	BEGIN
		INSERT INTO [dbo].[tb_user_prayer_attachment]([PrayerID], [Filename], [OriginalFilePath], [UserID])
		SELECT @PrayerID, @Filename, @OriginalFilePath, @UserID;

		SET @AttachmentID = Scope_Identity();
	END
	ELSE
	BEGIN
		SELECT @AttachmentID = AttachmentID FROM [dbo].[tb_user_prayer_attachment] WHERE PrayerID = @PrayerID AND [Filename] = @Filename;
	END
END

GO
/****** Object:  StoredProcedure [dbo].[usp_AddNewPrayerComment]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_AddNewPrayerComment] 
(@QueueActionGUID VARCHAR(128), @UserID BIGINT, @PrayerID BIGINT, @Comment NVARCHAR(MAX), @CreatedWhen BIGINT, @TouchedWhen BIGINT, @Result VARCHAR(100) OUTPUT, @CommentID BIGINT OUTPUT)
AS
BEGIN
	IF EXISTS(SELECT 1 FROM [dbo].[tb_QueueAction] (NOLOCK) WHERE [UserID] = @UserID AND [GUID] = @QueueActionGUID)
	BEGIN
		SELECT @Result = 'EXISTS-' + CONVERT(VARCHAR(20), CommentID) FROM [dbo].[tb_user_prayer_comment] WHERE QueueActionGUID = @QueueActionGUID
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[tb_QueueAction](UserID, [GUID])
		SELECT @UserID, @QueueActionGUID;

		INSERT INTO [dbo].[tb_user_prayer_comment](PrayerID, UserID, Comment, CreatedWhen, TouchedWhen, QueueActionGUID)
		SELECT @PrayerID, @UserID, @Comment, @CreatedWhen, @TouchedWhen, @QueueActionGUID

		SET @CommentID = Scope_Identity();

		SET @Result = 'OK';
	END
END


GO
/****** Object:  StoredProcedure [dbo].[usp_AddNewPrayerRequest]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_AddNewPrayerRequest] 
(@QueueActionGUID VARCHAR(128), @UserID BIGINT, @Subject NVARCHAR(40), @Description NVARCHAR(4000), @CreatedWhen BIGINT, @TouchedWhen BIGINT, @Result VARCHAR(100) OUTPUT, @PrayerRequestID BIGINT OUTPUT)
AS
BEGIN
	IF EXISTS(SELECT 1 FROM [dbo].[tb_QueueAction] (NOLOCK) WHERE [UserID] = @UserID AND [GUID] = @QueueActionGUID)
	BEGIN
		SELECT @Result = 'EXISTS-' + CONVERT(VARCHAR(20), PrayerRequestID) FROM [dbo].[tb_user_prayer_Request] (NOLOCK) WHERE QueueActionGUID = @QueueActionGUID
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[tb_QueueAction](UserID, [GUID])
		SELECT @UserID, @QueueActionGUID;

		INSERT INTO [dbo].[tb_user_prayer_Request](UserID, [Subject], [Description], CreatedWhen, TouchedWhen, QueueActionGUID)
		SELECT @UserID, @Subject, @Description, @CreatedWhen, @TouchedWhen, @QueueActionGUID

		SET @PrayerRequestID = Scope_Identity();

		SET @Result = 'OK';
	END
END


GO
/****** Object:  StoredProcedure [dbo].[usp_AddNewPrayerRequestAttachment]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_AddNewPrayerRequestAttachment]
(@PrayerRequestID BIGINT, @Filename VARCHAR(128), @OriginalFilePath VARCHAR(MAX), @UserID BIGINT, @AttachmentID BIGINT OUTPUT)
AS
BEGIN
	IF NOT EXISTS(SELECT 1 FROM [dbo].[tb_user_prayer_request_attachment] (NOLOCK) WHERE PrayerRequestID = @PrayerRequestID AND [Filename] = @Filename)
	BEGIN
		INSERT INTO [dbo].[tb_user_prayer_request_attachment](PrayerRequestID, [Filename], [OriginalFilePath], [UserID])
		SELECT @PrayerRequestID, @Filename, @OriginalFilePath, @UserID;

		SET @AttachmentID = Scope_Identity();
	END
	ELSE
	BEGIN
		SELECT @AttachmentID = AttachmentID FROM [dbo].[tb_user_prayer_request_attachment] WHERE PrayerRequestID = @PrayerRequestID AND [Filename] = @Filename;
	END
END

GO
/****** Object:  StoredProcedure [dbo].[usp_AddNewUser]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_AddNewUser] 
(@LoginType VARCHAR(15), @UserName VARCHAR(50), @Name NVARCHAR(200), @ProfilePictureURL VARCHAR(200), @Password VARCHAR(50),
 @MobilePlatform VARCHAR(10), @PushNotificationID VARCHAR(200), @HMACHashKey VARCHAR(128),
 @Country VARCHAR(50), @Region VARCHAR(100), @City VARCHAR(100),
 @Result VARCHAR(200) OUTPUT, @ID BIGINT OUTPUT, @VerificationCode VARCHAR(100) OUTPUT)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT 1 FROM [dbo].[tb_user] (NOLOCK) WHERE LoginType = @LoginType and UserName = @UserName)
	BEGIN
		
		DECLARE @EmailVerification BIT = NULL;

		IF(@LoginType = 'Email')
		BEGIN
			SET @EmailVerification = 0;
		END
		ELSE
		BEGIN
			SET @EmailVerification = 1;
		END

		INSERT INTO [dbo].[tb_user](LoginType, UserName, DisplayName, ProfilePictureURL, [Password], MobilePlatform, PushNotificationID, 
								    HMACHashKey, EmailVerification, Country, Region, City)
		SELECT @LoginType, @UserName, @Name, @ProfilePictureURL, @Password, @MobilePlatform, @PushNotificationID,
			   @HMACHashKey, @EmailVerification, @Country, @Region, @City;

		SELECT @ID = ID FROM [dbo].[tb_user] WHERE LoginType = @LoginType AND UserName = @UserName;


		IF(@LoginType = 'Email')
		BEGIN
			SET @VerificationCode = REPLACE(CONVERT(VARCHAR(36), NEWID()) + '-' + CONVERT(VARCHAR(36), NEWID()) + '-' + CONVERT(VARCHAR(36), NEWID()), '-','');
			INSERT INTO [dbo].[tb_user_onetimecode]([UserID], [Purpose], ActivationCode)
			SELECT @ID, 'Account Activation', @VerificationCode
		END

		SET @Result = 'OK';
		
	END
	ELSE
	BEGIN
		set @Result = 'EXISTS';
	END


END
GO
/****** Object:  StoredProcedure [dbo].[usp_AddNonce]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_AddNonce] 
(@LoginType VARCHAR(15), @Username VARCHAR(50), @RequestDate DATETIME, @Nonce INT, @Result VARCHAR(200) OUTPUT)
AS
BEGIN
	SET NOCOUNT ON;

    IF EXISTS(SELECT 1 FROM [dbo].[tb_used_nonce] (NOLOCK) WHERE LoginType = @LoginType AND UserName = @Username AND RequestTime = @RequestDate AND Nonce = @Nonce)
	BEGIN
		SET @Result = 'EXISTS';
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[tb_used_nonce](LoginType, UserName, RequestTime, Nonce)
		SELECT @LoginType, @Username, @RequestDate, @Nonce;

		SET @Result = 'OK';
	END
END

GO
/****** Object:  StoredProcedure [dbo].[usp_AddQueueAction]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_AddQueueAction] 
(@UserID BIGINT, @GUID VARCHAR(128), @Result VARCHAR(10) OUTPUT)
AS
BEGIN
	IF EXISTS(SELECT 1 FROM [dbo].[tb_QueueAction] (NOLOCK) WHERE [UserID] = @UserID AND [GUID] = @GUID)
	BEGIN
		SET @Result = 'EXISTS';
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[tb_QueueAction](UserID, [GUID])
		SELECT @UserID, @GUID;
		SET @Result = 'OK';
	END
END

GO
/****** Object:  StoredProcedure [dbo].[usp_DeletePrayer]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_DeletePrayer] 
(@QueueActionGUID VARCHAR(128), @UserID BIGINT, @PrayerID BIGINT)
AS
BEGIN
	UPDATE [dbo].[tb_user_prayer] SET Deleted = 1, TouchedWhen = (datediff(second,'1970-01-01 00:00:00',getutcdate()))
	WHERE UserID = @UserID AND PrayerID = @PrayerID;
END
GO
/****** Object:  StoredProcedure [dbo].[usp_DeletePrayerAmen]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_DeletePrayerAmen] 
(@QueueActionGUID VARCHAR(128), @UserID BIGINT, @PrayerID BIGINT)
AS
BEGIN
	UPDATE [dbo].[tb_user_prayer_amen] SET Deleted = 1, TouchedWhen = (datediff(second,'1970-01-01 00:00:00',getutcdate()))
	WHERE UserID = @UserID AND PrayerID = @PrayerID;
END


GO
/****** Object:  StoredProcedure [dbo].[usp_DeletePrayerAnswered]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_DeletePrayerAnswered] 
(@QueueActionGUID VARCHAR(128), @UserID BIGINT, @AnsweredID BIGINT, @Result VARCHAR(100) OUTPUT)
AS
BEGIN
	IF EXISTS(SELECT 1 FROM [dbo].[tb_QueueAction] (NOLOCK) WHERE [UserID] = @UserID AND [GUID] = @QueueActionGUID)
	BEGIN
		SET @Result = 'EXISTS';
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[tb_QueueAction](UserID, [GUID])
		SELECT @UserID, @QueueActionGUID;

		UPDATE [dbo].[tb_user_prayer_answered] SET TouchedWhen = datediff(second,'1970-01-01 00:00:00',getutcdate()), Deleted = 1 WHERE AnsweredID = @AnsweredID;

		SET @Result = 'OK';
	END
END


GO
/****** Object:  StoredProcedure [dbo].[usp_DeletePrayerComment]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_DeletePrayerComment] 
(@QueueActionGUID VARCHAR(128), @UserID BIGINT, @CommentID BIGINT, @Result VARCHAR(100) OUTPUT)
AS
BEGIN
	IF EXISTS(SELECT 1 FROM [dbo].[tb_QueueAction] (NOLOCK) WHERE [UserID] = @UserID AND [GUID] = @QueueActionGUID)
	BEGIN
		SET @Result = 'EXISTS';
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[tb_QueueAction](UserID, [GUID])
		SELECT @UserID, @QueueActionGUID;

		UPDATE [dbo].[tb_user_prayer_comment] SET TouchedWhen = datediff(second,'1970-01-01 00:00:00',getutcdate()), Deleted = 1 WHERE [CommentID] = @CommentID;

		SET @Result = 'OK';
	END
END


GO
/****** Object:  StoredProcedure [dbo].[usp_DeletePrayerRequest]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_DeletePrayerRequest] 
(@QueueActionGUID VARCHAR(128), @UserID BIGINT, @PrayerRequestID BIGINT, @Result VARCHAR(100) OUTPUT)
AS
BEGIN
	IF EXISTS(SELECT 1 FROM [dbo].[tb_QueueAction] (NOLOCK) WHERE [UserID] = @UserID AND [GUID] = @QueueActionGUID)
	BEGIN
		SET @Result = 'EXISTS';
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[tb_QueueAction](UserID, [GUID])
		SELECT @UserID, @QueueActionGUID;

		UPDATE [dbo].[tb_user_prayer_request] SET TouchedWhen = datediff(second,'1970-01-01 00:00:00',getutcdate()), Deleted = 1 WHERE PrayerRequestID = @PrayerRequestID AND UserID = @UserID;

		SET @Result = 'OK';
	END
END


GO
/****** Object:  StoredProcedure [dbo].[usp_GetCreatedPrayerFromQueueActionGUID]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetCreatedPrayerFromQueueActionGUID] 
(@UserID BIGINT, @QueueActionGUID VARCHAR(128), @PrayerID BIGINT OUTPUT)
AS
BEGIN
	
	SET NOCOUNT ON;
    SELECT @PrayerID = PrayerID FROM [dbo].[tb_user_prayer] (NOLOCK) WHERE UserID = @UserID AND QueueActionGUID = @QueueActionGUID
END


GO
/****** Object:  StoredProcedure [dbo].[usp_GetLatestFriends]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetLatestFriends] 
(@UserID BIGINT, @friends VARCHAR(MAX))
AS
BEGIN
	
	DECLARE @TABLE TABLE(UserID BIGINT)

	INSERT INTO @TABLE
	SELECT Token from [dbo].[udf_Split](@friends, ';')

	SELECT B.DisplayName, B.ProfilePictureURL, B.ID FROM [dbo].[tb_user_friends] AS A 
	INNER JOIN [dbo].[tb_user] AS B ON B.ID = A.FriendID
	WHERE A.UserID = @UserID AND A.[FriendID] NOT IN(SELECT UserID from @TABLE)
END

GO
/****** Object:  StoredProcedure [dbo].[usp_GetLatestFriendsPrayers]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetLatestFriendsPrayers] 
(@UserID BIGINT)
AS
BEGIN
	
	DECLARE @Prayers TABLE(PrayerID BIGINT);

	INSERT INTO @Prayers(PrayerID)
	SELECT TOP 10 A.PrayerID FROM [dbo].[tb_user_prayer] (nolock) AS A 
	LEFT OUTER JOIN [dbo].[tb_user_prayer_tag_friends] (nolock) AS B ON  A.PrayerID = B.PrayerID
	WHERE A.Deleted = 0 AND A.UserID IN (SELECT FriendID FROM [dbo].[tb_user_friends] WHERE UserID = @UserID)
	AND (A.PublicView = 1 OR B.UserID = @UserID)
	Order By A.PrayerID DESC

	SELECT [TouchedWhen] ,[CreatedWhen] ,[UserID] ,[PrayerID] ,[PrayerContent] ,[PublicView] ,[QueueActionGUID]
	       ,CONVERT(XML, (SELECT [UserID], G.[DisplayName], G.[ProfilePictureURL] FROM [dbo].[tb_user_prayer_tag_friends] AS F (NOLOCK) INNER JOIN [dbo].[tb_user] AS G (NOLOCK) ON G.ID = F.UserID WHERE F.PrayerID = A.PrayerID FOR XML PATH('TagFriend'), ROOT('AllTagFriends'))) AS TagFriends
		   ,CONVERT(XML ,(SELECT [AttachmentID] AS [GUID], B.UserID ,[FileName] ,[OriginalFilePath] FROM [dbo].[tb_user_prayer_attachment] AS B (NOLOCK) WHERE B.PrayerID = A.PrayerID FOR XML PATH('Attachment'), ROOT('AllAttachments'))) AS Attachments
		   ,CONVERT(XML ,(SELECT [CommentID], [UserID] AS WhoID, H.DisplayName AS WhoName, H.ProfilePictureURL AS WhoProfilePicture, [Comment] ,C.[CreatedWhen] ,C.[TouchedWhen] FROM [dbo].[tb_user_prayer_comment] AS C (NOLOCK) INNER JOIN [dbo].[tb_user] AS H (NOLOCK) ON H.ID = C.UserID WHERE C.Deleted = 0 AND C.PrayerID = A.PrayerID AND C.UserID = A.UserID FOR XML PATH('Comment'), ROOT('AllComments'))) AS Comment
		   ,Convert(XML ,(SELECT [AnsweredID], A.[UserID] AS WhoID, G.DisplayName AS WhoName, G.ProfilePictureURL AS WhoProfilePicture ,[Answered] ,D.[CreatedWhen] ,D.[TouchedWhen] FROM [dbo].[tb_user_prayer_answered] AS D (NOLOCK) INNER JOIN [dbo].[tb_user] AS G (NOLOCK) ON G.ID = D.UserID WHERE Deleted = 0 AND D.PrayerID = A.PrayerID AND D.UserID = A.UserID FOR XML PATH('Answer'), ROOT('AllAnswers'))) AS Answers
		   ,CONVERT(XML, (SELECT [AmenID] ,[UserID] AS WhoID ,I.DisplayName AS WhoName, I.ProfilePictureURL AS WhoProfilePicture, E.[CreatedWhen] , E.[TouchedWhen] FROM [dbo].[tb_user_prayer_amen] AS E (NOLOCK) INNER JOIN [dbo].[tb_user] AS I (NOLOCK) ON I.ID = E.UserID WHERE E.Deleted = 0 AND E.PrayerID = A.PrayerID FOR XML PATH('Amen'), ROOT('AllAmen'))) AS Amen		   
	FROM [dbo].[tb_user_prayer] As A (NOLOCK)
	WHERE A.[PrayerID] IN (SELECT PrayerID FROM @Prayers) AND A.Deleted = 0
	ORDER BY [PrayerID] DESC

	-- name xml name must be the same as the android object name.
	-- put no lock.
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetLatestOthersPrayers]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetLatestOthersPrayers] 
(@UserID BIGINT)
AS
BEGIN
	
	DECLARE @Prayers TABLE(PrayerID BIGINT);

	INSERT INTO @Prayers(PrayerID)
	SELECT TOP 10 PrayerID FROM [dbo].[tb_user_prayer] 
	WHERE Deleted = 0 AND PublicView = 1 AND UserID <> @UserID AND UserID NOT IN (SELECT FriendID FROM [dbo].[tb_user_friends] WHERE UserID = @UserID)
	Order By PrayerID DESC

	SELECT [TouchedWhen] ,[CreatedWhen] ,[UserID] ,[PrayerID] ,[PrayerContent] ,[PublicView] ,[QueueActionGUID]
	       ,CONVERT(XML, (SELECT [UserID], G.[DisplayName], G.[ProfilePictureURL] FROM [dbo].[tb_user_prayer_tag_friends] AS F (NOLOCK) INNER JOIN [dbo].[tb_user] AS G (NOLOCK) ON G.ID = F.UserID WHERE F.PrayerID = A.PrayerID FOR XML PATH('TagFriend'), ROOT('AllTagFriends'))) AS TagFriends
		   ,CONVERT(XML ,(SELECT [AttachmentID] AS [GUID], B.UserID ,[FileName] ,[OriginalFilePath] FROM [dbo].[tb_user_prayer_attachment] AS B (NOLOCK) WHERE B.PrayerID = A.PrayerID FOR XML PATH('Attachment'), ROOT('AllAttachments'))) AS Attachments
		   ,CONVERT(XML ,(SELECT [CommentID], [UserID] AS WhoID, H.DisplayName AS WhoName, H.ProfilePictureURL AS WhoProfilePicture, [Comment] ,C.[CreatedWhen] ,C.[TouchedWhen] FROM [dbo].[tb_user_prayer_comment] AS C (NOLOCK) INNER JOIN [dbo].[tb_user] AS H (NOLOCK) ON H.ID = C.UserID WHERE C.Deleted = 0 AND C.PrayerID = A.PrayerID AND C.UserID = A.UserID FOR XML PATH('Comment'), ROOT('AllComments'))) AS Comment
		   ,Convert(XML ,(SELECT [AnsweredID], A.[UserID] AS WhoID, G.DisplayName AS WhoName, G.ProfilePictureURL AS WhoProfilePicture ,[Answered] ,D.[CreatedWhen] ,D.[TouchedWhen] FROM [dbo].[tb_user_prayer_answered] AS D (NOLOCK) INNER JOIN [dbo].[tb_user] AS G (NOLOCK) ON G.ID = D.UserID WHERE Deleted = 0 AND D.PrayerID = A.PrayerID AND D.UserID = A.UserID FOR XML PATH('Answer'), ROOT('AllAnswers'))) AS Answers
		   ,CONVERT(XML, (SELECT [AmenID] ,[UserID] AS WhoID ,I.DisplayName AS WhoName, I.ProfilePictureURL AS WhoProfilePicture, E.[CreatedWhen] , E.[TouchedWhen] FROM [dbo].[tb_user_prayer_amen] AS E (NOLOCK) INNER JOIN [dbo].[tb_user] AS I (NOLOCK) ON I.ID = E.UserID WHERE E.Deleted = 0 AND E.PrayerID = A.PrayerID FOR XML PATH('Amen'), ROOT('AllAmen'))) AS Amen		   
	FROM [dbo].[tb_user_prayer] As A (NOLOCK)
	WHERE A.[PrayerID] IN (SELECT PrayerID FROM @Prayers) AND A.Deleted = 0
	ORDER BY [PrayerID] DESC

	-- name xml name must be the same as the android object name.
	-- put no lock.
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetLatestPrayerRequest]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetLatestPrayerRequest] 
(@UserID BIGINT, @pr VARCHAR(MAX))
AS
BEGIN
	
	DECLARE @TABLE TABLE(PrayerRequestID BIGINT)

	INSERT INTO @TABLE
	SELECT Token from [dbo].[udf_Split](@pr, ';')

	SELECT [TouchedWhen]
      ,[CreatedWhen]
      ,[PrayerRequestID]
      ,[Subject]
      ,[Description]
      ,[Answered]
      ,[AnswerComment]
      ,[AnsweredWhen]
	  ,CONVERT(XML ,(SELECT [AttachmentID] AS [GUID], B.UserID ,[FileName] ,[OriginalFilePath] FROM [dbo].[tb_user_prayer_request_attachment] AS B (NOLOCK) WHERE B.PrayerRequestID = A.PrayerRequestID FOR XML PATH('Attachment'), ROOT('AllAttachments'))) AS Attachments
		   
  FROM [pypdb].[dbo].[tb_user_prayer_request] (NOLOCK) AS A
  WHERE [PrayerRequestID] NOT IN (SELECT PrayerRequestID from @TABLE) AND UserID = @UserID

END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetLatestPrayers]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetLatestPrayers] 
(@UserID BIGINT, @PrayerID BIGINT)
AS
BEGIN
	SELECT [TouchedWhen] ,[CreatedWhen] ,[UserID] ,[PrayerID] ,[PrayerContent] ,[PublicView] ,[QueueActionGUID]
	       ,CONVERT(XML, (SELECT [UserID], G.[DisplayName], G.[ProfilePictureURL] FROM [dbo].[tb_user_prayer_tag_friends] AS F (NOLOCK) INNER JOIN [dbo].[tb_user] AS G (NOLOCK) ON G.ID = F.UserID WHERE F.PrayerID = A.PrayerID FOR XML PATH('TagFriend'), ROOT('AllTagFriends'))) AS TagFriends
		   ,CONVERT(XML ,(SELECT [AttachmentID] AS [GUID], B.UserID ,[FileName] ,[OriginalFilePath] FROM [dbo].[tb_user_prayer_attachment] AS B (NOLOCK) WHERE B.PrayerID = A.PrayerID FOR XML PATH('Attachment'), ROOT('AllAttachments'))) AS Attachments
		   ,CONVERT(XML ,(SELECT [CommentID], [UserID] AS WhoID, H.DisplayName AS WhoName, H.ProfilePictureURL AS WhoProfilePicture, [Comment] ,C.[CreatedWhen] ,C.[TouchedWhen] FROM [dbo].[tb_user_prayer_comment] AS C (NOLOCK) INNER JOIN [dbo].[tb_user] AS H (NOLOCK) ON H.ID = C.UserID WHERE C.Deleted = 0 AND C.PrayerID = A.PrayerID AND C.UserID = A.UserID FOR XML PATH('Comment'), ROOT('AllComments'))) AS Comment
		   ,Convert(XML ,(SELECT [AnsweredID], A.[UserID] AS WhoID, G.DisplayName AS WhoName, G.ProfilePictureURL AS WhoProfilePicture ,[Answered] ,D.[CreatedWhen] ,D.[TouchedWhen] FROM [dbo].[tb_user_prayer_answered] AS D (NOLOCK) INNER JOIN [dbo].[tb_user] AS G (NOLOCK) ON G.ID = D.UserID WHERE Deleted = 0 AND D.PrayerID = A.PrayerID AND D.UserID = A.UserID FOR XML PATH('Answer'), ROOT('AllAnswers'))) AS Answers
		   ,CONVERT(XML, (SELECT [AmenID] ,[UserID] AS WhoID ,I.DisplayName AS WhoName, I.ProfilePictureURL AS WhoProfilePicture, E.[CreatedWhen] , E.[TouchedWhen] FROM [dbo].[tb_user_prayer_amen] AS E (NOLOCK) INNER JOIN [dbo].[tb_user] AS I (NOLOCK) ON I.ID = E.UserID WHERE E.Deleted = 0 AND E.PrayerID = A.PrayerID FOR XML PATH('Amen'), ROOT('AllAmen'))) AS Amen		   
	FROM [dbo].[tb_user_prayer] As A (NOLOCK)
	WHERE A.[UserID] = @UserID AND A.[PrayerID] > @PrayerID AND Deleted = 0
	ORDER BY [PrayerID]

	-- name xml name must be the same as the android object name.
	-- put no lock.
END

GO
/****** Object:  StoredProcedure [dbo].[usp_GetPastFriendsPrayers]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetPastFriendsPrayers] 
(@UserID BIGINT, @LastPrayerID BIGINT)
AS
BEGIN


	DECLARE @Prayers TABLE(PrayerID BIGINT);

	INSERT INTO @Prayers(PrayerID)
	SELECT TOP 10 A.PrayerID FROM [dbo].[tb_user_prayer] (nolock) AS A 
	LEFT OUTER JOIN [dbo].[tb_user_prayer_tag_friends] (nolock) AS B ON  A.PrayerID = B.PrayerID
	WHERE A.Deleted = 0 AND A.UserID IN (SELECT FriendID FROM [dbo].[tb_user_friends] WHERE UserID = @UserID)
	AND A.PrayerID < @LastPrayerID AND (A.PublicView = 1 OR B.UserID = @UserID)
	Order By A.PrayerID DESC


	SELECT TOP 10 A.PrayerID FROM [dbo].[tb_user_prayer] (nolock) AS A 
	LEFT OUTER JOIN [dbo].[tb_user_prayer_tag_friends] (nolock) AS B ON  A.PrayerID = B.PrayerID
	WHERE A.Deleted = 0 AND A.UserID IN (SELECT FriendID FROM [dbo].[tb_user_friends] WHERE UserID = @UserID)
	AND A.PrayerID < @LastPrayerID AND (A.PublicView = 1 OR B.UserID = @UserID)
	Order By A.PrayerID DESC

	SELECT [TouchedWhen] ,[CreatedWhen] ,[UserID] ,[PrayerID] ,[PrayerContent] ,[PublicView] ,[QueueActionGUID]
	       ,CONVERT(XML, (SELECT [UserID], G.[DisplayName], G.[ProfilePictureURL] FROM [dbo].[tb_user_prayer_tag_friends] AS F (NOLOCK) INNER JOIN [dbo].[tb_user] AS G (NOLOCK) ON G.ID = F.UserID WHERE F.PrayerID = A.PrayerID FOR XML PATH('TagFriend'), ROOT('AllTagFriends'))) AS TagFriends
		   ,CONVERT(XML ,(SELECT [AttachmentID] AS [GUID], B.UserID ,[FileName] ,[OriginalFilePath] FROM [dbo].[tb_user_prayer_attachment] AS B (NOLOCK) WHERE B.PrayerID = A.PrayerID FOR XML PATH('Attachment'), ROOT('AllAttachments'))) AS Attachments
		   ,CONVERT(XML ,(SELECT [CommentID], [UserID] AS WhoID, H.DisplayName AS WhoName, H.ProfilePictureURL AS WhoProfilePicture, [Comment] ,C.[CreatedWhen] ,C.[TouchedWhen] FROM [dbo].[tb_user_prayer_comment] AS C (NOLOCK) INNER JOIN [dbo].[tb_user] AS H (NOLOCK) ON H.ID = C.UserID WHERE C.Deleted = 0 AND C.PrayerID = A.PrayerID AND C.UserID = A.UserID FOR XML PATH('Comment'), ROOT('AllComments'))) AS Comment
		   ,Convert(XML ,(SELECT [AnsweredID], A.[UserID] AS WhoID, G.DisplayName AS WhoName, G.ProfilePictureURL AS WhoProfilePicture ,[Answered] ,D.[CreatedWhen] ,D.[TouchedWhen] FROM [dbo].[tb_user_prayer_answered] AS D (NOLOCK) INNER JOIN [dbo].[tb_user] AS G (NOLOCK) ON G.ID = D.UserID WHERE Deleted = 0 AND D.PrayerID = A.PrayerID AND D.UserID = A.UserID FOR XML PATH('Answer'), ROOT('AllAnswers'))) AS Answers
		   ,CONVERT(XML, (SELECT [AmenID] ,[UserID] AS WhoID ,I.DisplayName AS WhoName, I.ProfilePictureURL AS WhoProfilePicture, E.[CreatedWhen] , E.[TouchedWhen] FROM [dbo].[tb_user_prayer_amen] AS E (NOLOCK) INNER JOIN [dbo].[tb_user] AS I (NOLOCK) ON I.ID = E.UserID WHERE E.Deleted = 0 AND E.PrayerID = A.PrayerID FOR XML PATH('Amen'), ROOT('AllAmen'))) AS Amen		   
	FROM [dbo].[tb_user_prayer] As A (NOLOCK)
	WHERE A.[PrayerID] IN (SELECT PrayerID FROM @Prayers) AND A.Deleted = 0
	ORDER BY [PrayerID] DESC

	-- name xml name must be the same as the android object name.
	-- put no lock.
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetPastOthersPrayers]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetPastOthersPrayers] 
(@UserID BIGINT, @LastPrayerID BIGINT)
AS
BEGIN


	DECLARE @Prayers TABLE(PrayerID BIGINT);

	INSERT INTO @Prayers(PrayerID)
	SELECT TOP 10 PrayerID FROM [dbo].[tb_user_prayer] 
	WHERE Deleted = 0 AND PublicView = 1 AND PrayerID < @LastPrayerID AND UserID <> @UserID AND UserID NOT IN (SELECT FriendID FROM [dbo].[tb_user_friends] WHERE UserID = @UserID)
	ORDER BY PrayerID DESC

	SELECT [TouchedWhen] ,[CreatedWhen] ,[UserID] ,[PrayerID] ,[PrayerContent] ,[PublicView] ,[QueueActionGUID]
	       ,CONVERT(XML, (SELECT [UserID], G.[DisplayName], G.[ProfilePictureURL] FROM [dbo].[tb_user_prayer_tag_friends] AS F (NOLOCK) INNER JOIN [dbo].[tb_user] AS G (NOLOCK) ON G.ID = F.UserID WHERE F.PrayerID = A.PrayerID FOR XML PATH('TagFriend'), ROOT('AllTagFriends'))) AS TagFriends
		   ,CONVERT(XML ,(SELECT [AttachmentID] AS [GUID], B.UserID ,[FileName] ,[OriginalFilePath] FROM [dbo].[tb_user_prayer_attachment] AS B (NOLOCK) WHERE B.PrayerID = A.PrayerID FOR XML PATH('Attachment'), ROOT('AllAttachments'))) AS Attachments
		   ,CONVERT(XML ,(SELECT [CommentID], [UserID] AS WhoID, H.DisplayName AS WhoName, H.ProfilePictureURL AS WhoProfilePicture, [Comment] ,C.[CreatedWhen] ,C.[TouchedWhen] FROM [dbo].[tb_user_prayer_comment] AS C (NOLOCK) INNER JOIN [dbo].[tb_user] AS H (NOLOCK) ON H.ID = C.UserID WHERE C.Deleted = 0 AND C.PrayerID = A.PrayerID AND C.UserID = A.UserID FOR XML PATH('Comment'), ROOT('AllComments'))) AS Comment
		   ,Convert(XML ,(SELECT [AnsweredID], A.[UserID] AS WhoID, G.DisplayName AS WhoName, G.ProfilePictureURL AS WhoProfilePicture ,[Answered] ,D.[CreatedWhen] ,D.[TouchedWhen] FROM [dbo].[tb_user_prayer_answered] AS D (NOLOCK) INNER JOIN [dbo].[tb_user] AS G (NOLOCK) ON G.ID = D.UserID WHERE Deleted = 0 AND D.PrayerID = A.PrayerID AND D.UserID = A.UserID FOR XML PATH('Answer'), ROOT('AllAnswers'))) AS Answers
		   ,CONVERT(XML, (SELECT [AmenID] ,[UserID] AS WhoID ,I.DisplayName AS WhoName, I.ProfilePictureURL AS WhoProfilePicture, E.[CreatedWhen] , E.[TouchedWhen] FROM [dbo].[tb_user_prayer_amen] AS E (NOLOCK) INNER JOIN [dbo].[tb_user] AS I (NOLOCK) ON I.ID = E.UserID WHERE E.Deleted = 0 AND E.PrayerID = A.PrayerID FOR XML PATH('Amen'), ROOT('AllAmen'))) AS Amen		   
	FROM [dbo].[tb_user_prayer] As A (NOLOCK)
	WHERE A.[PrayerID] IN (SELECT PrayerID FROM @Prayers) AND A.Deleted = 0
	ORDER BY [PrayerID] DESC

	-- name xml name must be the same as the android object name.
	-- put no lock.
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetPrayerAttachmentInformation]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetPrayerAttachmentInformation]
(@AttachmentID BIGINT, @UserID BIGINT, @Filename VARCHAR(128) OUTPUT, @OwnerID BIGINT OUTPUT)
AS
BEGIN

	if EXISTS(SELECT 1 FROM [tb_user_prayer_attachment] WHERE @AttachmentID = AttachmentID AND @UserID = UserID) 
		OR EXISTS(SELECT 1 FROM [dbo].[tb_user_prayer_tag_friends] WHERE PrayerID = (SELECT PrayerID FROM [tb_user_prayer_attachment] WHERE @AttachmentID = AttachmentID) AND UserID = @UserID)
	BEGIN
		
		SELECT @Filename = [FileName], @OwnerID = UserID FROM [tb_user_prayer_attachment] WHERE @AttachmentID = AttachmentID

	END
  
  

END

GO
/****** Object:  StoredProcedure [dbo].[usp_GetUserActivationCode]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetUserActivationCode] 
(@LoginType VARCHAR(15), @UserName VARCHAR(50))
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT LoginType, UserName, A.DisplayName, A.ID, B.ActivationCode FROM [dbo].[tb_user] (NOLOCK) AS A
	INNER JOIN [dbo].[tb_user_onetimecode] (NOLOCK) AS B ON B.UserID = A.ID AND B.[Purpose] = 'Account Activation'
	WHERE B.Expired = 0 AND LoginType = @LoginType AND UserName = @UserName AND [EmailVerification] = 0;

END

GO
/****** Object:  StoredProcedure [dbo].[usp_GetUserInformation]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetUserInformation] 
(@LoginType VARCHAR(15), @UserName VARCHAR(50))
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [LoginType] ,[UserName] ,[ID] ,[DisplayName] ,[ProfilePictureURL]
      ,[Password] ,[MobilePlatform] ,[PushNotificationID] ,[CreatedWhen] ,[TouchedWhen], [HMACHashKey]
	  ,[Country], [Region], [City], EmailVerification
	FROM [dbo].[tb_user] (NOLOCK) WHERE LoginType = @LoginType and UserName = @UserName 


END

GO
/****** Object:  StoredProcedure [dbo].[usp_ResetPassword]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_ResetPassword] 
(@LoginType VARCHAR(15), @Username VARCHAR(50), @Result VARCHAR(200) OUTPUT, @ID BIGINT OUTPUT, @VerificationCode VARCHAR(100) OUTPUT, @DisplayName VARCHAR(200) OUTPUT)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT @ID = ID, @DisplayName = DisplayName
	FROM [dbo].[tb_user] (NOLOCK)
	WHERE LoginType = @LoginType AND UserName = @Username;

	SET @VerificationCode = REPLACE(CONVERT(VARCHAR(36), NEWID()) + '-' + CONVERT(VARCHAR(36), NEWID()) + '-' + CONVERT(VARCHAR(36), NEWID()), '-','');			

	if(@ID IS NULL)
	BEGIN
		SET @Result = 'NOT EXISTS';
	END

    UPDATE [dbo].[tb_user_onetimecode] SET Expired = 1
	WHERE [Purpose] = 'Password Reset' AND UserID = @ID

	INSERT INTO [dbo].[tb_user_onetimecode] (UserID, Purpose, ActivationCode)
	SELECT @ID, 'Password Reset', @VerificationCode;

	SET @Result = 'OK';
END

GO
/****** Object:  StoredProcedure [dbo].[usp_UpdatePrayerAnswered]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_UpdatePrayerAnswered] 
(@QueueActionGUID VARCHAR(128), @UserID BIGINT, @AnsweredID BIGINT, @Answer NVARCHAR(MAX), @TouchedWhen BIGINT, @Result VARCHAR(100) OUTPUT)
AS
BEGIN
	IF EXISTS(SELECT 1 FROM [dbo].[tb_QueueAction] (NOLOCK) WHERE [UserID] = @UserID AND [GUID] = @QueueActionGUID)
	BEGIN
		SET @Result = 'EXISTS';
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[tb_QueueAction](UserID, [GUID])
		SELECT @UserID, @QueueActionGUID;

		UPDATE [dbo].[tb_user_prayer_answered] SET TouchedWhen = @TouchedWhen, Answered = @Answer WHERE AnsweredID = @AnsweredID

		SET @Result = 'OK';
	END
END


GO
/****** Object:  StoredProcedure [dbo].[usp_UpdatePrayerComment]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_UpdatePrayerComment] 
(@QueueActionGUID VARCHAR(128), @UserID BIGINT, @CommentID BIGINT, @Comment NVARCHAR(MAX), @TouchedWhen BIGINT, @Result VARCHAR(100) OUTPUT)
AS
BEGIN
	IF EXISTS(SELECT 1 FROM [dbo].[tb_QueueAction] (NOLOCK) WHERE [UserID] = @UserID AND [GUID] = @QueueActionGUID)
	BEGIN
		SET @Result = 'EXISTS';
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[tb_QueueAction](UserID, [GUID])
		SELECT @UserID, @QueueActionGUID;

		UPDATE [dbo].[tb_user_prayer_comment] SET TouchedWhen = @TouchedWhen, Comment = @Comment WHERE CommentID = @CommentID

		SET @CommentID = Scope_Identity();

		SET @Result = 'OK';
	END
END


GO
/****** Object:  StoredProcedure [dbo].[usp_UpdatePrayerPublicView]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_UpdatePrayerPublicView] 
(@QueueActionGUID VARCHAR(128), @UserID BIGINT, @PrayerID BIGINT, @PublicView BIT, @Result VARCHAR(100) OUTPUT)
AS
BEGIN
	IF EXISTS(SELECT 1 FROM [dbo].[tb_QueueAction] (NOLOCK) WHERE [UserID] = @UserID AND [GUID] = @QueueActionGUID)
	BEGIN
		SET @Result = 'EXISTS';
	END
	ELSE IF EXISTS(SELECT 1 FROM [dbo].[tb_user_prayer] WHERE PrayerID = @PrayerID)
	BEGIN
		INSERT INTO [dbo].[tb_QueueAction](UserID, [GUID])
		SELECT @UserID, @QueueActionGUID;

		UPDATE [dbo].[tb_user_prayer] SET TouchedWhen = (datediff(second,'1970-01-01 00:00:00',getutcdate())), PublicView = @PublicView WHERE PrayerID = @PrayerID;
		SET @Result = 'OK';
	END
	ELSE
	BEGIN
		SET @Result = 'NOTEXISTS';
	END
END
GO
/****** Object:  StoredProcedure [dbo].[usp_UpdatePrayerRequest]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_UpdatePrayerRequest] 
(@QueueActionGUID VARCHAR(128), @UserID BIGINT, @PrayerRequestID BIGINT, @Subject NVARCHAR(50), @Description NVARCHAR(4000), @Answered BIT, @AnswerComment NVARCHAR(4000), @AnsweredWhen BIGINT, @TouchedWhen BIGINT, @Result VARCHAR(100) OUTPUT)
AS
BEGIN
	IF EXISTS(SELECT 1 FROM [dbo].[tb_QueueAction] (NOLOCK) WHERE [UserID] = @UserID AND [GUID] = @QueueActionGUID)
	BEGIN
		SET @Result = 'EXISTS';
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[tb_QueueAction](UserID, [GUID])
		SELECT @UserID, @QueueActionGUID;

		UPDATE [dbo].[tb_user_prayer_request] SET TouchedWhen = @TouchedWhen, [Subject] = @Subject, [Description] = @Description, Answered = @Answered, AnswerComment = @AnswerComment, AnsweredWhen = @AnsweredWhen
		WHERE PrayerRequestID = @PrayerRequestID

		SET @Result = 'OK';
	END
END


GO
/****** Object:  StoredProcedure [dbo].[usp_UpdatePrayerTagFriends]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_UpdatePrayerTagFriends] 
(@QueueActionGUID VARCHAR(128), @UserID BIGINT, @PrayerID BIGINT, @Friends XML, @Result VARCHAR(100) OUTPUT)
AS
BEGIN
	IF EXISTS(SELECT 1 FROM [dbo].[tb_QueueAction] (NOLOCK) WHERE [UserID] = @UserID AND [GUID] = @QueueActionGUID)
	BEGIN
		SET @Result = 'EXISTS';
	END
	ELSE IF EXISTS(SELECT 1 FROM [dbo].[tb_user_prayer] WHERE PrayerID = @PrayerID)
	BEGIN
		INSERT INTO [dbo].[tb_QueueAction](UserID, [GUID])
		SELECT @UserID, @QueueActionGUID;

		DELETE FROM [dbo].[tb_user_prayer_tag_friends] WHERE PrayerID = @PrayerID

		INSERT INTO [dbo].[tb_user_prayer_tag_friends](PrayerID, UserID)
		select @PrayerID, T.N.value('UserID[1]', 'bigint') as [Key]
		FROM @Friends.nodes('/Friends/*') as T(N);

		UPDATE [dbo].[tb_user_prayer] SET TouchedWhen = (datediff(second,'1970-01-01 00:00:00',getutcdate())) WHERE PrayerID = @PrayerID;
		SET @Result = 'OK';
	END
	ELSE
	BEGIN
		SET @Result = 'NOTEXISTS';
	END
END
GO
/****** Object:  StoredProcedure [dbo].[usp_UpdateUserMobileDeviceInformation]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_UpdateUserMobileDeviceInformation] 
(@UserID BIGINT, @MobilePlatform VARCHAR(10), @PushNotificationID VARCHAR(200))
AS
BEGIN
	
	UPDATE [dbo].[tb_user] SET MobilePlatform = @MobilePlatform, PushNotificationID = @PushNotificationID
	WHERE ID = @UserID

END
GO
/****** Object:  StoredProcedure [dbo].[usp_UpdateUserSocialInformation]    Script Date: 27/04/2016 3:47:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_UpdateUserSocialInformation] 
(@UserID BIGINT, @DisplayName NVARCHAR(200), @SocialEmail VARCHAR(50), @ProfilePictureURL VARCHAR(200))
AS
BEGIN
	
	UPDATE [dbo].[tb_user] SET DisplayName = @DisplayName, SocialEmail = @SocialEmail, ProfilePictureURL = @ProfilePictureURL
	WHERE ID = @UserID

END
GO
