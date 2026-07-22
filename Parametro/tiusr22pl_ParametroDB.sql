USE [tiusr22pl_ParametroDB]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 CREATE TABLE [dbo].[PARAMETRO](
     [ID]    [varchar](10)  NOT NULL,  -- solo letras mayúsculas, max 10 chars
     [VALOR] [varchar](500) NOT NULL,
     PRIMARY KEY CLUSTERED ([ID] ASC)
 )
 GO


INSERT INTO [dbo].[PARAMETRO] ([ID], [VALOR]) VALUES
('TKNJWT',      '5'),
('TKNREFRESH',  '60'),
('TKNCONFIRM',  '15'),
('FOTOMAXKB',   '1024'),
('DOMINIO',     'cuc.ac.cr'),
('INSTITUCION', 'Colegio Universitario de Cartago'),
('ANIOVIGENCIA','2026'),
('MAXLOGIN',    '5'),
('EMAILREMITE', 'noreply@cuc.ac.cr'),
('ESTDEFECTO',  'INACTIVO')
GO


SELECT ID, VALOR FROM [dbo].[PARAMETRO] ORDER BY ID
GO
