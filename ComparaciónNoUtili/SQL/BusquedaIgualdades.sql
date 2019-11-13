use ArrendamientoInmueble
go
DECLARE @folio nvarchar(50)
DECLARE @ID nvarchar(50)
DECLARE @sql1 nvarchar(max)
Declare @sql2 nvarchar(max)
DECLARE @sql3 nvarchar(max)
DECLARE @campo nvarchar(50)

set @folio = '39450'
set @ID = '1728'
set @campo = 'TipoUsoInmueble'

set @sql1 = 'select [dbo].[ReporteTotal].[' + @campo + '] from  [dbo].[ReporteTotal] where [dbo].[ReporteTotal].[FolioContrato] = ' + @folio
set @sql2 = 'SELECT [dbo].[ContratosVigentes].[' + @campo + '] FROM [dbo].[ContratosVigentes] where [dbo].[ContratosVigentes].[ID] = ' + @ID

set @sql3 = 'select [dbo].[ReporteTotal].[' + @campo + '], [dbo].[ContratosVigentes].[' + @campo + ']  ' +
'from [dbo].[ReporteTotal] join [dbo].[ContratosVigentes] on ' + 
'[dbo].[ReporteTotal].[FolioContrato] = ' + @folio + ' and [dbo].[ContratosVigentes].[ID] = ' + @ID + ' and ' +
'[dbo].[ReporteTotal].[' + @campo + '] = [dbo].[ContratosVigentes].[' + @campo + ']';

execute (@sql1)
execute (@sql2)
EXECUTE (@sql3)
