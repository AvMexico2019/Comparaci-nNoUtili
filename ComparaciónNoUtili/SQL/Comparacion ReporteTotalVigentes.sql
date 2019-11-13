 use [ArrendamientoInmueble]
  go
  select [dbo].[ContratosVigentes].FolioContrato as FolioVigente, [dbo].[ReporteTotal].FolioContrato as FolioTotal from [dbo].[ContratosVigentes]
  left join [dbo].[ReporteTotal]
  on	[dbo].[ReporteTotal].FolioContrato = [dbo].[ContratosVigentes].FolioContrato
		and [dbo].[ReporteTotal].TipoContrato = [dbo].[ContratosVigentes].TipoContrato
		--and [dbo].[ReporteTotal].TipoOcupacion = [dbo].[ContratosVigentes].TipoOcupacion
		and [dbo].[ReporteTotal].Propietario = [dbo].[ContratosVigentes].Propietario
		--and [dbo].[ReporteTotal].Municipio = [dbo].[ContratosVigentes].Municipio
		and [dbo].[ReporteTotal].Calle = [dbo].[ContratosVigentes].Calle
		--and [dbo].[ReporteTotal].CodigoPostal = [dbo].[ContratosVigentes].CodigoPostal
		--and [dbo].[ReporteTotal].NumExterior = [dbo].[ContratosVigentes].NumExterior