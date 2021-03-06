USE [Empresa]
GO
ALTER TABLE [dbo].[Turno] DROP CONSTRAINT [FK_Turno_Cliente]
GO
ALTER TABLE [dbo].[Turno] DROP CONSTRAINT [FK_Turno_Cancha]
GO
/****** Object:  Table [dbo].[Turno]    Script Date: 15/7/2020 19:01:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Turno]') AND type in (N'U'))
DROP TABLE [dbo].[Turno]
GO
/****** Object:  Table [dbo].[Cliente]    Script Date: 15/7/2020 19:01:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Cliente]') AND type in (N'U'))
DROP TABLE [dbo].[Cliente]
GO
/****** Object:  Table [dbo].[Cancha]    Script Date: 15/7/2020 19:01:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Cancha]') AND type in (N'U'))
DROP TABLE [dbo].[Cancha]
GO
/****** Object:  Table [dbo].[Cancha]    Script Date: 15/7/2020 19:01:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cancha](
	[NroCancha] [int] IDENTITY(1,1) NOT NULL,
	[NombreCancha] [nvarchar](50) NOT NULL,
	[Habilitada] [bit] NOT NULL,
	[Importe] [float] NOT NULL,
 CONSTRAINT [PK_Cancha] PRIMARY KEY CLUSTERED 
(
	[NroCancha] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cliente]    Script Date: 15/7/2020 19:01:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cliente](
	[Email] [nvarchar](50) NOT NULL,
	[Contraseña] [nvarchar](50) NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[Puntos] [int] NOT NULL,
 CONSTRAINT [PK_Cliente] PRIMARY KEY CLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Turno]    Script Date: 15/7/2020 19:01:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Turno](
	[NroTurno] [int] IDENTITY(1,1) NOT NULL,
	[EmailCliente] [nvarchar](50) NOT NULL,
	[NroCancha] [int] NOT NULL,
	[FechaHora] [datetime] NOT NULL,
 CONSTRAINT [PK_Turno] PRIMARY KEY CLUSTERED 
(
	[NroTurno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Turno]  WITH CHECK ADD  CONSTRAINT [FK_Turno_Cancha] FOREIGN KEY([NroCancha])
REFERENCES [dbo].[Cancha] ([NroCancha])
GO
ALTER TABLE [dbo].[Turno] CHECK CONSTRAINT [FK_Turno_Cancha]
GO
ALTER TABLE [dbo].[Turno]  WITH CHECK ADD  CONSTRAINT [FK_Turno_Cliente] FOREIGN KEY([EmailCliente])
REFERENCES [dbo].[Cliente] ([Email])
GO
ALTER TABLE [dbo].[Turno] CHECK CONSTRAINT [FK_Turno_Cliente]
GO
