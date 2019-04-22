USE [master]
GO

CREATE DATABASE [CustomSearchDB]
GO

begin
EXEC [CustomSearchDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

USE [CustomSearchDB]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SearchResults](
	[url] [nchar](500) NULL,
	[name] [nchar](500) NULL
) ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [CustomSearchDB] SET  READ_WRITE 
GO
